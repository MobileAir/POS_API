using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC.Common;
using MVC.DTOs;
using MVC.Filters;

namespace MVC.Controllers
{
    [TokenAuthCheckFilter]
    public class BaseController : Controller
    {
        public BaseController()
        {
            
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.LayoutViewModel = null;
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            // _Sidebar
            List<CategoryDTO> categories = null;

            var r = new TokenAuthCrudClient().Get<List<CategoryDTO>>("v1/Categories/all", Session["Token"].ToString(), Request.UserAgent);
            categories = r?.Data;
            if (categories == null)
            {
                TempData["Categories"] = new List<CategoryDTO>();
            }
            else
            {
                TempData["Categories"] = categories;
            }
        }
        // GET: Base
        public ActionResult Index()
        {
            return View();
        }
    }
}