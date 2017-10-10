using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Csn.Retail.Editorial.Web.Features.Shared.HtmlHelpers
{
    public static class TenantHtmlHelpers
    {
        public static TenantInfo CurrentTenant(this HtmlHelper html)
        {
            return CurrentTenant();
        }

        public static MvcHtmlString CurrentTenantJson(this HtmlHelper html)
        {
            // For safety reason we don't want to pass all data of tenant only what might needed in UI
            var tenant = CurrentTenant();
            var data = new
            {
                tenant.Name,
                tenant.MediaMotiveAccountId,
                tenant.MediaMotiveUrl
            };
            return MvcHtmlString.Create(JsonConvert.SerializeObject(data, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }));
        }

        private static TenantInfo CurrentTenant()
        {
            return DependencyResolver.Current.GetService<ITenantProvider<TenantInfo>>().Current();
        }
    }
}