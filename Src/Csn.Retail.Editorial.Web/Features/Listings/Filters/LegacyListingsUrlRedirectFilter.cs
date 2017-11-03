﻿using System.Linq;
using System.Web.Mvc;
using Csn.Logging;
using Csn.Retail.Editorial.Web.Features.Listings.Helpers;
using Csn.Retail.Editorial.Web.Features.Listings.Logger;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Redirects;

namespace Csn.Retail.Editorial.Web.Features.Listings.Filters
{
    public class LegacyListingsUrlRedirectFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction) return;

            var redirectLogger = DependencyResolver.Current.GetService(typeof(ILegacyListingUrlRedirectLogger)) as LegacyListingUrlRedirectLogger;
            /*This also handles ?Make={make} and ?Type={type} pages since these can be converted to binary expressions*/
            var originalQuery = GetQueryString(filterContext);

            if (originalQuery.IsRyvussBinaryTreeSyntax())
            {
                var url = GetRedirectionUrl(filterContext, originalQuery);
                if (!url.IsNullOrEmpty())
                {
                    redirectLogger?.Log(filterContext.HttpContext.Request.Url?.ToString());
                    filterContext.Result = new RedirectResult(url, true);
                }
            }
        }

        private string GetQueryString(ActionExecutingContext filterContext)
        {
            return filterContext.HttpContext.Request?.QueryString["q"] ??
                   filterContext.HttpContext.Request?.QueryString?.ToString();
        }

        private string GetRedirectionUrl(ActionExecutingContext filterContext, string query)
        {
            long offset = 0;
            long.TryParse(filterContext.HttpContext.Request?.QueryString["offset"] ?? "", out offset);
            var sortOrder = filterContext.HttpContext.Request?.QueryString["sort"] ?? "";
            var keyword = filterContext.HttpContext.Request?.QueryString["Keywords"] ?? "";

            var legacyListingsRedirectHelper =
                DependencyResolver.Current.GetService(typeof(ILegacyListingsRedirectHelper)) as
                    ILegacyListingsRedirectHelper;

            return legacyListingsRedirectHelper?.GetRedirectionUrl(query, offset, sortOrder, keyword);
        }
    }
}