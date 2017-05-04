using System;
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
        //public ActionResult Login(string username, string password, string ip, string userAgent, long ticks)
        public ActionResult Login(string token)
        {
            // TODO: once all good change it into Token auth api template...
            // JS token gen seems to fack when trying HmacSHA512... left it to 256..
            
            //var token = TokenAuthSecurityManager.GenerateToken(username, password, i, ua, t);
            
            // WE LET THE FILTER FOR THE CONTROLLER ACTION CALLED HANDLE IF YES/NO HAS A TOKEN. THEN THE ACTION CALL WILL FAIL IF NOT VALID TOKEN

            //var client = new TokenAuthCrudClient();
            //var authorized = client.Post("token/validate", token, Request.UserAgent);
            //if (!authorized)
            //{
            //    Session.Remove(SecurityToken);
            //    Session.Clear();
            //    Session.Abandon();
            //    return new HttpUnauthorizedResult("Not auth - please try again"); // redirect login or somehting;
            //}
            //else
            {
                Session[SecurityToken] = token;
                //return RedirectToAction("Sale", "Sale");
                return Content("OK");
            }
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