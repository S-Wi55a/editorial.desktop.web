using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Listings.ModelBinders;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;
using Expresso.Expressions;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    [AutoBind]
    public class ArticleTypeListingQueryHandler : IAsyncQueryHandler<ArticleTypeListingQuery, GetListingsQuery>
    {
        private readonly IExpressionFormatter _expressionFormatter;
        private readonly IArticleTypeLookup _articleTypeLookup;

        public ArticleTypeListingQueryHandler(IExpressionFormatter expressionFormatter, IArticleTypeLookup articleTypeLookup)
        {
            _expressionFormatter = expressionFormatter;
            _articleTypeLookup = articleTypeLookup;
        }

        public async Task<GetListingsQuery> HandleAsync(ArticleTypeListingQuery query)
        {
            var facetName = _articleTypeLookup.GetFacetNameFromArticleType(query.ArticleType);

            var expression = string.IsNullOrEmpty(facetName) ? Expression.Create() : new FacetExpression("Type", facetName);

            return new GetListingsQuery
            {
                Query = _expressionFormatter.Format(expression),
                QueryExpression = expression,
                EditorialPageType = EditorialPageTypes.Landing
            };
        }
    }
}