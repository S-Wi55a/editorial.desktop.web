﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Constants;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.SimpleCqrs;
using IContextStore = Ingress.ContextStores.IContextStore;
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
        private readonly IContextStore _contextStore;
        private readonly IKeywordExpressionHelper _keywordExpressionHelper;

        public GetListingsQueryHandler(IEditorialRyvussApiProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider, IMapper mapper, IPaginationHelper paginationHelper,
            ISortingHelper sortingHelper, IContextStore contextStore, IKeywordExpressionHelper keywordExpressionHelper)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
            _paginationHelper = paginationHelper;
            _sortingHelper = sortingHelper;
            
            _contextStore = contextStore;
            _keywordExpressionHelper = keywordExpressionHelper;
        }

        public async Task<GetListingsResponse> HandleAsync(GetListingsQuery query)
        {
            query.Q = string.IsNullOrEmpty(query.Q) ? $"Service.{_tenantProvider.Current().Name}." : query.Q;

            query.Q = _keywordExpressionHelper.AppendOrUpdate(query.Q, query.Keyword);

            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput()
            {
                Query = query.Q,
                Offset = query.Offset,
                Limit = PageItemsLimit.ListingPageItemsLimit,
                SortOrder = query.SortOrder,
                IncludeCount = true,
                IncludeSearchResults = true,
                NavigationName = "RetailNav",
                PostProcessors = new List<string> {"Retail", "FacetSort", "ShowZero"}
            });

            var resultData = !result.IsSucceed ? null : result.Data;

            _contextStore.Set(ContextStoreKeys.CurrentSearchResult.ToString(), resultData);

            var navResults = _mapper.Map<NavResult>(resultData, opt => { opt.Items["sortOrder"] = query.SortOrder; });
            
            return resultData == null ? null : new GetListingsResponse
                {
                    ListingsViewModel = new ListingsViewModel
                    {
                        NavResults = navResults,
                        Paging = _paginationHelper.GetPaginationData(navResults.Count, PageItemsLimit.ListingPageItemsLimit, query.Offset, query.SortOrder, query.Q, query.Keyword),
                        Sorting = _sortingHelper.GenerateSortByViewModel(EditorialSortKeyValues.Items, query.SortOrder, query.Q, query.Keyword),
                        CurrentQuery = GetFormattedQuery(query),
                        Keyword = query.Keyword
                    }
                };
        }

        private static string GetFormattedQuery(GetListingsQuery query)
        {
            return string.IsNullOrEmpty(query.Q) ? string.Empty : $"?q={query.Q}{UrlParamsFormatter.GetSortParam(query.SortOrder)}";
        }
    }
}