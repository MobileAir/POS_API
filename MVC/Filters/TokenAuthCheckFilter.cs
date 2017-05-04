using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Ajax.Utilities;

namespace MVC.Filters
{
    public class TokenAuthCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (HttpContext.Current.Session["Token"] == null ||
                    (HttpContext.Current.Session["Token"] != null &&
                     HttpContext.Current.Session["Token"].ToString().IsNullOrWhiteSpace()))
                {
                    HttpContext.Current.Session.Clear();
                    HttpContext.Current.Session.Abandon();
                    filterContext.Result = new HttpUnauthorizedResult("The request does not have an authorized session...");
                    //new RedirectToRouteResult(new RouteValueDictionary("home"));
                    //new HttpUnauthorizedResult("The request does not have an authorized session...");
                }
            }
            catch (Exception e)
            {
                filterContext.Result = new HttpUnauthorizedResult("The request does not have an authorized session...");
            }
        }
    }
}