using System.Web;
using System.Web.Optimization;

namespace PPIC
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.sparkline.min.js",
                        "~/Scripts/jquery-jvectormap-1.2.2.min.js",
                        "~/Scripts/jquery-jvectormap-world-mill-en.js",
                        "~/Scripts/adminlte/plugins/jQueryUI/jquery-ui.min.js",
                        "~/Scripts/adminlte/components/moment/moment.js",
                        "~/Scripts/adminlte/components/moment/jquery-slimscroll.min.js",
                        "~/Scripts/adminlte/components/fastclick/lib/fastclick.js",
                        "~/Scripts/adminlte/components/fullcalendar/dist/fullcalendar.min.js",
                        "~/Scripts/jquery.dataTables.min.js",
                        "~/Scripts/jquery.validate*"));

            // Scripts\adminlte\components\jquery-slimscroll
            //Scripts\popper.min tooltip.min

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-timepicker.min.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/dataTables.bootstrap.min.js",
                      "~/Scripts/bootstrap3-typeahead.min.js",
                      "~/Scripts/bootstrapAlert.min.js",
                      "~/Scripts/bootstrap-slider.min.js",
                      "~/Scripts/bootstrap3-wysihtml5.all.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/barcode").Include(
                      "~/Scripts/zip.js",
                      "~/Scripts/zip-ext.js",
                      "~/Scripts/deflate.js",
                      "~/Scripts/JSPrintManager.js"));



            bundles.Add(new ScriptBundle("~/bundles/tools").Include(
                      "~/Scripts/fastclick.js",
                      "~/Scripts/adminlte.min.js",
                      "~/Scripts/demo.js",
                      "~/Scripts/fastclick.js",
                      "~/Scripts/Chart.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/Css/font-awesome.min.css",
                      "~/Content/Css/ionicons.min.css",
                      "~/Content/Css/dataTables.bootstrap.min.css",
                      "~/Content/Css/AdminLTE.min.css",
                      "~/Content/Css/_all-skins.min.css",
                      "~/Scripts/adminlte/plugins/iCheck/square/blue.css",
                      "~/Content/Css/jquery-jvectormap.min.css",
                      "~/Content/Css/bootstrap-timepicker.min.css",
                      "~/Content/Css/bootstrap-datepicker.min.css",
                      "~/Content/Css/bootstrap3-wysihtml5.min.css",
                      "~/Content/Css/bootstrap-slider.css"));

            bundles.Add(new StyleBundle("~/Content/site").Include(
                       "~/Content/Site.css"));
        }
    }
}
