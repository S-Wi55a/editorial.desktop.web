using System.Linq;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.Mappers
{
    public interface ISponsoredLinksDataMapper
    {
        bool ShowSponsoredLinks();
    }

    [AutoBind]
    public class SponsoredLinksDataMapper : ISponsoredLinksDataMapper
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public SponsoredLinksDataMapper(ITenantProvider<TenantInfo> _tenantProvider)
        {
            this._tenantProvider = _tenantProvider;
        }

        public bool ShowSponsoredLinks()
        {
            return _tenantProvider.Current().AdUnits.Any(au => SposoredLinkedTiles.Tiles.Contains(au));
        }
    }
}