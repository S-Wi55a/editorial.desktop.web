using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds
{
    [AutoBind]
    public class MediaMotiveAdQueryHandler : IQueryHandler<MediaMotiveAdQuery, MediaMotiveAdViewModel>
    {
        private readonly IEnumerable<IMediaMotiveTagBuilder> _tagBuilders;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;


        public MediaMotiveAdQueryHandler(IEnumerable<IMediaMotiveTagBuilder> tagBuilders, ITenantProvider<TenantInfo> tenantProvider)
        {
            _tagBuilders = tagBuilders;
            _tenantProvider = tenantProvider;
        }

        public MediaMotiveAdViewModel Handle(MediaMotiveAdQuery query)
        {

            var tags = _tagBuilders
                .Where(builder => builder.IsApplicable(query))
                .SelectMany(x => x.Build(query))
                .Where(x => !x.Name.IsNullOrEmpty())
                .Select(x => $"{x.Name}={string.Join(",", x.Values.NullSafe().Select(v => v.NullSafe()))}").Distinct().ToList();

            var urlargs = string.Join("/", tags);

            // lookup the ad settings for this tile
            if (!MediaMotiveAdSettings.AdTypes.TryGetValue(query.AdSize, out MediaMotiveAdSetting adSetting))
            {
                return null;
            }

            var dimensions = adSetting.AdSize.Dimensions().First();

            return new MediaMotiveAdViewModel()
            {
                TileId = query.TileId.ToString(),
                Description = adSetting.Description.ToString(),
                Height = dimensions.Height,
                Width = dimensions.Width,
                DataKruxRequired = adSetting.DataKruxRequired,
                ScriptUrl = $"//mm.carsales.com.au/carsales/jserver/{urlargs}",
                NoScriptUrl = $"//mm.carsales.com.au/carsales/adclick/{urlargs}",
                NoScriptImageUrl = $"//mm.carsales.com.au/carsales/iserver/{urlargs}",
                DisplayAdsSource = DisplayAdsSource.MediaMotive
            };
        }
    }
}