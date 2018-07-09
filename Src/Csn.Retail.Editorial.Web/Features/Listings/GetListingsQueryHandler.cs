using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;
using Csn.Retail.Editorial.Web.Features.Listings.Mappings;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Mappers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.SimpleCqrs;
using Expresso.Syntax;
using NewRelic.Api.Agent;
using IMapper = Csn.Retail.Editorial.Web.Infrastructure.Mappers.IMapper;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    [AutoBind]
    public class GetListingsQueryHandler : IAsyncQueryHandler<GetListingsQuery, GetListingsResponse>
    {
        
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IMapper _mapper;
        private readonly IPaginationHelper _paginationHelper;
        private readonly ISortingHelper _sortingHelper;
        private readonly ISearchResultContextStore _searchResultContextStore;
        private readonly IExpressionParser _parser;
        private readonly IExpressionFormatter _expressionFormatter;
        private readonly IPolarNativeAdsDataMapper _polarNativeAdsDataMapper;
        private readonly IListingInsightsDataMapper _listingInsightsDataMapper;
        private readonly ISeoDataMapper _seoDataMapper;
        private readonly IRyvussDataService _ryvussDataService;

        public GetListingsQueryHandler(ITenantProvider<TenantInfo> tenantProvider, IMapper mapper, IPaginationHelper paginationHelper,
            ISortingHelper sortingHelper, ISearchResultContextStore searchResultContextStore, IExpressionParser parser, IExpressionFormatter expressionFormatter, IPolarNativeAdsDataMapper polarNativeAdsDataMapper, 
            IListingInsightsDataMapper listingInsightsDataMapper, ISeoDataMapper seoDataMapper, IRyvussDataService ryvussDataService)
        {
            _tenantProvider = tenantProvider;
            _mapper = mapper;
            _paginationHelper = paginationHelper;
            _sortingHelper = sortingHelper;
            _searchResultContextStore = searchResultContextStore;
            _parser = parser;
            _expressionFormatter = expressionFormatter;
            _polarNativeAdsDataMapper = polarNativeAdsDataMapper;
            _listingInsightsDataMapper = listingInsightsDataMapper;
            _seoDataMapper = seoDataMapper;
            _ryvussDataService = ryvussDataService;
        }

        [Transaction]
        public async Task<GetListingsResponse> HandleAsync(GetListingsQuery query)
        {
            if (!_tenantProvider.Current().SupportsSeoFriendlyListings)
            {
                query.Query = string.IsNullOrEmpty(query.Query) ? $"Service.{_tenantProvider.Current().Name}." : query.Query;
            }
            
            // TODO: replace this with a redirect to the rose tree syntax query with keyword in the ryvuss query
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Query = _expressionFormatter.Format(query.QueryExpression?.AppendOrUpdateKeywords(query.Keywords));
            }

            var sortOrder = EditorialSortKeyValues.IsValidSort(query.Sb) ? query.Sb : string.Empty;

            var resultData = await _ryvussDataService.GetNavAndResults(string.IsNullOrEmpty(query.SeoFragment) ? query.Query : query.SeoFragment, true, sortOrder, query.Pg);

            if (resultData == null) return null;

            // check in case there is an equivalent SEO URL that we can redirect to
            if (_tenantProvider.Current().SupportsSeoFriendlyListings 
                && query.EditorialPageType == EditorialPageTypes.Listing
                && !string.IsNullOrEmpty(resultData.Metadata?.Seo) 
                && resultData.Metadata.Seo.Trim('/') != query.SeoFragment.Trim('/'))
            {
                return new GetListingsResponse
                {
                    RedirectRequired = true,
                    RedirectUrl = ListingUrlHelper.GetSeoUrl(resultData.Metadata.Seo, query.Pg, sortOrder)
                };
            }

            var searchContext = new SearchContext
            {
                RyvussNavResult = resultData,
                Query = string.IsNullOrEmpty(query.Query) ? resultData.Metadata?.Query : query.Query,
                Offset = query.Pg,
                Sort = sortOrder,
                SeoFragment = query.SeoFragment,
                SearchEventType = query.SearchEventType,
                EditorialPageType = query.EditorialPageType
            };

            _searchResultContextStore.Set(searchContext);

            var navResults = _mapper.Map<NavResult>(resultData, opt =>
            {
                if (!string.IsNullOrEmpty(sortOrder))
                {
                    opt.Items["sortOrder"] = sortOrder;
                }
            });
            navResults.INav.CurrentAction = ListingUrlHelper.GetQueryString(!string.IsNullOrEmpty(query.SeoFragment) ? query.SeoFragment : query.Query, sortOrder);
            navResults.INav.CurrentUrl = !string.IsNullOrEmpty(query.SeoFragment) ? ListingUrlHelper.GetSeoUrl(query.SeoFragment, query.Pg, sortOrder) :
                ListingUrlHelper.GetPathAndQueryString(query.Query, query.Pg, sortOrder, includeResultsSegment: true);

            return new GetListingsResponse
            {
                RedirectRequired = false,
                ListingsViewModel = new ListingsViewModel
                {
                    NavResults = navResults,
                    Paging = _paginationHelper.GetPaginationData(navResults.Count, query.Pg, sortOrder, query.Query, resultData.Metadata?.Seo, query.Keywords),
                    Sorting = _sortingHelper.GenerateSortByViewModel(string.IsNullOrEmpty(sortOrder) ? EditorialSortKeyValues.ListingPageDefaultSort : sortOrder, !string.IsNullOrEmpty(query.SeoFragment) ? query.SeoFragment : query.Query, query.Keywords, query.SeoFragment),
                    Keyword = !string.IsNullOrEmpty(query.Keywords) ? query.Keywords : _parser.Parse(resultData.Metadata?.Query).GetKeywords(),
                    DisqusSource = _tenantProvider.Current().DisqusSource,
                    PolarNativeAdsData = _polarNativeAdsDataMapper.Map(resultData.INav.BreadCrumbs, MediaMotiveAreaNames.EditorialResultsPage),
                    InsightsData = _listingInsightsDataMapper.Map(searchContext),
                    SeoData = _seoDataMapper.Map(resultData),
                    EditorialPageType = query.EditorialPageType,
                    MediaMotiveModel = new MediaMotiveModel()
                }
            };
        }
    }
}