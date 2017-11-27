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
            if (!_tenantProvider.Current().SupportsSeoFriendlyListings)
            {
                query.Q = string.IsNullOrEmpty(query.Q) ? $"Service.{_tenantProvider.Current().Name}." : query.Q;
            }
            
            if (!string.IsNullOrEmpty(query.Keywords))
            {
                query.Q = _expressionFormatter.Format(_parser.Parse(query.Q).AppendOrUpdateKeywords(query.Keywords));
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
                Query = string.IsNullOrEmpty(query.SeoFragment) ? query.Q : query.SeoFragment,
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

            _contextStore.Set(ContextStoreKeys.CurrentSearchResult.ToString(), resultData);

            var navResults = _mapper.Map<NavResult>(resultData, opt => { opt.Items["sortOrder"] = query.Sort; });
            navResults.INav.CurrentAction = string.IsNullOrEmpty(query.SeoFragment) ? query.Q : query.SeoFragment;

            return new GetListingsResponse
            {
                RedirectRequired = false,
                ListingsViewModel = new ListingsViewModel
                {
                    NavResults = navResults,
                    Paging = _paginationHelper.GetPaginationData(navResults.Count, PageItemsLimit.ListingPageItemsLimit, query.Offset, sortOrder, query.Q, query.Keywords),
                    Sorting = _sortingHelper.GenerateSortByViewModel(sortOrder, query.Q, query.Keywords),
                    Keyword = !string.IsNullOrEmpty(query.Keywords) ? query.Keywords : _parser.Parse(query.Q).GetKeywords(),
                    DisqusSource = _tenantProvider.Current().DisqusSource,
                    PolarNativeAdsData = _polarNativeAdsDataMapper.Map(resultData.INav.BreadCrumbs),
                    ShowSponsoredLinks = _sponsoredLinksDataMapper.ShowSponsoredLinks(),
                    InsightsData = _listingInsightsDataMapper.Map(query.Q, query.Sort)
                }
            };
        }
    }
}