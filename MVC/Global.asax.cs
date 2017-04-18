using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MVC.Infrastructure;

namespace MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ActiveTheme"]))
            {
                var activeTheme = ConfigurationManager.AppSettings["ActiveTheme"];
                ViewEngines.Engines.Insert(0, new ThemeViewEngine(activeTheme));
            };

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}
