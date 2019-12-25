using System.Web;
using System.Web.Optimization;

namespace Channakya_Base
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
"~/Scripts/jquery-message-box.js", "~/Scripts/ch-dialog.js",
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/AdminLTE/dist/js/app.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                  "~/Scripts/ch-dpicker.js"
                ));



            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/font-awesome.css",

              "~/AdminLTE/dist/css/AdminLTE.css",
                      "~/bootstrap/css/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/AdminLTE/css").Include(
                           "~/Content/messagebox.css ",
              "~/Content/ch-dialog.css",
                    "~/AdminLTE/dist/css/AdminLTE.css",
                    "~/bootstrap/css/bootstrap.css",
                    "~/Content/font-awesome.min.css",
                    "~/AdminLTE/dist/css/skins/_all-skins.min.css",
                    "~/AdminLTE/plugins/iCheck/flat/blue.css"

                ));
            bundles.Add(new ScriptBundle("~/bundles/dropzone")
                      .Include("~/Scripts/dropzone.js"));

            bundles.Add(new StyleBundle("~/Content/dropzone-css")
                        .Include("~/Content/dropzone.css"));


           // BundleTable.EnableOptimizations = false;

        }
    }
}
