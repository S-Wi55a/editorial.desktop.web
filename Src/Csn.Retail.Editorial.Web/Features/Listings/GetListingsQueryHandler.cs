using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Constants;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;
using Expresso.Expressions;
using Expresso.Syntax;
using IContextStore = Ingress.ContextStores.IContextStore;

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
        private readonly IExpressionParser _parser;
        private readonly IExpressionFormatter _expressionFormatter;
        private readonly IContextStore _contextStore;

        public GetListingsQueryHandler(IEditorialRyvussApiProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider, IMapper mapper, IPaginationHelper paginationHelper,
            ISortingHelper sortingHelper, IExpressionParser parser, IExpressionFormatter expressionFormatter, IContextStore contextStore)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
            _paginationHelper = paginationHelper;
            _sortingHelper = sortingHelper;
            _parser = parser;
            _expressionFormatter = expressionFormatter;
            _contextStore = contextStore;
        }
        public async Task<GetListingsResponse> HandleAsync(GetListingsQuery query)
        {
            query.Q = string.IsNullOrEmpty(query.Q) ? $"Service.{_tenantProvider.Current().Name}." : query.Q;

            query.Q = AppendOrUpdateKeywordExpression(query.Q, query.Keyword);

            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput()
            {
                Query = query.Q,
                Offset = query.Offset,
                Limit = PageItemsLimit.ListingPageItemsLimit,
                SortOrder = query.SortOrder,
                IncludeCount = true,
                IncludeSearchResults = true,
                NavigationName = "RetailNav",
                PostProcessors = new List<string> { "Retail", "FacetSort", "ShowZero" }
            });

            var resultData = !result.IsSucceed ? null : result.Data;

            _contextStore.Set(ContextStoreKeys.CurrentSearchResult.ToString(), resultData);

            var navResults = _mapper.Map<NavResult>(resultData);

            if (navResults != null)
            {
                navResults.PendingQuery = string.IsNullOrEmpty(query.Q) ? string.Empty: query.Q;
                navResults.Paging = _paginationHelper.GetPaginationData(navResults.Count, PageItemsLimit.ListingPageItemsLimit, query.Offset, query.SortOrder, query.Q);
                navResults.Sorting = _sortingHelper.GenerateSortByViewModel(EditorialSortKeyValues.Items, query.SortOrder, query.Q);
                navResults.Keyword = query.Keyword;
            }
            
            return resultData == null ? null : new GetListingsResponse
            {
                ListingsViewModel = new ListingsViewModel
                {
                    NavResults = navResults
                }
            };
        }

        private string AppendOrUpdateKeywordExpression(string query, string keyword)
        {
            var currentExpression = _parser.Parse(query);
            var currentKeywordExpression = Expression.Create();

            if (currentExpression != EmptyExpression.Instance && currentExpression is BranchExpression)
            {
                currentKeywordExpression = ((BranchExpression)currentExpression).Expressions.FirstOrDefault(a => a is KeywordExpression);
            }

            if (!keyword.IsNullOrEmpty())
            {
                var keywordexpression = new KeywordExpression("Keywords", $"({keyword})");
                if (currentKeywordExpression != null && currentKeywordExpression != EmptyExpression.Instance)
                {
                    ((BranchExpression) currentExpression).Expressions.Remove(currentKeywordExpression);                    
                }

                return _expressionFormatter.Format(currentExpression & keywordexpression);
            }

            if (currentKeywordExpression != null && currentKeywordExpression != EmptyExpression.Instance)
            {
                ((BranchExpression)currentExpression).Expressions.Remove(currentKeywordExpression);
                return _expressionFormatter.Format(currentExpression);
            }

            return query;
        }
    }
}