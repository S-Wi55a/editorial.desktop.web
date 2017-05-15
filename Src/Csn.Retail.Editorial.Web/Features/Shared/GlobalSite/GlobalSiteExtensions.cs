using System.Web.Mvc;
using Csn.Retail.AppShellClient;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.TrackingApi;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;

namespace Csn.Retail.Editorial.Web.Features.Shared.GlobalSite
{
    public static class GlobalSiteExtensions
    {
        public static AppShellData GlobalSiteData(this HtmlHelper html)
        {
            return DependencyResolver.Current.GetService<IContextStore<AppShellData>>().Get() ?? new AppShellData();
        }

        public static TrackingApiDto GoogleAnalyticsScript(this HtmlHelper html)
        {
            return DependencyResolver.Current.GetService<IContextStore<TrackingApiDto>>().Get() ?? new TrackingApiDto();
        }
    }
}