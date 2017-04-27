using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
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
        public ActionResult Index(string data = null)
        {
            List<ProductVm> products = null;

            //var throwE = int.Parse("hahahahaha");

            if (data != null && TempData["Model"] != null)
            {
                var p = (ProductVm)TempData["Model"];
                ViewBag.Product = $"Success data change to {p.name}";
            }
            var r = new TokenAuthCrudClient().Get<List<ProductVm>>("v1/Products/all", Session["Token"].ToString(), Request.UserAgent);
            products = r?.Data;
            if (products == null)
            {
                // Get - Debug basic error  info - basic handliing better should be done
                if (r?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new HttpUnauthorizedResult(r?.ReasonPhrase);
                }
                return HttpNotFound(r?.Exception ?? "Response was null");
            }
            return View(products);
        }
    }
}