using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;
using Expresso.Expressions;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    public class RedbookHomepageQuery : IQuery
    {
        public Vertical Vertical { get; set; }
    }

    [AutoBind]
    public class RedbookHomepageQueryHandler : IAsyncQueryHandler<RedbookHomepageQuery, GetListingsQuery>
    {
        private readonly IExpressionFormatter _expressionFormatter;

        public RedbookHomepageQueryHandler(IExpressionFormatter expressionFormatter)
        {
            _expressionFormatter = expressionFormatter;
        }

        public Task<GetListingsQuery> HandleAsync(RedbookHomepageQuery query)
        {
            var expression = new FacetExpression("Service", "Redbook") &
                                new FacetExpression("Vertical", RedbookVerticals.Items[query.Vertical]);

            return Task.FromResult(new GetListingsQuery
            {
                Query = _expressionFormatter.Format(expression),
                QueryExpression = expression,
                EditorialPageType = EditorialPageTypes.Homepage
            });
        }
    }
}