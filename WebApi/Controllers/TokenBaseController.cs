using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class TokenBaseController : ApiController
    {

        /// <summary>
        /// Get generated Token form client, validate by comparing.
        /// </summary>
        /// <returns></returns>
        [Route("token/validate")]
        [HttpPost]
        public HttpResponseMessage ValidateToken([FromBody] string token)
        {
            var tokenToValidate = token;

            // Ok works... next Bring proj demo SecurityManager into web api and merge with Token and User Services..all the validation will  be on web api side
            // CLient will only generate a token and send over for verification of key and password.
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
    }
}
