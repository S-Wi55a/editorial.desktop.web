using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds
{
    [AutoBind]
    public class MediaMotiveAdQueryHandler : IQueryHandler<MediaMotiveAdQuery, MediaMotiveAdViewModel>
    {
        public MediaMotiveAdViewModel Handle(MediaMotiveAdQuery query)
        {
            MediaMotiveAdUnit adUnit;

            // we need to look up the ad unit based on the tileId provided. If the tile does not exist then return null
            if (query.MediaMotiveData.AdUnits == null || !query.MediaMotiveData.AdUnits.TryGetValue(query.TileId.ToLower(), out adUnit))
            {
                return null;
            }

            return new MediaMotiveAdViewModel()
            {
                TileId = adUnit.TileId,
                Description = adUnit.Description,
                Height = adUnit.Height,
                Width = adUnit.Width,
                DataKruxRequired = adUnit.DataKruxRequired,
                ScriptUrl = $"{query.MediaMotiveData.BaseUrl}{query.MediaMotiveData.ScriptPath}{query.MediaMotiveData.CommonTags}{adUnit.Tags}",
                NoScriptUrl = $"{query.MediaMotiveData.BaseUrl}{query.MediaMotiveData.NoScriptPath}{query.MediaMotiveData.CommonTags}{adUnit.Tags}",
                NoScriptImageUrl = $"{query.MediaMotiveData.BaseUrl}{query.MediaMotiveData.NoScriptImagePath}{query.MediaMotiveData.CommonTags}{adUnit.Tags}"
            };
        }
    }
}