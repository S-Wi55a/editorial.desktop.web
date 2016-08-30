using System.Web.Mvc;
using System.Web.Routing;
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
            //routes.AppendTrailingSlash = true;

            routes.IgnoreRoute("nshc/{*pathInfo}"); // This is important for production haproxy and netscalar setup
            routes.IgnoreRoute("compare/dist/{*pathInfo}");
            routes.IgnoreRoute("{*allgif}", new { allgif = @".*\.gif(/.*)?" });
            routes.IgnoreRoute("{*alljpg}", new { alljpg = @".*\.jpg(/.*)?" });
            routes.IgnoreRoute("{*allpng}", new { allpng = @".*\.png(/.*)?" });
            routes.IgnoreRoute("{*allIcons}", new { allIcons = @".*\.ico(/.*)?" });
            routes.IgnoreRoute("{*allCss}", new { allCss = @".*\.css(/.*)?" });
            routes.IgnoreRoute("{*allJs}", new { allJs = @".*\.js(/.*)?" });
            routes.IgnoreRoute("{file}.css");
            routes.IgnoreRoute("{file}.js");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "<NotSet>/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }
    }
}