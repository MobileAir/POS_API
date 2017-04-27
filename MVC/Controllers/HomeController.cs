using System.Web.Mvc;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [Route("dashboard")]
        public ActionResult Dashboard()
        {
            ViewBag.Message = "Dashboard.";

            return View();
        }
    }
}