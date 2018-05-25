using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Tracking.Scripts.Core;
using Csn.Tracking.Scripts.Ryvus42;
using Expresso.Expressions;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Listings.Mappings
{
    public interface IListingInsightsDataMapper
    {
        Dictionary<string, string> Map(SearchContext searchContext);
    }

    [AutoBind]
    public class ListingInsightsDataMapper : IListingInsightsDataMapper
    {
        private readonly IExpressionParser _expressionParser;

        public ListingInsightsDataMapper(IExpressionParser expressionParser)
        {
            _expressionParser = expressionParser;
        }

        public Dictionary<string, string> Map(SearchContext searchContext)
        {            
            var expression = _expressionParser.Parse(searchContext.Query);
            var dimensions = GetTags(expression);

            dimensions.Add(TrackingScriptTags.ContentGroup1, TrackingScriptContentGroups.NewsAndReviews);

            if (searchContext.EditorialPageType != EditorialPageTypes.Homepage)
            {
                dimensions.Add(TrackingScriptTags.ContentGroup2, TrackingScriptPageTypes.Listing);
                dimensions.Add(TrackingScriptTags.Action, searchContext.SearchEventType.ToTrackingScriptPageType());
                dimensions.Add(TrackingScriptTags.SortBy, string.IsNullOrEmpty(searchContext.Sort) ? EditorialSortKeyValues.ListingPageDefaultSort : searchContext.Sort);
                dimensions.Add(TrackingScriptTags.ListingResultCount, searchContext.RyvussNavResult.Count.ToString());
            }

            return dimensions;
        }

        private Dictionary<string, string> GetTags(Expression expression)
        {
            return expression.Accept(new TrackingScriptTagVisitor(), new List<TrackingScriptTag>()).ToDictionary(tag => tag.Name, tag => tag.Value);
        }
    }
}