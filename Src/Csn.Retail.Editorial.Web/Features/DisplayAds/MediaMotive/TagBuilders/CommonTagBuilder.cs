using System.Collections.Generic;
using System.Linq;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Utils;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.TagBuilders
{
    [AutoBind]
    public class CommonTagBuilder : IMediaMotiveTagBuilder
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public CommonTagBuilder(ITenantProvider<TenantInfo> tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }
        public IEnumerable<MediaMotiveTag> Build(MediaMotiveTagBuildersParams parameters)
        {
            var dimensions = parameters.DisplayAdSizes.Dimensions().ToList();

            return new List<MediaMotiveTag>()
            {
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Site, _tenantProvider.Current().MediaMotiveAccountId),
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Method, "get"),
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Tile, parameters.TileId.ToString()),
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Size, dimensions.Select(x => $"{x.Width}x{x.Height}")),
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Random, RandomNumberGenerator.Generate().ToString()),
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.ViewId, RandomNumberGenerator.Generate().ToString()),
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Certified, string.Empty)
            };
        }

        public bool IsApplicable(MediaMotiveTagBuildersParams parameters)
        {
            // apply this every time
            return true;
        }
    }
}