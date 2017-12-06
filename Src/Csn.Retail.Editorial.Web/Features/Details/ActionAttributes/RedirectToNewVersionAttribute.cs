using System;
using System.Linq;
using System.Web.Mvc;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.Details.ActionAttributes
{
    public class RedirectToNewVersionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var rawSlug = (string)filterContext.RequestContext.RouteData.Values["slug"];

            if (rawSlug.IsNullOrEmpty()) return;
            
            var slug = rawSlug.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();

            if (slug.IsEmpty()) return;

            filterContext.Result = new RedirectResult($"~/editorial/details/{slug}/", true);
        }
    }
}