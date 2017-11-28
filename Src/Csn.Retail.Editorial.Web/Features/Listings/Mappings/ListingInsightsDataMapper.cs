using System.Collections.Generic;
using System.Linq;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.Tracking.Scripts.Core;
using Csn.Tracking.Scripts.Ryvus42;
using Expresso.Expressions;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Listings.Mappings
{
    public interface IListingInsightsDataMapper
    {
        Dictionary<string, string> Map(string query, string sort);
    }

    [AutoBind]
    public class ListingInsightsDataMapper : IListingInsightsDataMapper
    {
        private readonly IExpressionParser _expressionParser;

        public ListingInsightsDataMapper(IExpressionParser expressionParser)
        {
            _expressionParser = expressionParser;
        }

        public Dictionary<string, string> Map(string query, string sort)
        {            
            var expression = _expressionParser.Parse(query);
            var dimensions = GetTags(expression);

            dimensions.Add(TrackingScriptTags.ContentGroup1, TrackingScriptContentGroups.NewsAndReviews);
            dimensions.Add(TrackingScriptTags.ContentGroup2, TrackingScriptPageTypes.Listing);
            dimensions.Add(TrackingScriptTags.Action, GetActionType());
            dimensions.Add(TrackingScriptTags.SortBy, string.IsNullOrEmpty(sort) ? string.Empty : sort);

            return dimensions;
        }

        private Dictionary<string, string> GetTags(Expression expression)
        {
            return expression.Accept(new TrackingScriptTagVisitor(), new List<TrackingScriptTag>()).ToDictionary(tag => tag.Name, tag => tag.Value);
        }
        private string GetActionType()
        {
            var uri = HttpContext.Current.Request.Url;
            var uriReferrer = HttpContext.Current.Request.UrlReferrer;

            // If it is a listing url...
            if (uriReferrer != null && (uriReferrer.AbsolutePath.Contains("/results") || uriReferrer.AbsolutePath.Contains("/beta-results")))
            {
                // Check if it's sorting
                var curSort = uri.GetQueryParameter(ListingsQueryStringParams.Sort);
                var prvSort = uriReferrer.GetQueryParameter(ListingsQueryStringParams.Sort);
                if (curSort != null && curSort != prvSort && (curSort != EditorialSortKeyValues.ListingPageDefaultSort || prvSort != null))
                {
                    return TrackingScriptPageTypes.Sort;
                }

                // Check if it's paging                
                var curOffset = uri.GetQueryParameter(ListingsQueryStringParams.Offset); 
                var prvOffset = uriReferrer.GetQueryParameter(ListingsQueryStringParams.Offset);
                if ((curOffset != null && curOffset != prvOffset) && (curOffset != "0" || prvOffset != null))
                {
                    return TrackingScriptPageTypes.Pagination;
                }

                // otherwise refinement                
                return TrackingScriptPageTypes.Refinement;
            }

            return TrackingScriptPageTypes.Search;
        }
    }
}