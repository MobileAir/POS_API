using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using MVC.Common;
using MVC.DTOs;
using MVC.Filters;

namespace MVC.Controllers
{
    //[TokenAuthCheckFilter]
    [RoutePrefix("tile")]
    public class SaleController : Controller
    {
        // GET: Sale
        [Route("sale")]
        //[Route("sale/{category:int}")]
        public ActionResult Sale(/*int category = 0*/)
        {
            List<ProductDTO> products = null;

            var uri = "v1/Products/all";
            //if (category != 0)
            //    uri = $"v1/Products/category/{category}";

            var apiResponse = new TokenAuthCrudClient().Get<List<ProductDTO>>(uri, Session["Token"].ToString(), Request.UserAgent);
            products = apiResponse?.Data;
            if (products == null)
            {
                // Get - Debug basic error  info - basic handliing better should be done
                if (apiResponse?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new HttpUnauthorizedResult(apiResponse?.ReasonPhrase);// RedirectToAction("Home", "Home"); //new HttpUnauthorizedResult(r?.ReasonPhrase);
                }
                return HttpNotFound(apiResponse?.Exception ?? "Response was null");
            }

            return View(products);
        }


        // TODO: look into
        // http://stackoverflow.com/questions/32610270/how-to-render-partial-view-in-mvc5-via-ajax-call-to-a-controller-and-return-html
        // NEw wer MVC 5 ajax approach....might need to move id inside partial?
        [Route("sale/{category:int}")]
        [HttpGet]
        public JsonResult SaleByCategory(int category = 0)
        {
            List<ProductDTO> products = null;

            var uri = "v1/Products/all";
            if (category != 0)
                uri = $"v1/Products/category/{category}";

            var apiResponse = new TokenAuthCrudClient().Get<List<ProductDTO>>(uri, Session["Token"].ToString(), Request.UserAgent);
            products = apiResponse?.Data;
            if (products == null)
            {
                products = new List<ProductDTO>(); // TODO fix error - ATM this..
                //// Get - Debug basic error  info - basic handliing better should be done
                //if (apiResponse?.StatusCode == HttpStatusCode.Unauthorized)
                //{
                //    return new HttpUnauthorizedResult(apiResponse?.ReasonPhrase);// RedirectToAction("Home", "Home"); //new HttpUnauthorizedResult(r?.ReasonPhrase);
                //}
                //return HttpNotFound(apiResponse?.Exception ?? "Response was null");
            }

            // ReSharper disable once Mvc.PartialViewNotResolved
            return Json(products, JsonRequestBehavior.AllowGet);
        }
    }
}