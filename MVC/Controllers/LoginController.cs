using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    /// <summary>
    /// Should simply use ASPNEt identity. Users for this CLient (e.g. this MVC web app) should be stored on the client side. Not in Web APi. Web APi have already his own users for access.
    /// </summary>
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
    }
}