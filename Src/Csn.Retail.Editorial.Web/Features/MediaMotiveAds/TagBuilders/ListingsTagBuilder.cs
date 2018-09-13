using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.DisplayAds;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders
{
    [AutoBind]
    public class ListingsTagBuilder : IMediaMotiveTagBuilder
    {
        private readonly IPageContextStore _pageContextStore;
        private readonly IListingsBreadCrumbTagBuilder _breadCrumbTagBuilder;

        public ListingsTagBuilder(IPageContextStore pageContextStore, IListingsBreadCrumbTagBuilder breadCrumbTagBuilder)
        {
            _pageContextStore = pageContextStore;
            _breadCrumbTagBuilder = breadCrumbTagBuilder;
        }

        public IEnumerable<MediaMotiveTag> Build(MediaMotiveTagBuildersParams parameters)
        {
            var pageContext = _pageContextStore.Get();
            var listingPageContext = pageContext?.PageContextType == PageContextTypes.Listing ? (ListingPageContext)pageContext : null;

            return BuildTags(listingPageContext);
        }

        public bool IsApplicable(MediaMotiveTagBuildersParams parameters)
        {
            return _pageContextStore.Exists();
        }

        private IEnumerable<MediaMotiveTag> BuildTags(ListingPageContext listingPageContext)
        {
            var tagList = new List<MediaMotiveTag>();

            if (listingPageContext == null) return tagList;

            tagList.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Area,
                listingPageContext.EditorialPageType == EditorialPageTypes.Homepage ? MediaMotiveAreaNames.EditorialHomePage : MediaMotiveAreaNames.EditorialResultsPage));

            var navResult = listingPageContext.RyvussNavResult;

            if (navResult == null) return tagList;

            tagList.AddRange(_breadCrumbTagBuilder.BuildTags(navResult));

            var makeTag = tagList.FirstOrDefault(t => t.Name == SasAdTags.SasAdTagKeys.Make);

            if (makeTag == null) return tagList;
            {
                var modelTag = tagList.FirstOrDefault(t => t.Name == SasAdTags.SasAdTagKeys.Model);
                var marketingTag = tagList.FirstOrDefault(t => t.Name == SasAdTags.SasAdTagKeys.MarketingGroup);
                tagList.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Car, makeTag.Values.FirstOrDefault() + (modelTag != null ? modelTag.Values.FirstOrDefault() : string.Empty) + (marketingTag != null ? marketingTag.Values.FirstOrDefault() : string.Empty)));
            }

            return tagList;
        }
    }
}