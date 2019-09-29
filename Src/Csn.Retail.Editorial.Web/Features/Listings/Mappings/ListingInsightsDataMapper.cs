using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Tracking.Scripts.Core;
using Csn.Tracking.Scripts.Ryvus42;
using Expresso.Syntax;
using Microsoft.Ajax.Utilities;
using Expression = Expresso.Expressions.Expression;

namespace Csn.Retail.Editorial.Web.Features.Listings.Mappings
{
    public interface IListingInsightsDataMapper
    {
        CsnInsightsData Map(ListingPageContext pageContext);
    }

    [AutoBind]
    public class ListingInsightsDataMapper : IListingInsightsDataMapper
    {
        private readonly IExpressionParser _expressionParser;
        private Dictionary<string, string> _postTypes;

        public ListingInsightsDataMapper(IExpressionParser expressionParser)
        {
            _expressionParser = expressionParser;
            _postTypes = new Dictionary<string, string>()
            {
                { "car news", "news" },
                { "car advice", "advice" },
                { "car features", "feature" },
                { "car reviews", "review" },
                { "car videos", "video" },
                { "car comparisons", "comparison" }
            };
        }

        public CsnInsightsData Map(ListingPageContext listingPageContext)
        {            
            var dimensions = GetDimensions(_expressionParser.Parse(listingPageContext.Query));

            dimensions.Add(TrackingScriptTags.ContentGroup1, TrackingScriptContentGroups.NewsAndReviews);

            if (listingPageContext.EditorialPageType != EditorialPageTypes.Homepage)
            {
                dimensions.Add(TrackingScriptTags.ContentGroup2, TrackingScriptPageTypes.Listing);
                dimensions.Add(TrackingScriptTags.Action, listingPageContext.SearchEventType.ToTrackingScriptPageType());
                dimensions.Add(TrackingScriptTags.SortBy, string.IsNullOrEmpty(listingPageContext.Sort) ? EditorialSortKeyValues.ListingPageDefaultSort : listingPageContext.Sort);
                dimensions.Add(TrackingScriptTags.ListingResultCount, listingPageContext.RyvussNavResult.Count.ToString());
            }

            return new CsnInsightsData()
            {
                MetaData = dimensions,
                SearchResultsData = GetSearchResultsData(listingPageContext)
            };
        }

        private Dictionary<string, string> GetDimensions(Expression expression)
        {
            var tags = GetTags(expression);
            if (tags.Any(a => a.Key.ToLower().Equals("service") && a.Value.ToLower().Equals("carsales")) && tags.Any(a => a.Key.ToLower().Equals("posttype")))
            {
                if (_postTypes.TryGetValue(tags["posttype"], out string value))
                {
                    tags.Add("type", value);
                }
                tags.Remove("posttype");
            }

            return tags;
        }

        private Dictionary<string, string> GetTags(Expression expression)
        {
            return expression.Accept(new TrackingScriptTagVisitor(), new List<TrackingScriptTag>()).ToDictionary(tag => tag.Name, tag => tag.Value);
        }

        private CsnInsightsSearchResultsData GetSearchResultsData(ListingPageContext listingPageContext)
        {
            if (listingPageContext.RyvussNavResult?.SearchResults == null) return new CsnInsightsSearchResultsData();

            return new CsnInsightsSearchResultsData()
            {
                Results = listingPageContext.RyvussNavResult.SearchResults.Select(r => new CsnInsightsSearchResultItem{Id = r.Id, Name = r.Id}).ToList()
            };
        }
    }
}