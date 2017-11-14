using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using ContextStore = Ingress.Web.Common.Abstracts;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders
{
    [AutoBind]
    public class ListingsTagBuilder : IMediaMotiveTagBuilder
    {
        private readonly Ingress.Web.Common.Abstracts.IContextStore _contextStore;
        private readonly IListingsBreadCrumbTagBuilder _breadCrumbTagBuilder;

        public ListingsTagBuilder(ContextStore.IContextStore contextStore, IListingsBreadCrumbTagBuilder breadCrumbTagBuilder)
        {
            _contextStore = contextStore;
            _breadCrumbTagBuilder = breadCrumbTagBuilder;
        }

        public IEnumerable<MediaMotiveTag> Build(MediaMotiveAdQuery query)
        {
            var searchContext = _contextStore.Get(ContextStoreKeys.CurrentSearchResult.ToString()) as RyvussNavResultDto;

            return BuildTags(searchContext);
        }

        public bool IsApplicable(MediaMotiveAdQuery query)
        {
            return _contextStore.Exists(ContextStoreKeys.CurrentSearchResult.ToString());
        }

        private IEnumerable<MediaMotiveTag> BuildTags(RyvussNavResultDto navResult)
        {
            var tagList = new List<MediaMotiveTag>()
            {
                new MediaMotiveTag(SasAdTags.SasAdTagKeys.Area, "searchresults")
            };

            if (navResult == null) return tagList;

            tagList.AddRange(_breadCrumbTagBuilder.BuildTags(navResult));

            var makeTag = tagList.FirstOrDefault(t => t.Name == SasAdTags.SasAdTagKeys.Make);

            if (makeTag != null)
            {
                var modelTag = tagList.FirstOrDefault(t => t.Name == SasAdTags.SasAdTagKeys.Model);

                tagList.Add(new MediaMotiveTag(SasAdTags.SasAdTagKeys.Car, makeTag.Values.FirstOrDefault() + (modelTag != null ? modelTag.Values.FirstOrDefault() : string.Empty)));
            }

            return tagList;
        }
    }
}