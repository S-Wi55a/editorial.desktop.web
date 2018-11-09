using System.Linq;
using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Infrastructure.Filters
{
    [AutoBind]
    public class SeoFriendlyRoutesRedirectFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(DependencyResolver.Current.GetService<ITenantProvider<TenantInfo>>().Current().TenantName == "redbook") return;

            var path = filterContext.RequestContext.HttpContext.Request.Url?.AbsolutePath;
            if(string.IsNullOrEmpty(path)) return;

            var seoFragments = path.Trim('/').Split('/');
            var editorialSettings = DependencyResolver.Current.GetService<IEditorialRouteSettings>();

            if (seoFragments.All(seo => seo.ToLower() != editorialSettings.ResultsSegment)) return;

            var requestedQuery = filterContext.HttpContext.Request.QueryString["q"];
            if (seoFragments.Length <= 2 || !string.IsNullOrEmpty(requestedQuery)) return;

            var redirectedPath = path.Replace($"/{editorialSettings.ResultsSegment}", "");
            filterContext.Result = new RedirectResult($"{redirectedPath}/{filterContext.RequestContext.HttpContext.Request.Url?.Query}", true);
        }
    }
}