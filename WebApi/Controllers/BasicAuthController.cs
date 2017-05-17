using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Services;
using Services.DTOs;
using Services.Interface;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiBasicAuthenticationFilter] //-> Requires credential Basic Auth
    public class BasicAuthController : ApiController
    {
        #region Private variable.

        private readonly IBasicAuthTokenServices _basicAuthTokenServices;
        private readonly IUserServices _userServices;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize product service instance
        /// </summary>
        public BasicAuthController(IBasicAuthTokenServices basicAuthTokenServices, IUserServices userServices)
        {
            _basicAuthTokenServices = basicAuthTokenServices;
            _userServices = userServices;
        }

        #endregion

        /// <summary>
        /// Authenticates user and returns token with expiry.
        /// </summary>
        /// <returns></returns>
        [Route("login")]
        [Route("authenticate")]
        [Route("get/token")]
        [HttpPost]
        public HttpResponseMessage Authenticate()
        {
            if (System.Threading.Thread.CurrentPrincipal != null && System.Threading.Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity = System.Threading.Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    var userId = basicAuthenticationIdentity.UserId;
                    return GetAuthToken(userId); // TODO, if not use token just return Request.CreateResponse(HttpStatusCode.OK, "Authorized");
                }
            }
            return null;
        }

        /// <summary>
        /// Returns auth token for the validated user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private HttpResponseMessage GetAuthToken(int userId)
        {
            var token = _basicAuthTokenServices.GenerateToken(userId);
            var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
            response.Headers.Add("Token", token.AuthToken);
            response.Headers.Add("TokenIssuedOn", token.IssuedOn.ToString("F"));
            response.Headers.Add("TokenExpiresOn", token.ExpiresOn.ToString("F"));
            response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiresOn");
            return response;
        }

        /// <summary>
        /// For testing as for now - create user.
        /// To be build properly later...
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [Route("createuser")]
        [HttpPost]
        public string Post([FromBody] UserDTO userDto)
        {
            return _userServices.Create(userDto);
        }
    }
}