using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Services;

namespace WebApi.ActionFilters
{

    /// <summary>
    /// TODO: TO properly used this, I am sending the expiry as well. Then on Client. On app laod would need to login then store token in session. 
    /// On each call, check Session token expiry, if expired, login get new token then call the api
    /// But then everytimes the token expired ... 2 calls would be necessary, login and get token. then call with token
    /// </summary>
    public class BasicAuthTokenRequiredAttribute : ActionFilterAttribute
    {
        private const string Token = "Token";

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            //  Get API key provider
            var provider = filterContext.ControllerContext.Configuration
                .DependencyResolver.GetService(typeof(IBasicAuthTokenServices)) as IBasicAuthTokenServices;

            if (filterContext.Request.Headers.Contains(Token))
            {
                var tokenValue = filterContext.Request.Headers.GetValues(Token).First();

                // Validate Token
                if (provider != null && !provider.ValidateToken(tokenValue))
                {
                    var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };
                    filterContext.Response = responseMessage;
                }
            }
            else
            {
                filterContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }

            base.OnActionExecuting(filterContext);

        }
    }
}