using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.Listings.Filters
{
    public class RedbookDefaultVerticalAttributeFilter : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction) return;

            if (DependencyResolver.Current.GetService<ITenantProvider<TenantInfo>>().Current().TenantName == "redbook")
            {
                var vertical = HttpContext.Current.Request.Url.AbsolutePath.Split(new[] { "/editorial/" }, StringSplitOptions.None)[1].Split('/').FirstOrDefault();
                if (!Enum.TryParse<Vertical>(vertical, true, out var _))
                {
                    filterContext.Result = new RedirectResult("/editorial/cars/", true);
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}