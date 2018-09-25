using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;
using Csn.Retail.Editorial.Web.Features.Details.RouteConstraints;
using Csn.Retail.Editorial.Web.Features.Shared.RouteConstraints;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.StartUpTasks;

namespace Csn.Retail.Editorial.Web
{
    [AutoBind]
    public class SetupDefaultRouteTask : IStartUpTask
    {
        private readonly IEditorialSettings _editorialSettings;
        private readonly string _basepath;

        public SetupDefaultRouteTask(IEditorialSettings editorialSettings)
        {
            _editorialSettings = editorialSettings;

            _basepath = _editorialSettings.BasePath.Trim('/');
        }

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

            AddRouteWithBasePath(
                routes,
                name: "TrackingRoute",
                url: "Tracking",
                defaults: new { controller = "Tracking" }
            );

            AddRouteWithBasePath(
                routes,
                name: "DisplayAdsRoute",
                url: "DisplayAds",
                defaults: new { controller = "DisplayAds" }
            );

            AddRouteWithBasePath(
                routes,
                name: "NativeAdRoute",
                url: "NativeAd",
                defaults: new { controller = "NativeAd" }
            );

            AddRouteWithBasePath(
                routes,
                name: "MediaMotiveDetailsAdRoute",
                url: "MediaMotiveDetailsAd",
                defaults: new { controller = "MediaMotiveDetailsAd" }
            );

            AddRouteWithBasePath(
                routes,
                name: "MediaMotiveAdRoute",
                url: "MediaMotiveAd",
                defaults: new { controller = "MediaMotiveAd" }
            );

            AddRouteWithBasePath(
                routes,
                name: "LandingHome",
                url: "",
                defaults: new { controller = "Landing", action = "Index" }
            );

            AddRouteWithBasePath(
                routes,
                name: "DetailsV2",
                url: "{*detailsPath}",
                defaults: new { controller = "Details", action = "IndexDetailsV2" },
                constraints: new { detailsPath = new DetailsV2RouteConstraint() }
            );

            AddRouteWithBasePath(
                routes,
                name: "DetailsV1",
                url: "{*detailsPath}",
                defaults: new { controller = "Details", action = "IndexDetailsV1" },
                constraints: new { detailsPath = new DetailsV1RouteConstraint() }
            );

            AddRouteWithBasePath(
                routes,
                name: "LandingManufacturer",
                url: "{*manufacturer}",
                defaults: new { controller = "Landing", action = "Index" }, 
                constraints: new { manufacturer = new ManufacturerRouteConstraint() }
            );

            //TODO: to be removed once legacy url structures are no longer needed
            AddRouteWithBasePath(
                routes,
                name: "DetailsLegacyUrls",
                url: "{*detailsSegments}",
                defaults: new { controller = "Details", action = "RedirectLegacyUrl" },
                constraints: new { detailsSegments = new LegacyDetailsPageRouteConstraint() }
            );

            AddRouteWithBasePath(
                routes,
                name: "ArticleType",
                url: "{*articleType}",
                defaults: new { controller = "Listings", action = "ArticleTypeListing" },
                constraints: new { articleType = new ArticleTypeRouteConstraint() }
            );

            AddRouteWithBasePath(
                routes,
                name: "RedbookHome",
                url: "{*redbookVertical}",
                defaults: new { controller = "Listings", action = "RedbookListing" },
                constraints: new { redbookVertical = new VerticalRouteConstraint() }
            );

            AddRouteWithBasePath(
                routes,
                name: "RedbookResults",
                url: "{redbookVertical}/results",
                defaults: new { controller = "Listings", action = "RedbookListing" },
                constraints: new { redbookVertical = new VerticalRouteConstraint() }
            );

            AddRouteWithBasePath(
                routes,
                name: "ListingPage-pre-international",
                url: "results/{*seoFragment}",
                defaults: new { controller = "Listings", action = "Listing" },
                constraints: new { seoFragment = "(^[\\w-/]*)?" }
            );

            AddRouteWithBasePath(
                routes,
                name: "ListingPage",
                url: "{*seoFragment}",
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

        private void AddRouteWithBasePath(RouteCollection routes, string name, string url, object defaults, object constraints = null)
        {
            routes.MapRoute(
                name: name,
                url: $"{_basepath}/{url}",
                defaults: defaults,
                constraints: constraints
            );
        }
    }
}