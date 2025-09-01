using System.Web.Optimization;

namespace MCS.UI
{
    public class BundlesConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            // bundles.Add(new ScriptBundle("~/bundles/jquery")
            //.Include("~/Scripts/jquery.min.js")
            //.Include("~/Scripts/jquery.plugin.min.js")
            //.Include("~/Scripts/jquery.tooltip.min.js")
            //.Include("~/Scripts/jquery.navgoco.min.js")
            //.Include("~/Scripts/jquery.signalR-2.2.0.min.js")
            //);

            // bundles.Add(new ScriptBundle("~/bundles/jqueryui")
            // .Include("~/Scripts/jquery-ui.min.js")
            // );

            // bundles.Add(new ScriptBundle("~/bundles/jqueryval")
            // .Include("~/Scripts/CustomValidation/jquery.validate.min.js")
            // .Include("~/Scripts/CustomValidation/jquery.validate.unobtrusive.min.js")
            //.Include("~/Scripts/jquery-confirm.js")
            // );

            // bundles.Add(new ScriptBundle("~/bundles/custom")
            // .Include("~/Scripts/CustomValidation/CustomValidation.min.js")
            // .Include("~/Scripts/HtmlHelper.min.js")
            // .Include("~/Scripts/ActionButtonsHelper.min.js")
            // .Include("~/Scripts/AutoCompleteScript.min.js")
            // .Include("~/Views/Shared/Scripts/GridViewActions.min.js")
            // //.Include("~/Views/Shared/Scripts/Tree.js")
            // .Include("~/Views/Shared/Scripts/Tree.min.js")
            // .Include("~/Scripts/Common.min.js")
            // .Include("~/Scripts/chart.min.js")
            // .Include("~/Scripts/Collaboration.min.js")
            // .Include("~/Scripts/push.min.js")
            // );

            // bundles.Add(new ScriptBundle("~/bundles/lib")
            //  .Include("~/Content/User/lib/js/owl.carousel.js")
            //  .Include("~/Content/User/lib/js/jquery.toastmessage-min.js")
            //  .Include("~/Content/User/lib/js/autosize.min.js")
            //  .Include("~/Content/User/lib/js/slidebars.min.js")
            //  .Include("~/Content/User/lib/js/plugins.js")
            //  .Include("~/Content/User/lib/js/demo_file_handling.js")
            //  .Include("~/Content/User/lib/js/faq.js")
            //  );

            // bundles.Add(new ScriptBundle("~/bundles/plugin")
            //  .Include("~/Content/User/lib/js/bootstrap/bootstrap.min.js")
            //  .Include("~/Content/User/lib/js/bootstrap/bootstrap-select.min.js")
            //  .Include("~/Content/User/lib/js/jquery.matchHeight-min.js")
            //  .Include("~/Content/User/lib/js/aos.js")
            //  .Include("~/Content/User/lib/js/jquery.slimscroll.min.js")
            //  .Include("~/Content/User/lib/js/app.min.js")
            //  .Include("~/Content/User/lib/plugins/select2/select2.min.js")
            //  .Include("~/Content/User/lib/js/jstree.min.js")
            //  .Include("~/Scripts/grid/gridmvc.min.js")
            //  .Include("~/Scripts/grid/gridmvc-ext.min.js")
            //  .Include("~/Scripts/grid/ladda-bootstrap/ladda.min.js")
            //  .Include("~/Scripts/grid/ladda-bootstrap/spin.min.js")
            // .Include("~/Scripts/grid/URI.min.js")
            //  //.Include("~/Scripts/jquery-confirm.js")
            //  .Include("~/Content/User/lib/js/fileinput.min.js")
            //  .Include("~/Content/User/lib/js/jqueryFileTree.min.js")
            //  .Include("~/Content/User/lib/js/bootstrap-file-input.min.js")
            //  .Include("~/Content/User/lib/js/cropper.min.js")
            //  .Include("~/Content/User/lib/js/slick.min.js")

            //  //.Include("~/Content/User/lib/js/slidebars.min.js")
            //  //.Include("~/Content/User/lib/js/bootstrap-datetimepicker.min.js")
            //  );


            Framework.Controls.BundlingHelper.RegisterBundles(bundles);

            BundleTable.EnableOptimizations = true;
        }


    }
}