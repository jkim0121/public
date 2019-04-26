using System.Web;
using System.Web.Optimization;

namespace Deg.FrontEndWebServer
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery.2.1.4.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include("~/Scripts/angular.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/angular-resource").Include("~/Scripts/angular-resources.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/dockspawn").Include("~/Scripts/dockspawn.js"));
            bundles.Add(new ScriptBundle("~/bundles/chart").Include("~/Scripts/chart.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/dashboards").Include("~/dashboards.js"));

            bundles.Add(new StyleBundle("~/Content/css").IncludeDirectory("~/Content", "*.css"));
        }
    }
}
