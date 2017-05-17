using System;
using System.Web.Mvc;
using MVC.Common;
using MVC.DTOs;
using MVC.ViewModels;

namespace MVC.Controllers
{
    [RoutePrefix("user")]
    public class UserController : Controller
    {
        private const string SecurityToken = "Token";

        // GET: Login
        [Route("signin")]
        [HttpGet]
        public ActionResult Login(string token)
        {
            // http://www.primaryobjects.com/2015/05/08/token-based-authentication-for-web-service-apis-in-c-mvc-net/
            // http://www.primaryobjects.com/2010/12/05/web-gardens-web-farms-clouds-and-session-state-in-c-asp-net/
            Session[SecurityToken] = token;
            //return RedirectToAction("Sale", "Sale"); // let js handle the redirection
            return Json(new { data = "sign in succcess!"}, JsonRequestBehavior.AllowGet);
        }

        [Route("signout")]
        public ActionResult Logout()
        {
            Session.Remove(SecurityToken);
            Session.Clear(); // Removes all keys and values from the session-state collection.
            Session.Abandon(); // you lose that specific session and the user will get a new session key. You could use it for example when the user logs out.
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                var responseCookie = Response.Cookies["ASP.NET_SessionId"];
                if (responseCookie != null)
                    responseCookie.Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction("Home", "Home");
        }

        [Route("signup")]
        public ActionResult SignUp() { return View(); }
        [Route("register")]
        [HttpPost]
        public ActionResult Register(string keys, string name, string email)
        {
            var vm = new RegisterDTO() {Email = email, Name = name, Keys = keys};

            var response = new TokenAuthCrudClient().
                    AsyncPost($"v1/user/register", "No token required", Request.UserAgent, vm);
            if (response.Success)
                return Json(new { message = $"Sign up succcess - userId: {response.Message} ", redirect = "/home"});

            return Json(new { message = "Fail to register.." });
            //return RedirectToRoutePermanent("/home", new {message = "Sign up succcess!"});
        }

    }
}