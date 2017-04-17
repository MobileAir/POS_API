using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using Services;
using Services.DTOs;
using WebApi.ActionFilters;

namespace WebApi.Controllers
{
    [TokenAuthRequired]
    //[BasicAuthTokenRequired] // Should be applied manually on controllers or use can be use at action level -> this check first if have a token then valid token
    //ApiBasicAuthenticationFilter: Can be applied in Global.asax for all Controllers => GlobalConfiguration.Configuration.Filters.Add(new ApiAuthenticationFilter());
    //[ApiBasicAuthenticationFilter] // This is Basic Auth only for every req. cal also be used as a Fallback from client. Will try get resource directly Basic Auth and no token header. 
    [RoutePrefix("v1/Products")]
    public class ProductController : ApiController
    {

        private readonly IProductServices _productServices;

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize product service instance
        /// </summary>
        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        #endregion

        
        [Route("all")]
        [Route("products")]
        [HttpGet]
        [EnableQuery]
        //[EnableQuery(PageSize = 500)] // [EnableQuery()] : transform the Odata query into LINQ. LinQ to Entities in this instance
        public HttpResponseMessage Get()
        {
            var products = _productServices.GetAll().AsQueryable();
            if (products != null)
            {
                var productEntities = products as List<ProductDTO> ?? products.ToList();
                if (productEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, productEntities);
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Products not found");
        }

        [Route("allordered")]
        [HttpGet]
        public HttpResponseMessage GetOrderedByName()
        {
            var products = _productServices.GetAll();
            if (products != null)
            {
                var productEntities = products as List<ProductDTO> ?? products.ToList();
                if (productEntities.Any())
                    return Request.CreateResponse(HttpStatusCode.OK, productEntities.OrderByDescending(x => x.Name));
            }
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Products not found");
        }

        // GET api/product/5 - not anymore useing custom routing below or multiple
        [Route("get/{id:int}")]
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var product = _productServices.GetById(id);
            if (product != null)
                return Request.CreateResponse(HttpStatusCode.OK, product);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No product found for this id");
        }

        [Route("getbyname/{name}")]
        [HttpGet]
        public HttpResponseMessage GetByName(string name)
        {
            var product = _productServices.GetAll().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if (product != null)
                return Request.CreateResponse(HttpStatusCode.OK, product);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No product found for this id");
        }

        // POST api/product
        [Route("add")]
        [Route("post")]
        [HttpPost]
        public ProductDTO Post([FromBody] ProductDTO productEntity)
        {
            return _productServices.Create(productEntity);
        }

        // PUT api/product/5
        [Route("update/{id:int}")]
        [Route("put/{id:int}")]
        [HttpPut]
        public bool Put(int id, [FromBody]ProductDTO productEntity)
        {
            if (id > 0)
            {
                return _productServices.Update(id, productEntity);
            }
            return false;
        }

        // DELETE api/product/5
        [Route("remove/{id}")]
        [Route("delete/{id}")]
        [HttpDelete]
        public bool Delete(int id)
        {
            if (id > 0)
                return _productServices.Delete(id);
            return false;
        }
    }
}
