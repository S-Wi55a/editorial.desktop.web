using System.Collections.Generic;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;
using Csn.Retail.Editorial.Web.Features.Listings.Mappings;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.Mappers;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.SimpleCqrs;
using Expresso.Syntax;
using IMapper = Csn.Retail.Editorial.Web.Infrastructure.Mappers.IMapper;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    [AutoBind]
    public class GetListingsQueryHandler : IAsyncQueryHandler<GetListingsQuery, GetListingsResponse>
    {
        private readonly IEditorialRyvussApiProxy _ryvussProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IMapper _mapper;
        private readonly IPaginationHelper _paginationHelper;
        private readonly ISortingHelper _sortingHelper;
        private readonly ISearchResultContextStore _searchResultContextStore;
        private readonly IExpressionParser _parser;
        private readonly IExpressionFormatter _expressionFormatter;
        private readonly IPolarNativeAdsDataMapper _polarNativeAdsDataMapper;
        private readonly ISponsoredLinksDataMapper _sponsoredLinksDataMapper;
        private readonly IListingInsightsDataMapper _listingInsightsDataMapper;
        private readonly ISeoDataMapper _seoDataMapper;

        public GetListingsQueryHandler(IEditorialRyvussApiProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider, IMapper mapper, IPaginationHelper paginationHelper,
            ISortingHelper sortingHelper, ISearchResultContextStore searchResultContextStore, IExpressionParser parser, IExpressionFormatter expressionFormatter, IPolarNativeAdsDataMapper polarNativeAdsDataMapper, 
            ISponsoredLinksDataMapper sponsoredLinksDataMapper, IListingInsightsDataMapper listingInsightsDataMapper, ISeoDataMapper seoDataMapper)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
            _paginationHelper = paginationHelper;
            _sortingHelper = sortingHelper;
            _searchResultContextStore = searchResultContextStore;
            _parser = parser;
            _expressionFormatter = expressionFormatter;
            _polarNativeAdsDataMapper = polarNativeAdsDataMapper;
            _sponsoredLinksDataMapper = sponsoredLinksDataMapper;
            _listingInsightsDataMapper = listingInsightsDataMapper;
            _seoDataMapper = seoDataMapper;
        }

        
        public async Task<GetListingsResponse> HandleAsync(GetListingsQuery query)
        {
            if (!_tenantProvider.Current().SupportsSeoFriendlyListings)
            {
                query.Query = string.IsNullOrEmpty(query.Query) ? $"Service.{_tenantProvider.Current().Name}." : query.Query;
            }
            
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Query = _expressionFormatter.Format(query.QueryExpression?.AppendOrUpdateKeywords(query.Keywords));
            }

            var sortOrder = EditorialSortKeyValues.IsValidSort(query.Sort) ? query.Sort : EditorialSortKeyValues.ListingPageDefaultSort;

            var postProcessors = new List<string>();

            if (_tenantProvider.Current().SupportsSeoFriendlyListings)
            {
                postProcessors.Add("Seo");
            }

            postProcessors.AddRange(new []{"Retail", "FacetSort", "ShowZero"});

            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput
            {
                Query = string.IsNullOrEmpty(query.SeoFragment) ? query.Query : query.SeoFragment,
                Offset = query.Offset,
                Limit = PageItemsLimit.ListingPageItemsLimit,
                SortOrder = sortOrder,
                IncludeCount = true,
                IncludeSearchResults = true,
                ControllerName = _tenantProvider.Current().SupportsSeoFriendlyListings ? $"seo-{_tenantProvider.Current().Name}" : "",
                ServiceProjectionName = _tenantProvider.Current().SupportsSeoFriendlyListings ? _tenantProvider.Current().Name : "",
                NavigationName = _tenantProvider.Current().RyvusNavName,
                PostProcessors = postProcessors,
                IncludeMetaData = true
            });

            var resultData = !result.IsSucceed ? null : result.Data;

            if (resultData == null) return null;

            // check in case there is an equivalent SEO URL that we can redirect to
            if (_tenantProvider.Current().SupportsSeoFriendlyListings && !string.IsNullOrEmpty(resultData.Metadata?.Seo) && resultData.Metadata.Seo != query.SeoFragment)
            {
                return new GetListingsResponse()
                {
                    RedirectRequired = true,
                    RedirectUrl = ListingsUrlFormatter.GetSeoUrl(resultData.Metadata.Seo, query.Offset, query.Sort)
                };
            }

            var searchContext = new SearchContext()
            {
                RyvussNavResult = resultData,
                Query = string.IsNullOrEmpty(query.Query) ? resultData.Metadata?.Query : query.Query,
                Offset = query.Offset,
                Sort = sortOrder,
                SeoFragment = query.SeoFragment,
                SearchEventType = query.SearchEventType
            };

            _searchResultContextStore.Set(searchContext);

            var navResults = _mapper.Map<NavResult>(resultData, opt => { opt.Items["sortOrder"] = query.Sort; });
            navResults.INav.CurrentAction = ListingsUrlFormatter.GetQueryString(!string.IsNullOrEmpty(query.SeoFragment) ? query.SeoFragment : query.Query, query.Sort);
            navResults.INav.CurrentUrl = !string.IsNullOrEmpty(query.SeoFragment) ? ListingsUrlFormatter.GetSeoUrl(query.SeoFragment, query.Offset, query.Sort):
                ListingsUrlFormatter.GetPathAndQueryString(query.Query, query.Offset, query.Sort);

            return new GetListingsResponse
            {
                RedirectRequired = false,
                ListingsViewModel = new ListingsViewModel
                {
                    NavResults = navResults,
                    Paging = _paginationHelper.GetPaginationData(navResults.Count, PageItemsLimit.ListingPageItemsLimit, query.Offset, sortOrder, !string.IsNullOrEmpty(query.SeoFragment) ? query.SeoFragment : query.Query, query.Keywords),
                    Sorting = _sortingHelper.GenerateSortByViewModel(sortOrder, !string.IsNullOrEmpty(query.SeoFragment) ? query.SeoFragment : query.Query, query.Keywords),
                    Keyword = !string.IsNullOrEmpty(query.Keywords) ? query.Keywords : _parser.Parse(resultData.Metadata?.Query).GetKeywords(),
                    DisqusSource = _tenantProvider.Current().DisqusSource,
                    PolarNativeAdsData = _polarNativeAdsDataMapper.Map(resultData.INav.BreadCrumbs),
                    ShowSponsoredLinks = _sponsoredLinksDataMapper.ShowSponsoredLinks(),
                    InsightsData = _listingInsightsDataMapper.Map(searchContext),
                    SeoData = _seoDataMapper.Map(resultData)
                }
            };
        }
    }
}