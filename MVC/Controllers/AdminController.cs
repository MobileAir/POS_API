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

            return View();
        }

        [Route("Reports")]
        public ActionResult Reports()
        {
            ViewBag.Message = "Dashboard.";

            return View();
        }

        [Route("Staff")]
        public ActionResult Staff()
        {
            ViewBag.Message = "Dashboard.";

            return View();
        }

        [Route("Products")]
        public ActionResult Products()
        {
            ViewBag.Message = "Dashboard.";

            return View();
        }
    }
}