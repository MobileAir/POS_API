using System;
using System.Web.Mvc;
using MVC.Common;
using MVC.DTOs;
using MVC.ViewModels;

// TODO all redirect to action will not conserve data from view bag...of course! doh!
namespace MVC.Controllers
{
    [RoutePrefix("user")]
    public class UserController : Controller
    {
        private const string SecurityToken = "Token";
        
        [Route("signin")]
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection form)
        {
            try
            {
                var token = form["token"].ToString();

                var response = new TokenAuthCrudClient().Post<UserDTO>($"v1/user/login", token, Request.UserAgent, null);
                if (response.Success)
                {
                    Session[SecurityToken] = token;
                    if (response.Data?.IsSuperUser == true)
                    {
                        Session["SuperUser"] = token;
                        return RedirectToAction("Dashboard", "Home");
                    }
                    //return Json(new {data = "sign in succcess!"}, JsonRequestBehavior.AllowGet);
                    return RedirectToAction("Sale", "Sale");
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = "Issue while trying to log you in. Please try again.";
                return RedirectToAction("Home", "Home");
            }

            ViewBag.Message = "Issue while trying to log you in. Please try again.";
            return RedirectToAction("Home", "Home");
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
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(FormCollection form)
        {
            try
            {
                var email = form["email"].ToString();
                var name = form["name"].ToString();
                var keys = form["keys"].ToString();

                var vm = new RegisterDTO() {Email = email, Name = name, Keys = keys};

                var response = new TokenAuthCrudClient().Post<RegisterDTO>($"v1/user/register", "No token required", Request.UserAgent, vm);
                if (response.Success)
                {
                    ViewBag.Message = "Sign up successful!";
                    return RedirectToAction("Home", "Home");
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = "Issue while trying to sign you up. Please try again.";
                return RedirectToAction("SignUp", "User");
            }

            ViewBag.Message = "Issue while trying to sign you up. Please try again.";
            return RedirectToAction("SignUp", "User");
            //return RedirectToRoutePermanent("/home", new {message = "Sign up succcess!"});
        }

    }
}