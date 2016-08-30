using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;

namespace Csn.Retail.Editorial.Web.Features.Shared.GlobalSite
{
    public static class GlobalSiteExtensions
    {
        public static GlobalSiteDataDto GlobalSiteData(this HtmlHelper html)
        {
            return DependencyResolver.Current.GetService<IContextStore<GlobalSiteDataDto>>().Get() ?? new GlobalSiteDataDto();
        }
    }
}