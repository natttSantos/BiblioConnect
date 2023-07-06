using System.Web;
using System.Web.Optimization;

namespace BiblioConnect
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //var bundleScript = new ScriptBundle("~/bundles/bootstrap")
            //.Include("~/vendor/jquery/jquery.min.js")
            //.Include("~/vendor/bootstrap/js/bootstrap.bundle.js")
            //.Include("~/vendor/jquery-easing/jquery.easing.min.js")
            //.Include("~/js/sb-admin-2.min.js")
            //.Include("~/vendor/datatables/jquery.dataTables.min.js")
            //.Include("~/vendor/datatables/dataTables.bootstrap4.min.js")
            //.Include("~/Scripts/SweetAlert/sweetalert.min.js")
            //.Include("~/Scripts/jquery-ui.js");


            //bundles.Add(bundleScript);

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
