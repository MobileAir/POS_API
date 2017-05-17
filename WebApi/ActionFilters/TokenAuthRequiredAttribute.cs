using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Ajax.Utilities;
using Services;
using Services.Interface;

namespace WebApi.ActionFilters
{

    /// <summary>
    /// TODO: TO properly used this, I am sending the expiry as well. Then on Client. On app laod would need to login then store token in session. 
    /// On each call, check Session token expiry, if expired, login get new token then call the api
    /// But then everytimes the token expired ... 2 calls would be necessary, login and get token. then call with token
    /// </summary>
    public class TokenAuthRequiredAttribute : ActionFilterAttribute
    {
        private const string Token = "Token";
        private const string ClientUserAgent = "Client-User-Agent";

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            //  Get API key provider
            var provider = filterContext.ControllerContext.Configuration
                .DependencyResolver.GetService(typeof(ITokenAuthServices)) as ITokenAuthServices;

            try
            {
                if (filterContext.Request.Headers.Contains(Token))
                {
                    var tokenValue = filterContext.Request.Headers.GetValues(Token).First();

                    // TODO: ip... to be tested live
                    var ip =
                        IPAddress.Parse(((HttpContextBase)filterContext.Request.Properties["MS_HttpContext"]).Request.UserHostAddress)
                            .ToString();
                    var userAgent = filterContext.Request.Headers.GetValues(ClientUserAgent)?.First(); // good sent with httpclient request since it returns null with Request.Headers.GetValues("User-Agent")

                    //HTTP_CLIENT_USER_AGENT
                    //var userAgent = ((HttpContextBase)actionContext.Request.Properties["MS_HttpContext"]).Request.Params["HTTP_CLIENT_USER_AGENT"];

                    // Validate Token
                    if (provider != null && !ip.IsNullOrWhiteSpace() && !userAgent.IsNullOrWhiteSpace() && provider.IsTokenValid(tokenValue, ip, userAgent))
                    {
                        // let controller handle the resp, code and obj return
                    }
                    else
                        filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };

                }
                else
                {
                    filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }
            }
            catch (Exception e)
            {
                // log e
                // TODO: Should not use Try/catch for logic execution! What if this would throw e as wel... then it would dump a huge error revealing lot of info
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(filterContext);

        }
    }
}