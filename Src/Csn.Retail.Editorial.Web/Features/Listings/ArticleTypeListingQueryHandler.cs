using Csn.Retail.Editorial.Web.Features.Listings.ModelBinders;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;
using Expresso.Expressions;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    [AutoBind]
    public class ArticleTypeListingQueryHandler : IQueryHandler<ArticleTypeListingQuery, GetListingsQuery>
    {
        private readonly IExpressionFormatter _expressionFormatter;
        private readonly IArticleTypeLookup _articleTypeLookup;

        public ArticleTypeListingQueryHandler(IExpressionFormatter expressionFormatter, IArticleTypeLookup articleTypeLookup)
        {
            _expressionFormatter = expressionFormatter;
            _articleTypeLookup = articleTypeLookup;
        }

        public GetListingsQuery Handle(ArticleTypeListingQuery query)
        {
            var facetName = _articleTypeLookup.GetFacetNameFromArticleType(query.ArticleType);

            var aspectName = query.ArticleType == ArticleType.Sponsored ? "ArticleTypes" : "Type";

            var expression = string.IsNullOrEmpty(facetName) ? Expression.Create() : new FacetExpression(aspectName, facetName);

            return new GetListingsQuery
            {
                Query = _expressionFormatter.Format(expression),
                QueryExpression = expression,
                EditorialPageType = EditorialPageTypes.Landing
            };
        }
    }
}