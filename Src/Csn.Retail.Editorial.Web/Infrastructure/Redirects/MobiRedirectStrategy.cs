using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Infrastructure.Redirects
{
    public class MobiRedirectStrategy : IRedirectStrategy
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IEnumerable<IMobiDetailsUrlBuilder> _detailsUrlBuilders;
        private readonly ICookieStore _cookieStore;
        private readonly IRequestContextWrapper _requestContextWrapper;

        public MobiRedirectStrategy(ITenantProvider<TenantInfo> tenantProvider,
            IEnumerable<IMobiDetailsUrlBuilder> detailsUrlBuilders,
            ICookieStore cookieStore, 
            IRequestContextWrapper requestContextWrapper)
        {
            _tenantProvider = tenantProvider;
            _detailsUrlBuilders = detailsUrlBuilders;
            _cookieStore = cookieStore;
            _requestContextWrapper = requestContextWrapper;
        }

        public RedirectInstruction Apply(ActionExecutingContext filterContext)
        {
            var editorialId = filterContext.RouteData?.Values["id"]?.ToString() ?? String.Empty;

            if (string.IsNullOrWhiteSpace(editorialId)) return RedirectInstruction.None;

            if (!IsMobiRedirectApplicable()) return RedirectInstruction.None;

            var tenant = _tenantProvider.Current();

            var url = _detailsUrlBuilders?.FirstOrDefault(x => x.IsSupported(tenant.Name))?.Build(editorialId);

            return new RedirectInstruction
            {
                Url = url
            };
        }

        public int Order => 0;

        private bool IsMobiRedirectApplicable()
        {
            // also need to check the domain here....if the domain is bikesales/carsales then we redirect. Otherwise we don't


            var isDesktopForcedByUser = _cookieStore.Get("showas")?.IsSame("dsk") ?? false;

            if (isDesktopForcedByUser) return false;

            var isMobi = _requestContextWrapper.IsMobile();

            return isMobi;
        }
    }
}