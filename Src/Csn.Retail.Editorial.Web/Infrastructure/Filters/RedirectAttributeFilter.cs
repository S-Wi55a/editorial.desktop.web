using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Csn.Retail.Editorial.Web.Infrastructure.Filters
{
    public class RedirectAttributeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction) return;

            var strategies =
                (DependencyResolver.Current.GetServices<IRedirectStrategy>() ?? Enumerable.Empty<IRedirectStrategy>())
                .OrderBy(x => x.Order).ToList();

            foreach (var redirectStrategy in strategies)
            {
                var instruction = redirectStrategy.Apply(filterContext);

                if (!instruction.IsRedirectRequired) continue;

                filterContext.Result = new RedirectResult(instruction.Url, instruction.IsPermanent);

                break;
            }
        }
    }
}