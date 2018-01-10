using System.Linq;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;
using Expresso.Expressions;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    [AutoBind]
    public class RedbookListingQueryHandler : IAsyncQueryHandler<RedbookListingQuery, GetListingsQuery>
    {
        private readonly IExpressionFormatter _expressionFormatter;
        private readonly IExpressionParser _expressionParser;

        public RedbookListingQueryHandler(IExpressionFormatter expressionFormatter, IExpressionParser expressionParser)
        {
            _expressionFormatter = expressionFormatter;
            _expressionParser = expressionParser;
        }

        public async Task<GetListingsQuery> HandleAsync(RedbookListingQuery verticalQuery)
        {

            if (string.IsNullOrEmpty(verticalQuery.Query))
            {
                var expression = new FacetExpression("Service", "Redbook") & new FacetExpression("Vertical", RedbookVerticals.Items[verticalQuery.Vertical]);
                return new GetListingsQuery
                {
                    Query = _expressionFormatter.Format(expression),
                    QueryExpression = expression,
                    EditorialPageType = EditorialPageTypes.Landing
                };
            }
            var exp = _expressionParser.Parse(verticalQuery.Query);
            var service = Expression.Create();
            var vertical = Expression.Create();

            if (exp is BranchExpression verticalExp && !verticalExp.Expressions.Any(b => b is FacetExpression fe && fe.Left == "Vertical"))
            {
                vertical = new FacetExpression("Vertical", RedbookVerticals.Items[verticalQuery.Vertical]);
            }
            if (exp is BranchExpression serviceExp && !serviceExp.Expressions.Any(b => b is FacetExpression fe && fe.Left == "Service"))
            {
                service = new FacetExpression("Service", "Redbook");
            }
            exp = exp & service & vertical;
            return new GetListingsQuery
            {
                Query = _expressionFormatter.Format(exp),
                QueryExpression = exp,
                EditorialPageType = EditorialPageTypes.Landing
            };
        }
    }
}