using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Culture
{
    public static class LanguageResourceValueProvider
    {
        public static string GetValue(string key)
        {
            var tenantProvider = DependencyResolver.Current.GetService<ITenantProvider<TenantInfo>>();
            return LanguageResources.ResourceManager.GetString(key, tenantProvider.Current().Culture);
        }
    }
}