using System.Linq;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;

namespace Csn.Retail.Editorial.Web.Infrastructure.Filters
{
    public class LatamInherentRedirectFilter: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var editorialSettings = DependencyResolver.Current.GetService<IEditorialRouteSettings>();
            if (string.IsNullOrEmpty(editorialSettings.ListingsSegment)) return;
            
            var path = filterContext.RequestContext.HttpContext.Request.Url?.AbsolutePath;
            if (string.IsNullOrEmpty(path)) return;

            var seoFragments = path.Trim('/').Split('/');
            if (seoFragments.All(seo => seo.ToLower() != editorialSettings.ListingsSegment)) return;

            var redirectedPath = path.Replace($"/{editorialSettings.ListingsSegment}", "");

            filterContext.Result = new RedirectResult($"{redirectedPath}/{filterContext.RequestContext.HttpContext.Request.Url?.Query}", true);
        }
    }
}