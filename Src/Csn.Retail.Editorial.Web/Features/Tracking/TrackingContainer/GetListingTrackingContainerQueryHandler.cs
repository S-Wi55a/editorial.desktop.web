using System.Linq;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;
using Csn.WebMetrics.Core.Model;
using Csn.WebMetrics.Editorial.Interfaces;

namespace Csn.Retail.Editorial.Web.Features.Tracking.TrackingContainer
{
    [AutoBind]
    public class GetListingTrackingContainerQueryHandler : IQueryHandler<GetListingTrackingContainerQuery, IAnalyticsTrackingContainer>
    {
        private readonly IEditorialListingTrackingContainerProvider _listingTrackingProvider;
        private readonly HttpContextBase _httpContext;
        private readonly IPageContextStore _pageContextStore;
        private readonly IMapper _mapper;

        public GetListingTrackingContainerQueryHandler(IEditorialListingTrackingContainerProvider listingTrackingProvider, 
                                                        HttpContextBase httpContext, 
                                                        IPageContextStore pageContextStore, 
                                                        IMapper mapper)
        {
            _listingTrackingProvider = listingTrackingProvider;
            _httpContext = httpContext;
            _pageContextStore = pageContextStore;
            _mapper = mapper;
        }

        public IAnalyticsTrackingContainer Handle(GetListingTrackingContainerQuery query)
        {
            var pageContext = _pageContextStore.Get();

            var search = pageContext?.PageContextType == PageContextTypes.Listing ? pageContext as ListingPageContext : null;

            var editorialListItems = search?.RyvussNavResult?.SearchResults ?? Enumerable.Empty<SearchResultDto>();

            var results = editorialListItems.Select(_mapper.Map<AnalyticsEditorialTrackingItem>).ToList();

            var searchContext = _mapper.Map<AnalyticsSearchContext>(search);

            return _listingTrackingProvider.GetContainer(results, searchContext, _httpContext);
        }
    }
}