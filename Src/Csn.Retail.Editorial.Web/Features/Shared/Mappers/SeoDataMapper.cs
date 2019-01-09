using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Redirects;

namespace Csn.Retail.Editorial.Web.Features.Shared.Mappers
{
    public interface ISeoDataMapper
    {
        SeoData Map(RyvussNavResultDto ryvussNavResult);
        SeoData MapLandingSeoData(RyvussNavResultDto ryvussNavResult);
    }

    [AutoBind]
    public class SeoDataMapper : ISeoDataMapper
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IRequestContextWrapper _requestContext;

        public SeoDataMapper(ITenantProvider<TenantInfo> tenantProvider, IRequestContextWrapper requestContext)
        {
            _tenantProvider = tenantProvider;
            _requestContext = requestContext;
        }

        public SeoData Map(RyvussNavResultDto ryvussNavResult)
        {
            var protocolAndDomain = $"{_tenantProvider.Current().UrlProtocol}://{_tenantProvider.Current().SiteDomain}";
            return new SeoData
            {
                AllowSeoIndexing = _tenantProvider.Current().AllowSeoIndexOfListing,
                Title = ryvussNavResult.Metadata == null || _tenantProvider.Current().TenantName == "redbook" ? _tenantProvider.Current().DefaultPageTitle : ryvussNavResult.Metadata.Title,
                CanonicalUrl = $"{protocolAndDomain}{_requestContext.Url.AbsolutePath}",
                AlternateUrl = !string.IsNullOrEmpty(_tenantProvider.Current().ListingsAlternateUrl) ? $"{protocolAndDomain}{_tenantProvider.Current().ListingsAlternateUrl}" : string.Empty,
                Description = ryvussNavResult.Metadata == null ? string.Empty : ryvussNavResult.Metadata.Description
            };
        }

        public SeoData MapLandingSeoData(RyvussNavResultDto ryvussNavResult)
        {
            var protocolAndDomain = $"{_tenantProvider.Current().UrlProtocol}://{_tenantProvider.Current().SiteDomain}";
            return new SeoData
            {
                AllowSeoIndexing = _tenantProvider.Current().AllowSeoIndexOfLanding,
                Title = ryvussNavResult.Metadata == null ? _tenantProvider.Current().DefaultPageTitle : ryvussNavResult.Metadata.Title,
                CanonicalUrl = $"{protocolAndDomain}{_requestContext.Url.AbsolutePath}",
                AlternateUrl = string.Empty,
                Description = ryvussNavResult.Metadata == null ? string.Empty : ryvussNavResult.Metadata.Description
            };
        }
    }
}