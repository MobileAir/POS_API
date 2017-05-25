using System.Web.Mvc;
using MVC.Filters;

namespace MVC.Controllers
{
    [SuperUserCheckFilter]
    [RoutePrefix("super")]
    public class AdminController : Controller
    {
        [Route("Sales")]
        public ActionResult Sales()
        {
            ViewBag.Message = "Dashboard.";

            return View("Dashboard");
        }

        [Route("Reports")]
        public ActionResult Reports()
        {
            ViewBag.Message = "Dashboard.";

            return View("Dashboard");
        }

        [Route("Staff")]
        public ActionResult Staff()
        {
            ViewBag.Message = "Dashboard.";

            return View("Dashboard");
        }

        [Route("Products")]
        public ActionResult Products()
        {
            ViewBag.Message = "Dashboard.";

            return View("Dashboard");
        }
    }
}