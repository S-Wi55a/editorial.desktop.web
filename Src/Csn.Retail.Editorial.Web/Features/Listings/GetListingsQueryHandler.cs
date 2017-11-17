using System.Collections.Generic;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;
using Csn.Retail.Editorial.Web.Features.Listings.Mappings;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.Mappers;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.SimpleCqrs;
using Expresso.Expressions;
using Expresso.Syntax;
using ContextStore= Ingress.Web.Common.Abstracts;
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
        private readonly ContextStore.IContextStore _contextStore;
        private readonly IExpressionParser _parser;
        private readonly IExpressionFormatter _expressionFormatter;
        private readonly IPolarNativeAdsDataMapper _polarNativeAdsDataMapper;
        private readonly ISponsoredLinksDataMapper _sponsoredLinksDataMapper;
        private readonly IListingInsightsDataMapper _listingInsightsDataMapper;

        public GetListingsQueryHandler(IEditorialRyvussApiProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider, IMapper mapper, IPaginationHelper paginationHelper,
            ISortingHelper sortingHelper, ContextStore.IContextStore contextStore, IExpressionParser parser, IExpressionFormatter expressionFormatter, IPolarNativeAdsDataMapper polarNativeAdsDataMapper, 
            ISponsoredLinksDataMapper sponsoredLinksDataMapper, IListingInsightsDataMapper listingInsightsDataMapper)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
            _paginationHelper = paginationHelper;
            _sortingHelper = sortingHelper;
            _contextStore = contextStore;
            _parser = parser;
            _expressionFormatter = expressionFormatter;
            _polarNativeAdsDataMapper = polarNativeAdsDataMapper;
            _sponsoredLinksDataMapper = sponsoredLinksDataMapper;
            _listingInsightsDataMapper = listingInsightsDataMapper;
        }

        public async Task<GetListingsResponse> HandleAsync(GetListingsQuery query)
        {
            query.Q = string.IsNullOrEmpty(query.Q) ? $"Service.{_tenantProvider.Current().Name}." : query.Q;
            query.Q = _expressionFormatter.Format(_parser.Parse(query.Q).AppendOrUpdateKeyword(query.Keywords));

            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput
            {
                Query = query.Q,
                Offset = query.Offset,
                Limit = PageItemsLimit.ListingPageItemsLimit,
                SortOrder = query.Sort,
                IncludeCount = true,
                IncludeSearchResults = true,
                NavigationName = _tenantProvider.Current().RyvusNavName,
                PostProcessors = new List<string> { "Retail", "FacetSort", "ShowZero" }
            });

            var resultData = !result.IsSucceed ? null : result.Data;
            _contextStore.Set(ContextStoreKeys.CurrentSearchResult.ToString(), resultData);

            var navResults = _mapper.Map<NavResult>(resultData, opt => { opt.Items["sortOrder"] = query.Sort; });

            return resultData == null ? null : new GetListingsResponse
            {
                ListingsViewModel = new ListingsViewModel
                {
                    NavResults = navResults,
                    Paging = _paginationHelper.GetPaginationData(navResults.Count, PageItemsLimit.ListingPageItemsLimit, query.Offset, query.Sort, query.Q, query.Keywords),
                    Sorting = _sortingHelper.GenerateSortByViewModel(EditorialSortKeyValues.Items, query.Sort, query.Q, query.Keywords),
                    CurrentQuery = ListingsUrlFormatter.GetQueryString(query.Q, sortOrder: query.Sort, keyword: query.Keywords),
                    Keyword = query.Keywords,
                    DisqusSource = _tenantProvider.Current().DisqusSource,
                    PolarNativeAdsData = _polarNativeAdsDataMapper.Map(resultData.INav.BreadCrumbs),
                    ShowSponsoredLinks = _sponsoredLinksDataMapper.ShowSponsoredLinks(),
                    InsightsData = _listingInsightsDataMapper.Map(query.Q, query.Sort)
                }
            };
        }
    }
}