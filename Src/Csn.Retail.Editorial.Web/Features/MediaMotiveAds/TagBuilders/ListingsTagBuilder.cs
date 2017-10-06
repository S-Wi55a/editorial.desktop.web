using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using IContextStore = Ingress.ContextStores.IContextStore;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders
{
    [AutoBind]
    public class ListingsTagBuilder : IMediaMotiveTagBuilder
    {
        private readonly IContextStore _contextStore;

        public ListingsTagBuilder(IContextStore contextStore)
        {
            _contextStore = contextStore;
        }

        public IEnumerable<MediaMotiveTag> Build(MediaMotiveAdQuery query)
        {
            var searchContext = _contextStore.Get(ContextStoreKeys.CurrentSearchResult.ToString()) as RyvussNavResultDto;

            return BuildTagsFromBreadcrumbs(searchContext?.INav.BreadCrumbs);
        }

        public bool IsApplicable(MediaMotiveAdQuery query)
        {
            return _contextStore.Exists(ContextStoreKeys.CurrentSearchResult.ToString());
        }

        private IEnumerable<MediaMotiveTag> BuildTagsFromBreadcrumbs(List<BreadCrumbDto> breadCrumbs)
        {
            var tagList = new List<MediaMotiveTag>();

            if (breadCrumbs == null || !breadCrumbs.Any())
                return tagList;



            return tagList;
        }
    }
}