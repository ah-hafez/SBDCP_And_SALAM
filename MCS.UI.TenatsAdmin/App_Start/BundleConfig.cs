using System.Web.Optimization;


namespace MCS.UI.TenantsAdmin
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
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
                      "~/Scripts/AutoCompleteScript.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/lib_1").Include(
                "~/Content/Admin/lib/js/plugins/bootstrap/bootstrap.min.js",
                "~/Content/Admin/lib/js/plugins/icheck/icheck.min.js",
                "~/Content/Admin/lib/js/plugins/mcustomscrollbar/jquery.mCustomScrollbar.min.js",
                "~/Content/Admin/lib/js/plugins/bootstrap/bootstrap-datepicker.js",
                "~/Content/Admin/lib/js/plugins/bootstrap/bootstrap-file-input.js",
                "~/Content/Admin/lib/js/plugins/bootstrap/bootstrap-select.js",
                "~/Content/Admin/lib/js/plugins/tagsinput/jquery.tagsinput.min.js",
                "~/Content/Admin/lib/js/plugins/datatables/jquery.dataTables.min.js",
                "~/Content/Admin/lib/js/plugins/bootstrap/bootstrap-timepicker.min.js",
                "~/Content/Admin/lib/js/plugins/bootstrap/bootstrap-colorpicker.js",
                "~/Content/Admin/lib/js/plugins/dropzone/dropzone.min.js",
                "~/Content/Admin/lib/js/plugins/fileinput/fileinput.min.js",
                "~/Content/Admin/lib/js/plugins/filetree/jqueryFileTree.js",
                "~/Content/Admin/lib/js/plugins/cropper/cropper.min.js",
                "~/Content/Admin/lib/js/plugins/jstree/jstree.min.js",
                "~/Content/Admin/lib/js/plugins/noty/jquery.noty.js",
                "~/Content/Admin/lib/js/plugins/noty/layouts/topCenter.js",
                "~/Content/Admin/lib/js/plugins/noty/layouts/topLeft.js",
                "~/Content/Admin/lib/js/plugins/noty/layouts/topRight.js",
                "~/Content/Admin/lib/js/plugins/noty/themes/default.js"));

            bundles.Add(new ScriptBundle("~/bundles/lib_2").Include(
            "~/Content/Admin/lib/js/plugins.js",
            "~/Content/Admin/lib/js/actions.js",
            "~/Content/Admin/lib/js/demo_file_handling.js",
            "~/Content/Admin/lib/js/plugins/scrolltotop/scrolltopcontrol.js",
            "~/Content/Admin/lib/js/plugins/morris/raphael-min.js",
            "~/Content/Admin/lib/js/plugins/rickshaw/d3.v3.js",
            "~/Content/Admin/lib/js/plugins/rickshaw/rickshaw.min.js",
            "~/Content/Admin/lib/js/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
            "~/Content/Admin/lib/js/plugins/jvectormap/jquery-jvectormap-world-mill-en.js",
            "~/Content/Admin/lib/js/plugins/owl/owl.carousel.min.js",
            "~/Content/Admin/lib/js/plugins/moment.min.js",
            "~/Content/Admin/lib/js/plugins/daterangepicker/daterangepicker.js",
            "~/Content/Admin/lib/js/plugins/highlight/jquery.highlight-4.js",
            "~/Content/Admin/lib/js/faq.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                "~/Scripts/jquery.plugin.js",
                "~/Scripts/jquery.tooltip.js",
                "~/Scripts/jquery.navgoco.js",
                "~/Scripts/CustomValidation/jquery.validate.js",
                "~/Scripts/CustomValidation/jquery.validate.unobtrusive.js",
                "~/Scripts/CustomValidation/CustomValidation.js",
                "~/Scripts/HtmlHelper.js",
                "~/Scripts/ActionButtonsHelper.js",
                "~/Scripts/GridViewActions.js",
                "~/Scripts/Tree.js",
                "~/Scripts/jquery.nestable-rtl.js",
                "~/Scripts/jquery.contextMenu.js",
                "~/Scripts/gridmvc.js",
                "~/Scripts/gridmvc-ext.js",
                "~/Scripts/gridmvc.customwidgets.js",
                "~/Scripts/gridmvc.customwidgetsText.js",
                "~/Scripts/gridmvcAR.js",
                "~/Scripts/URI.js",
                "~/Scripts/Common.js",
                "~/Scripts/AutoCompleteScript.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/lib_3").Include(
                "~/Content/Admin/lib/js/theme/jquery-confirm/js/jquery-confirm.js",
                "~/Content/Admin/lib/js/theme/owlcarousel/owl.carousel.js",
                "~/Content/Admin/lib/js/theme/toastmessage/jquery.toastmessage-min.js"));

            Framework.Controls.BundlingHelper.RegisterBundles(bundles);
            BundleTable.EnableOptimizations = true;
        }
    }
}