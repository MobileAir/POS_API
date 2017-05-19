using System.Web;
using System.Web.Optimization;

namespace MVC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            #region DEFAULT

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                 "~/Scripts/jquery.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            #endregion

            #region THEME/MATERIAL

            // SCRIPTS
            bundles.Add(new ScriptBundle("~/js/theme/material/jquery").Include(
                "~/Scripts/Theme/Material/jquery-3.1.0.min.js",
                 "~/Scripts/jquery.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/js/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/js/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/js/theme/material/bootstrap").Include(
                "~/Scripts/Theme/Material/bootstrap.min.js",
                "~/Scripts/Theme/Material/bootstrap-notify.js"));

            bundles.Add(new ScriptBundle("~/js/theme/material/materialdesign").Include(
                "~/Scripts/Theme/Material/material.min.js",
                "~/Scripts/Theme/Material/material-dashboard.js"));

            bundles.Add(new ScriptBundle("~/js/theme/material/others").Include(
                "~/Scripts/Theme/Material/chartist.min.js",
                "~/Scripts/Theme/Material/demo.js"));


            // CSS
            bundles.Add(new StyleBundle("~/css/theme/material").Include(
                "~/Content/Theme/Material/css/bootstrap.min.css",
                 "~/Content/Theme/Material/css/demo.css",
                  "~/Content/Theme/Material/css/material-dashboard.css"));

            #endregion

        }
    }
}
