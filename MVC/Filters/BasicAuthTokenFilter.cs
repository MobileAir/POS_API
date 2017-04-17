using System;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using MVC.Common;

namespace MVC.Filters
{
    public class BasicAuthTokenFilter : FilterAttribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // nothing for now
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var token = System.Web.HttpContext.Current.Session["Token"];
            var expiresOn = System.Web.HttpContext.Current.Session["TokenExpiresOn"];

            if
                (token == null || token.ToString().IsNullOrWhiteSpace() ||
                expiresOn == null ||
                expiresOn.ToString().IsNullOrWhiteSpace() ||
                DateTime.Parse(expiresOn.ToString()) < DateTime.Now.AddSeconds(10)) // paranoid add 10 sec for server to get/validate
            {
                // get token via Basic Auth: login route
                var tokenH = BasicAuthRequestManager.RequestToken();

                // set token
                System.Web.HttpContext.Current.Session["Token"] = tokenH.Token;
                System.Web.HttpContext.Current.Session["TokenExpiresOn"] = tokenH.ExpiresOn;
            }
        }


    }
}