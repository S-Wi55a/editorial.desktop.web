using System;
using System.Collections.Generic;
using System.Linq;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.Models;
using Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.TagBuilders;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive
{
    [AutoBind]
    public class MediaMotiveAdQueryHandler : IQueryHandler<DisplayAdQuery, MediaMotiveAdViewModel>
    {
        private readonly IEnumerable<IMediaMotiveTagBuilder> _tagBuilders;
        private readonly IPageContextStore _pageContextStore;

        public MediaMotiveAdQueryHandler(IEnumerable<IMediaMotiveTagBuilder> tagBuilders, IPageContextStore pageContextStore)
        {
            _tagBuilders = tagBuilders;
            _pageContextStore = pageContextStore;
        }
        public MediaMotiveAdViewModel Handle(DisplayAdQuery displayAdQuery)
        {
            // lookup the ad settings for this type
            if (!MediaMotiveAdSettings.MediaMotiveAdTypes.TryGetValue(displayAdQuery.AdPlacement, out var adSetting))
            {
                return null;
            }

            if (_pageContextStore.Get().PageContextType == PageContextTypes.Details &&
                _pageContextStore.Get() is DetailsPageContext detailsPageContext && adSetting.NotSupportedArticleTypes.Intersect(detailsPageContext.ArticleTypes, StringComparer.OrdinalIgnoreCase).Any())
            {
                return null;
            }

            var mediaMotiveTagBuildersParams = new MediaMotiveTagBuildersParams
            {
                TileId = adSetting.TileId,
                DisplayAdSizes = adSetting.DisplayAdSize
            };

            var tags = _tagBuilders
                .Where(builder => builder.IsApplicable(mediaMotiveTagBuildersParams))
                .SelectMany(x => x.Build(mediaMotiveTagBuildersParams))
                .Where(x => !x.Name.IsNullOrEmpty())
                .Select(x => $"{x.Name}={string.Join(",", x.Values.NullSafe().Select(v => v.NullSafe()))}").Distinct().ToList();

            var urlargs = string.Join("/", tags);

            var dimensions = adSetting.DisplayAdSize.Dimensions().First();

            return new MediaMotiveAdViewModel()
            {
                TileId = adSetting.TileId.ToString(),
                Description = adSetting.Description.ToString(),
                Height = dimensions.Height,
                Width = dimensions.Width,
                DataKruxRequired = adSetting.DataKruxRequired,
                ScriptUrl = $"//mm.carsales.com.au/carsales/jserver/{urlargs}",
                NoScriptUrl = $"//mm.carsales.com.au/carsales/adclick/{urlargs}",
                NoScriptImageUrl = $"//mm.carsales.com.au/carsales/iserver/{urlargs}",
            };
        }
    }
}