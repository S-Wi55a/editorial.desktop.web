using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Helpers
{
    public interface IImageUrlHelper
    {
        string GetImageUrl(string photoPath);
    }

    [AutoBind]
    public class ImageUrlHelper : IImageUrlHelper
    {
        private readonly EditorialSettings _settings;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public ImageUrlHelper(EditorialSettings settings, ITenantProvider<TenantInfo> tenantProvider)
        {
            _settings = settings;
            _tenantProvider = tenantProvider;
        }

        public string GetImageUrl(string photoPath)
        {
            return _settings.ImageServerUrlTemplate.FormatWith(_tenantProvider.Current().ServiceProjection, photoPath.Trim('/'));
        }
    }
}