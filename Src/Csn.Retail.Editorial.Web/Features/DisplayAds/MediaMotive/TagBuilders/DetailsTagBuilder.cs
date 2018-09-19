using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.TagBuilders
{
    [AutoBind]
    public class DetailsTagBuilder : IMediaMotiveTagBuilder
    {
        private readonly IPageContextStore _pageContextStore;

        public DetailsTagBuilder(IPageContextStore pageContextStore)
        {
            _pageContextStore = pageContextStore;
        }

        public IEnumerable<MediaMotiveTag> Build(MediaMotiveTagBuildersParams parameters)
        {
            var adTags = new List<MediaMotiveTag>();
            var detailsPageContext = _pageContextStore.Get() is DetailsPageContext pageContext ? pageContext : null;
            var items = detailsPageContext?.Items;

            if (items != null && items.Any())
            {
                var item = items.FirstOrDefault();
                if (item != null)
                {
                    adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Car, SasAdTagBuilderExtensions.JoinMakeModelMarketingGroup(item.Make, item.Model, item.MarketingGroup)));
                    adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, SasAdTagBuilderExtensions.Clean(item.Make)));
                    adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Model, SasAdTagBuilderExtensions.Clean(item.Model)));
                }
            }

            // Move these sections outside the Items check, because if an article does not have any item, they won't get included in the commonttags
            adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Lifestyle, SasAdTagBuilderExtensions.Clean(GetLifestyle(detailsPageContext))));
            adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.ArticleType, SasAdTagBuilderExtensions.GetArticleTypeValues(detailsPageContext)));
            adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Category, SasAdTagBuilderExtensions.Clean(GetCategory(detailsPageContext))));
            adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Keyword, SasAdTagBuilderExtensions.Clean(GetKeyword(detailsPageContext))));
            adTags.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Area, MediaMotiveAreaNames.DetailsPage));

            return adTags;
        }

        public bool IsApplicable(MediaMotiveTagBuildersParams parameters)
        {
            return  _pageContextStore.Get().PageContextType == PageContextTypes.Details;
        }

        private static string GetKeyword(DetailsPageContext detailsPageContext)
        {
            // APPS-1086: the "kw" we are looking for is the (first) word within the list of Keywords of the article that have {}
            return detailsPageContext?.Keywords?.Split(',')
                .Select(w => w.Trim())
                .Where(w => w.StartsWith("{") && w.EndsWith("}"))
                .Select(w => w.TrimStart('{').TrimEnd('}'))
                .FirstOrDefault();
        }

        private static string GetLifestyle(DetailsPageContext detailsPageContext)
        {
            return detailsPageContext?.Lifestyles?.FirstOrDefault();
        }

        private static string GetCategory(DetailsPageContext detailsPageContext)
        {
            return detailsPageContext != null && !detailsPageContext.Categories.IsNullOrEmpty() ? string.Join(",", detailsPageContext.Categories) : string.Empty;
        }
    }
}