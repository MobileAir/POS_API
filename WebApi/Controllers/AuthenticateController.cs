using System.Net;
using System.Net.Http;
using System.Web.Http;
using Services;
using Services.DTOs;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ApiAuthenticationFilter] //-> Requires credential Basic Auth
    public class AuthenticateController : ApiController
    {
        #region Private variable.

        private readonly ITokenServices _tokenServices;
        private readonly IUserServices _userServices;

        #endregion

        #region Public Constructor

        /// <summary>
        /// Public constructor to initialize product service instance
        /// </summary>
        public AuthenticateController(ITokenServices tokenServices, IUserServices userServices)
        {
            _tokenServices = tokenServices;
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
            //var token = _tokenServices.GenerateToken(userId);
            var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");
            //response.Headers.Add("Token", token.AuthToken);
            //response.Headers.Add("TokenIssuedOn", token.IssuedOn.ToString("F"));
            //response.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["AuthTokenExpiry"]);
            //response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
            return response;
        }

        
        [Route("createuser")]
        [HttpPost]
        public string Post([FromBody] UserDTO userDto)
        {
            return _userServices.Create(userDto);
        }
    }
}