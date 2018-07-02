using System.Web.Http;

namespace Csn.Retail.Editorial.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "ApiProxy",
                routeTemplate: "editorial/api/v1/proxy",
                defaults: new { Controller = "ApiProxy", action = "Get" }
            );

            config.Routes.MapHttpRoute(
                name: "ApiCarousel",
                routeTemplate: "editorial/api/v1/carousel/",
                defaults: new { Controller = "Carousel", action = "Get" }
            );

            config.Routes.MapHttpRoute(
                name: "GetNav",
                routeTemplate: "editorial/api/v1/search/nav",
                defaults: new { Controller = "Nav", action = "GetNav" }
            );

            config.Routes.MapHttpRoute(
                name: "GetNavRefinements",
                routeTemplate: "editorial/api/v1/search/nav/refinements",
                defaults: new { Controller = "Nav", action = "GetRefinements" }
            );
        }
    }
}
