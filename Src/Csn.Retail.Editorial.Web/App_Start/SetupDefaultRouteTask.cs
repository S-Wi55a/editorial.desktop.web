using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;
using Csn.Retail.Editorial.Web.Features.Shared.RouteConstraints;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.StartUpTasks;

namespace Csn.Retail.Editorial.Web
{
    [AutoBind]
    public class SetupDefaultRouteTask : IStartUpTask
    {
        public void Run()
        {
            var routes = RouteTable.Routes;

            routes.RouteExistingFiles = true;
            routes.AppendTrailingSlash = true;

            routes.IgnoreRoute("nshc/{*pathInfo}"); // This is important for production haproxy and netscalar setup
            routes.IgnoreRoute("compare/dist/{*pathInfo}");
            routes.IgnoreRoute("{*allgif}", new {allgif = @".*\.gif(/.*)?"});
            routes.IgnoreRoute("{*alljpg}", new {alljpg = @".*\.jpg(/.*)?"});
            routes.IgnoreRoute("{*allpng}", new {allpng = @".*\.png(/.*)?"});
            routes.IgnoreRoute("{*allIcons}", new {allIcons = @".*\.ico(/.*)?"});
            routes.IgnoreRoute("{*allCss}", new {allCss = @".*\.css(/.*)?"});
            routes.IgnoreRoute("{*allJs}", new {allJs = @".*\.js(/.*)?"});
            routes.IgnoreRoute("{file}.css");
            routes.IgnoreRoute("{file}.js");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var constraintResolver = new DefaultInlineConstraintResolver();
            routes.MapMvcAttributeRoutes(constraintResolver);

            routes.MapRoute(
                name: "TrackingRoute",
                url: "editorial/Tracking",
                defaults: new { controller = "Tracking" }
            );

            routes.MapRoute(
                name: "NativeAdRoute",
                url: "editorial/NativeAd",
                defaults: new { controller = "NativeAd" }
            );

            routes.MapRoute(
                name: "MediaMotiveDetailsAdRoute",
                url: "editorial/MediaMotiveDetailsAd",
                defaults: new { controller = "MediaMotiveDetailsAd" }
            );

            routes.MapRoute(
                name: "MediaMotiveAdRoute",
                url: "editorial/MediaMotiveAd",
                defaults: new { controller = "MediaMotiveAd" }
            );

            routes.MapRoute(
                name: "LandingHome",
                url: "editorial/",
                defaults: new { controller = "Landing", action = "Index" }
            );

            routes.MapRoute(
                name: "Details",
                url: "editorial/details/{slug}",
                defaults: new { controller = "Details", action = "Index" },
                constraints: new { slug = "^.*-\\d+/?$" }
            );

            routes.MapRoute(
                name: "LandingManufacturer",
                url: "editorial/{*manufacturer}",
                defaults: new { controller = "Landing", action = "Index" }, 
                constraints: new { manufacturer = new ManufacturerRouteConstraint() }
            );

//TODO: to be removed once legacy url structures are no longer needed
            routes.MapRoute(
                name: "DetailsLegacyUrls",
                url: "editorial/{*detailsSegments}",
                defaults: new { controller = "Details", action = "Index" },
                constraints: new { detailsSegments = new LegacyDetailsPageRouteConstraint() }
            );

            routes.MapRoute(
                name: "ArticleType",
                url: "editorial/{*articleType}",
                defaults: new { controller = "Listings", action = "ArticleTypeListing" },
                constraints: new { articleType = new ArticleTypeRouteConstraint() }
            );

            routes.MapRoute(
                name: "RedbookHome",
                url: "editorial/{*redbookVertical}",
                defaults: new { controller = "Listings", action = "RedbookListing" },
                constraints: new { redbookVertical = new VerticalRouteConstraint() }
            );

            routes.MapRoute(
                name: "RedbookResults",
                url: "editorial/{redbookVertical}/results",
                defaults: new { controller = "Listings", action = "RedbookListing" },
                constraints: new { redbookVertical = new VerticalRouteConstraint() }
            );

            routes.MapRoute(
                name: "ListingPage-pre-international",
                url: "editorial/results/{*seoFragment}",
                defaults: new { controller = "Listings", action = "Listing" },
                constraints: new { seoFragment = "(^[\\w-/]*)?" }
            );

            routes.MapRoute(
                name: "ListingPage",
                url: "editorial/{*seoFragment}",
                defaults: new { controller = "Listings", action = "Listing" },
                constraints: new { seoFragment = "(^[\\w-/]*)?" }
            );

            // catch all route....used to catch bad urls. Warning....this will literally capture everything
            routes.MapRoute(
                "UnknownRoute",
                "{*url}",
                new { controller = "Errors", action = "Error404CatchAll" }
            );
        }
    }
}