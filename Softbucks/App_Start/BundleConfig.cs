using BundleTransformer.Core.Bundles;
using BundleTransformer.Core.Orderers;
using BundleTransformer.Core.Transformers;
using System.Web;
using System.Web.Optimization;

namespace Softbucks
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = false;

            bundles.UseCdn = true;
            var cssTransformer = new CssTransformer();
            var jsTransformer = new JsTransformer();
            var nullOrderer = new NullOrderer();

            var cssBundle = new CustomStyleBundle("~/bundles/css");
            cssBundle.Include("~/Content/Site.less", "~/Content/bootstrap/bootstrap.less");
            cssBundle.Transforms.Add(cssTransformer);
            cssBundle.Orderer = nullOrderer;
            bundles.Add(cssBundle);

            var jqueryBundle = new CustomScriptBundle("~/bundles/jquery");
            jqueryBundle.Include("~/Scripts/jquery-{version}.js");
            jqueryBundle.Transforms.Add(jsTransformer);
            jqueryBundle.Orderer = nullOrderer;
            bundles.Add(jqueryBundle);

            var jqueryvalBundle = new CustomScriptBundle("~/bundles/jqueryval");
            jqueryvalBundle.Include("~/Scripts/jquery.validate*");
            jqueryvalBundle.Transforms.Add(jsTransformer);
            jqueryvalBundle.Orderer = nullOrderer;
            bundles.Add(jqueryvalBundle);


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.

            var modernizrBundle = new CustomScriptBundle("~/bundles/modernizr");
            modernizrBundle.Include("~/Scripts/modernizr-*");
            modernizrBundle.Transforms.Add(jsTransformer);
            modernizrBundle.Orderer = nullOrderer;
            bundles.Add(modernizrBundle);


            var bootstrapBundle = new CustomScriptBundle("~/bundles/bootstrap");
            bootstrapBundle.Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js");
            bootstrapBundle.Transforms.Add(jsTransformer);
            bootstrapBundle.Orderer = nullOrderer;
            bundles.Add(bootstrapBundle);


            var myJSBundle = new CustomScriptBundle("~/bundles/myjs");
            myJSBundle.Include("~/Scripts/ScriptHomeSoftbucks.js");
            myJSBundle.Transforms.Add(jsTransformer);
            myJSBundle.Orderer = nullOrderer;
            bundles.Add(myJSBundle);

            //rating star

            var ratingstarBundle = new CustomScriptBundle("~/bundles/ratingstar");
            ratingstarBundle.Include("~/Content/rating/star-rating.min.js");
            ratingstarBundle.Transforms.Add(jsTransformer);
            ratingstarBundle.Orderer = nullOrderer;
            bundles.Add(ratingstarBundle);

            var cssratingstarBundle = new CustomStyleBundle("~/bundles/cssratingstar");
            cssratingstarBundle.Include("~/Content/rating/star-rating.min.css");
            cssratingstarBundle.Transforms.Add(cssTransformer);
            cssratingstarBundle.Orderer = nullOrderer;
            bundles.Add(cssratingstarBundle);
        }
    }
}
