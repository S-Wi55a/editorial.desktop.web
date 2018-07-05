using System.Linq;
using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
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

            if (seoFragments.All(seo => seo.ToLower() != "results")) return;

            var requestedQuery = filterContext.RequestContext.HttpContext.Request.Url?.Query;//filterContext.HttpContext.Request?.QueryString["q"];

            if (seoFragments.Length < 3 || !string.IsNullOrEmpty(requestedQuery)) return;

            var redirectedPath = path.Replace("/results", "");
            filterContext.Result = new RedirectResult($"{redirectedPath}/{filterContext.RequestContext.HttpContext.Request.Url?.Query}", true);
        }
    }
}