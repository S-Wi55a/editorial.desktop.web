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
            routes.IgnoreRoute("{*allgif}", new { allgif = @".*\.gif(/.*)?" });
            routes.IgnoreRoute("{*alljpg}", new { alljpg = @".*\.jpg(/.*)?" });
            routes.IgnoreRoute("{*allpng}", new { allpng = @".*\.png(/.*)?" });
            routes.IgnoreRoute("{*allIcons}", new { allIcons = @".*\.ico(/.*)?" });
            routes.IgnoreRoute("{*allCss}", new { allCss = @".*\.css(/.*)?" });
            routes.IgnoreRoute("{*allJs}", new { allJs = @".*\.js(/.*)?" });
            routes.IgnoreRoute("{file}.css");
            routes.IgnoreRoute("{file}.js");

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            var constraintResolver = new DefaultInlineConstraintResolver();
            constraintResolver.ConstraintMap.Add("article-types", typeof(ArticleTypeRouteConstraint));
            constraintResolver.ConstraintMap.Add("redbook-vertical", typeof(RedbookRouteConstraint));

            routes.MapMvcAttributeRoutes(constraintResolver);

            routes.MapRoute(
                name: "Default",
                url: "editorial/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = @"(Tracking|Home)" }
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