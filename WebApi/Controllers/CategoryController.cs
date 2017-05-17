using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using Services.DTOs;
using Services.Interface;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [TokenAuthorize]
    [RoutePrefix("v1/Categories")]
    public class CategoryController : ApiController
    {

        private readonly ICategoryServices _categoryServices;

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize category service instance
        /// </summary>
        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        #endregion

        
        [Route("all")]
        [Route("categories")]
        [HttpGet]
        [EnableQuery]
        public HttpResponseMessage Get()
        {
            var categories = _categoryServices.GetAll();
            
            var categoryDtos = categories as List<CategoryDTO> ?? categories.ToList();
            if (categoryDtos.Any())
                return Request.CreateResponse(HttpStatusCode.OK, categoryDtos);
            
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Categories not found");
        }

        // GET api/category/5 - not anymore useing custom routing below or multiple
        [Route("id/{id:int}")]
        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            var category = _categoryServices.GetById(id);
            if (category != null)
                return Request.CreateResponse(HttpStatusCode.OK, category);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No category found for this id");
        }

        [Route("name/{name}")]
        [HttpGet]
        public HttpResponseMessage GetByName(string name)
        {
            var category = _categoryServices.GetAll().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if (category != null)
                return Request.CreateResponse(HttpStatusCode.OK, category);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No category found for this id");
        }

        // POST api/category
        [Route("add")]
        [Route("post")]
        [HttpPost]
        public CategoryDTO Post([FromBody] CategoryDTO categoryDtos)
        {
            return _categoryServices.Create(categoryDtos);
        }

        // PUT api/category/5
        [Route("update/{id:int}")]
        [Route("put/{id:int}")]
        [HttpPut]
        public bool Put(int id, [FromBody]CategoryDTO categoryDtos)
        {
            if (id > 0)
            {
                return _categoryServices.Update(id, categoryDtos);
            }
            return false;
        }

        // DELETE api/category/5
        [Route("remove/{id}")]
        [Route("delete/{id}")]
        [HttpDelete]
        public bool Delete(int id)
        {
            if (id > 0)
                return _categoryServices.Delete(id);
            return false;
        }
    }
}
