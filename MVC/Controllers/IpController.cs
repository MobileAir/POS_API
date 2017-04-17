using System.Web.Mvc;
using MVC.Common;

namespace MVC.Controllers
{
    public class IpController : Controller
    {
        [HttpGet]
        public string Index()
        {
            return TokenAuthRequestManager.GetIP(Request);
        }
    }
}