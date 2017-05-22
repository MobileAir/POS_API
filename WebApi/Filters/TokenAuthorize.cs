using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Microsoft.Ajax.Utilities;
using Services.Interface;

namespace WebApi.Filters
{
    public class TokenAuthorize : AuthorizationFilterAttribute
    {
        private const string Token = "Token";
        private const string ClientUserAgent = "Client-User-Agent";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var provider = 
                actionContext.ControllerContext.Configuration.DependencyResolver.GetService(typeof(ITokenAuthServices)) as ITokenAuthServices;
            try
            {
                if (actionContext.Request.Headers.Contains(Token))
                {
                    var tokenValue = actionContext.Request.Headers.GetValues(Token).First();
                    
                    var userAgent = actionContext.Request.Headers.GetValues(ClientUserAgent)?.First(); 
                    
                    // Validate Token
                    if (provider != null && !userAgent.IsNullOrWhiteSpace() && provider.IsTokenValid(tokenValue, userAgent))
                    {
                        // let controller handle the resp, code and obj return
                    }
                    else
                        actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Invalid Request" };

                }
                else
                {
                    actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "No Token!" };
                }
            }
            catch (Exception e)
            {
                // log e
                // TODO: Should not use Try/catch for logic execution! What if this would throw e as wel... then it would dump a huge error revealing lot of info
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Something failed..." }; ;
            }
            base.OnAuthorization(actionContext);
        }
    }

    
}