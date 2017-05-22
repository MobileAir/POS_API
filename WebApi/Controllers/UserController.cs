using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Services.DTOs;
using Services.Interface;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [RoutePrefix("v1/user")]
    public class UserController : ApiController
    {
        private readonly IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [Route("login")]
        [HttpPost]
        [TokenAuthorize]
        public HttpResponseMessage Login()
        {
            object user = null;
            Request.Properties.TryGetValue("UserDTO", out user);
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [Route("register")]
        [AllowAnonymous]
        [HttpPost]
        public HttpResponseMessage Register([FromBody] RegisterDTO registerDto)
        {
            // Split the parts.
            string[] parts = registerDto.Keys.Split(new char[] { ':' });
            if (parts.Length != 2)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Keys.");

            // Get the hash message, username, and timestamp.
            var hash = parts[0];
            var username = new string(parts[1].ToCharArray().Reverse().ToArray());

            var id = _userServices.Register(
                    registerDto.Email,
                    registerDto.Name,
                    hash,
                    username
                );
            if (id == 0)
                Request.CreateResponse(HttpStatusCode.Conflict, id);

            return Request.CreateResponse(HttpStatusCode.Created, id);
        }

    }
}
