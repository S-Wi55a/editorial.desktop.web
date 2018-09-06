﻿using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders
{
    [AutoBind]
    public class ListingsTagBuilder : IMediaMotiveTagBuilder
    {
        private readonly ISearchResultContextStore _searchContextStore;
        private readonly IListingsBreadCrumbTagBuilder _breadCrumbTagBuilder;

        public ListingsTagBuilder(ISearchResultContextStore searchContextStore, IListingsBreadCrumbTagBuilder breadCrumbTagBuilder)
        {
            _searchContextStore = searchContextStore;
            _breadCrumbTagBuilder = breadCrumbTagBuilder;
        }

        public IEnumerable<MediaMotiveTag> Build(MediaMotiveTagBuildersQuery query)
        {
            var searchContext = _searchContextStore.Get();

            return BuildTags(searchContext);
        }

        public bool IsApplicable(MediaMotiveTagBuildersQuery query)
        {
            return _searchContextStore.Exists();
        }

        private IEnumerable<MediaMotiveTag> BuildTags(SearchContext searchContext)
        {
            var tagList = new List<MediaMotiveTag>();

            if (searchContext == null) return tagList;

            tagList.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Area,
                searchContext.EditorialPageType == EditorialPageTypes.Homepage ? MediaMotiveAreaNames.EditorialHomePage : MediaMotiveAreaNames.EditorialResultsPage));

            var navResult = searchContext.RyvussNavResult;

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