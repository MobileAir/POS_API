using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace MVC.Attributes
{
    public class TokenAuthCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(HttpContext.Current.Session["Token"] !=null && HttpContext.Current.Session["Token"].ToString().IsNullOrWhiteSpace())
                filterContext.Result = new HttpUnauthorizedResult("The request does not have an authorized session...");
        }
    }
}