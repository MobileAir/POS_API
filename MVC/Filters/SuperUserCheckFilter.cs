using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;

namespace MVC.Filters
{
    public class SuperUserCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (HttpContext.Current.Session["SuperUser"] == null ||
                    (HttpContext.Current.Session["SuperUser"] != null &&
                     HttpContext.Current.Session["SuperUser"].ToString().IsNullOrWhiteSpace()))
                {
                    filterContext.Result = new HttpUnauthorizedResult("The request does not have permission to access admin panel");
                    //new RedirectToRouteResult(new RouteValueDictionary("home"));
                    //new HttpUnauthorizedResult("The request does not have an authorized session...");
                }
            }
            catch (Exception e)
            {
                filterContext.Result = new HttpUnauthorizedResult("The request does not have permission to access admin panel");
            }
        }
    }
}