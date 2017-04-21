using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.ActionFilters;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [TokenAuthorize]
    //[TokenAuthRequired] // old way... not really correct since action filter would kick in after model binder.... model binding should not occurs if not auth
    public class TokenAuthController : ApiController
    {

        /// <summary>
        /// Get generated Token form client, validate by comparing.
        /// </summary>
        /// <returns></returns>
        [Route("token/validate")]
        [HttpPost]
        public HttpResponseMessage ValidateToken([FromBody] string token)
        {
            //    // The below to capture client request ip??? would need to be tested live!!! Live api and client!
            //    var ip = HttpContext.Current.Request.UserHostAddress;
            //    var userAgent = Request.Headers.GetValues("Client-User-Agent")?.First(); // good sent with httpclient request since it returns null with Request.Headers.GetValues("User-Agent")

            var filterDealtWithIfBadOutcome = true; // IF got to that point mean It passed the [TokenAuthorizationRequired]

            return Request.CreateResponse(HttpStatusCode.OK, filterDealtWithIfBadOutcome);
        }

    }
}
