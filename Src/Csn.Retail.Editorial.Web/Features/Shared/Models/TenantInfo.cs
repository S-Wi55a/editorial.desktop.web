using Csn.MultiTenant;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class TenantInfo : ITenant
    {
        public string Name { get; set; }
        public string MediaMotiveAccountId { get; set; }
        public string MediaMotiveUrl { get; set; }
        public string SiteNavPath { get; set; }
    }
}