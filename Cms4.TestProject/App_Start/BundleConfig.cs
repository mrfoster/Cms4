using System.Web.Optimization;

namespace Cms4.TestProject.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/SiteStyles")
                .Include("~/Content/site.min.css")
                .Include("~/Content/login.min.css"));
        }
    }
}