using MCS.DoconutMVC.Helpers;
using System.Web;
using System.Web.Optimization;

namespace DoconutViewer
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            var annotationsCssUri = WebResourcesHelper.GetWebResourceUrl("MCS.DoconutMVC.Content.Annotations.css");
            bundles.Add(new StyleBundle("~/Content/css").Include(annotationsCssUri));
        }
    }
}
