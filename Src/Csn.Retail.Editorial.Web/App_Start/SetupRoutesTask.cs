using System.Web.Http;
using System.Web.Mvc;
using System.Web.Mvc.Routing;
using System.Web.Routing;
using Csn.Retail.Editorial.Web.Features.Details.RouteConstraints;
using Csn.Retail.Editorial.Web.Features.Redirects;
using Csn.Retail.Editorial.Web.Features.Shared.RouteConstraints;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.StartUpTasks;

namespace Csn.Retail.Editorial.Web
{
    [AutoBind]
    public class SetupRoutesTask : IStartUpTask
    {
        private readonly string _basepath;
        private readonly string _resultsSegment;

        public SetupRoutesTask(IEditorialRouteSettings editorialSettings)
        {
            _basepath = editorialSettings.BasePath.Trim('/');
            _resultsSegment = editorialSettings.ResultsSegment.Trim('/');
        }

        public void Run()
        {
            SetupWebApiRoutes();
            SetupMvcRoutes();
        }

        private void SetupWebApiRoutes()
        {
            GlobalConfiguration.Configuration.MapHttpAttributeRoutes();

            AddWebApiRouteWithBasePath(
                RouteNames.WebApi.ApiProxy,
                "proxy",
                new { Controller = "ApiProxy", action = "Get" }
            );

            AddWebApiRouteWithBasePath(
                RouteNames.WebApi.ApiCarousel,
                "carousel",
                new { Controller = "Carousel", action = "Get" }
            );

            AddWebApiRouteWithBasePath(
                RouteNames.WebApi.GetNav,
                "search/nav",
                new { Controller = "Nav", action = "GetNav" }
            );

            AddWebApiRouteWithBasePath(
                RouteNames.WebApi.GetNavRefinements,
                "search/nav/refinements",
                new { Controller = "Nav", action = "GetRefinements" }
            );

            GlobalConfiguration.Configuration.EnsureInitialized();
        }

        private void AddWebApiRouteWithBasePath(string name, string url, object defaults)
        {
            GlobalConfiguration.Configuration.Routes.MapHttpRoute(
                name: name,
                routeTemplate: $"{_basepath}/api/v1/{url}",
                defaults: defaults
            );
        }

        private void SetupMvcRoutes()
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

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.Error,
                "Error",
                new { controller = "Errors", action = "ErrorGeneric" }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.TrackingRoute,
                "Tracking",
                new { controller = "Tracking" }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.DisplayAdsRoute,
                "DisplayAds",
                new { controller = "DisplayAds" }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.NativeAdRoute,
                "NativeAd",
                new { controller = "NativeAd" }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.MediaMotiveAdRoute,
                "MediaMotiveAd",
                new { controller = "MediaMotiveAd" }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.LandingHome,
                "",
                new { controller = "Landing", action = "Index" }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.DetailsV2,
                "{*detailsPath}",
                new { controller = "Details", action = "IndexDetailsV2" },
                new { detailsPath = new DetailsV2RouteConstraint() }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.DetailsV1,
                "{*detailsPath}",
                new { controller = "Details", action = "IndexDetailsV1" },
                new { detailsPath = new DetailsV1RouteConstraint() }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.DetailsModal,
                "details-modal/{networkId}",
                new { controller = "Details", action = "Modal" }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.LandingManufacturer,
                "{*manufacturer}",
                new { controller = "Landing", action = "Index" }, 
                new { manufacturer = new ManufacturerRouteConstraint() }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.ListingPagePreInternational,
                _resultsSegment,
                new { controller = "Listings", action = "Listing" }
            );

            // eventually we should replace this with something more configurable
            AddMvcRouteWithBasePath(
                RouteNames.Mvc.ArticleType,
                "{*articleType}",
                new { controller = "Listings", action = "ArticleTypeListing" },
                new { articleType = new ArticleTypeRouteConstraint() }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.RedbookHome,
                "{*redbookVertical}",
                new { controller = "Listings", action = "RedbookListing" },
                new { redbookVertical = new VerticalRouteConstraint() }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.RedbookResults,
                "{redbookVertical}/results",
                new { controller = "Listings", action = "RedbookListing" },
                new { redbookVertical = new VerticalRouteConstraint() }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.ConfigBasedRedirect,
                "{*url}",
                new { controller = "Redirect", action = "Redirect" },
                new { url = new ConfigBasedRedirectRouteConstraint() }
            );

            AddMvcRouteWithBasePath(
                RouteNames.Mvc.ListingPage,
                "{*seoFragment}",
                new { controller = "Listings", action = "Listing" },
                new { seoFragment = "(^[\\w-/]*)?" }
            );

            // catch all route....used to catch bad urls. Warning....this will literally capture everything
            routes.MapRoute(
                RouteNames.Mvc.UnknownRoute,
                "{*url}",
                new { controller = "Errors", action = "Error404CatchAll" }
            );
        }

        private void AddMvcRouteWithBasePath(string name, string url, object defaults, object constraints = null)
        {
            RouteTable.Routes.MapRoute(
                name: name,
                url: $"{_basepath}/{url}",
                defaults: defaults,
                constraints: constraints
            );
        }
    }
}