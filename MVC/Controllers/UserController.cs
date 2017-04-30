using System.Web.Mvc;
using MVC.Common;

namespace MVC.Controllers
{
    [RoutePrefix("user")]
    public class UserController : Controller
    {
        private const string SecurityToken = "Token";

        // GET: Login
        [Route("signin")]
        [HttpGet]
        public ActionResult Login(string username, string password, string ip, string userAgent, long ticks)
        {
            var token = TokenAuthSecurityManager.GenerateToken(username, password, ip, userAgent, ticks);
            Session[SecurityToken] = token;

            var client = new TokenAuthCrudClient();
            var authorized = client.Post("token/validate", token, Request.UserAgent);
            if (!authorized)
                return new HttpUnauthorizedResult("Not auth - please try again"); // redirect login or somehting;
            return RedirectToAction("Home", "Home");
        }

        [Route("signout")]
        public ActionResult Logout()
        {
            Session.Remove(SecurityToken);
            Session.Clear(); // Removes all keys and values from the session-state collection.
            Session.Abandon(); // you lose that specific session and the user will get a new session key. You could use it for example when the user logs out.

            return RedirectToAction("Home", "Home");
        }

    }
}