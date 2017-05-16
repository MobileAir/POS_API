using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MVC.Common;
using MVC.Filters;
using MVC.ViewModels;

namespace MVC.Controllers
{
    [TokenAuthCheckFilter]
    [RoutePrefix("tile")]
    public class SaleController : Controller
    {
        // GET: Sale
        [Route("sale")]
        [Route("sale/{category:int}")]
        public ActionResult Sale(int category = 0)
        {
            List<ProductVm> products = null;

            var uri = "v1/Products/all";
            if (category != 0)
                uri = $"v1/Products/category/{category}";

            var apiResponse = new TokenAuthCrudClient().Get<List<ProductVm>>(uri, Session["Token"].ToString(), Request.UserAgent);
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
    }
}