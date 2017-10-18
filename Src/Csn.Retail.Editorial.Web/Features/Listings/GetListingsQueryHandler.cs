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
        private readonly IExpressionSyntax _expressionSyntax;
        private readonly IContextStore _contextStore;

        public GetListingsQueryHandler(IEditorialRyvussApiProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider, IMapper mapper, IPaginationHelper paginationHelper,
            ISortingHelper sortingHelper, IExpressionParser parser, IExpressionSyntax expressionSyntax, IContextStore contextStore)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
            _paginationHelper = paginationHelper;
            _sortingHelper = sortingHelper;
            _parser = parser;
            _expressionSyntax = expressionSyntax;
            _contextStore = contextStore;
        }
        public async Task<GetListingsResponse> HandleAsync(GetListingsQuery query)
        {
            query.Q = string.IsNullOrEmpty(query.Q) ? $"Service.{_tenantProvider.Current().Name}." : query.Q;

            query.Q = AppendOrUpdateKeywordExpression(query.Q, query.Keyword);

            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput()
            {
                Query = query.Q,
                Limit = query.Limit,
                Offset = query.Skip,
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
                navResults.Paging = _paginationHelper.GetPaginationData(resultData.Count, query.Limit, query.Skip, query.SortOrder, query.Q);
                navResults.Sorting = _sortingHelper.GenerateSortByViewModel(EditorialSortKeyValues.Items, query.SortOrder);
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
            if (keyword.IsNullOrEmpty()) return query;

            var currentExpression = _parser.Parse(query);
            if (currentExpression != EmptyExpression.Instance && currentExpression is BranchExpression)
            {
                var currentKeywordExpression = ((BranchExpression) currentExpression).Expressions.FirstOrDefault(a => a is KeywordExpression);
                if (currentKeywordExpression != null)
                {
                    ((BranchExpression)currentExpression).Expressions.Remove(currentKeywordExpression);
                    currentExpression.And(new KeywordExpression("Keywords", $"({keyword})"));
                    return _expressionSyntax.Format(currentExpression);
                }
            }

            var keywordexpression = new KeywordExpression("Keywords", $"({keyword})");
            return _expressionSyntax.Format(currentExpression & keywordexpression);
        }
    }
}