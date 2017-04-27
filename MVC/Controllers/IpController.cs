using System.Web.Mvc;
using MVC.Common;

namespace MVC.Controllers
{
    [RoutePrefix("station")]
    public class IpController : Controller
    {
        [Route("ip")]
        [HttpGet]
        public string Index()
        {
            return TokenAuthRequestManager.GetIP(Request);
        }
    }
}