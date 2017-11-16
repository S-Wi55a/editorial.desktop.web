using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            var dimentions = GetTags(expression);

            dimentions.Add(TrackingScriptTags.ContentGroup1, TrackingScriptContentGroups.NewsAndReviews);
            dimentions.Add(TrackingScriptTags.ContentGroup2, TrackingScriptPageTypes.Listing);
            dimentions.Add(TrackingScriptTags.Action, GetPageType());
            dimentions.Add(TrackingScriptTags.SortBy, sort);

            return dimentions;
        }

        private Dictionary<string, string> GetTags(Expression expression)
        {
            var visitor = new TrackingScriptTagVisitor();
            return expression.Accept(visitor, new List<TrackingScriptTag>()).ToDictionary(tag => tag.Name, tag => tag.Value);
        }
        private string GetPageType()
        {
            var uri = HttpContext.Current.Request.Url;
            var uriReferrer = HttpContext.Current.Request.UrlReferrer;

            // If it is a listing url...
            if (uriReferrer != null && uriReferrer.AbsolutePath.Contains("/results") && uriReferrer.AbsolutePath.Contains("/beta-results"))
            {
                // Check if it's sorting
                var curSort = uri.GetQueryParameter("sort");
                var prvSort = uriReferrer.GetQueryParameter("sort");
                if ((curSort != null && curSort != prvSort) &&
                    (curSort != "Latest" || prvSort != null))
                {
                    return TrackingScriptPageTypes.Sort;
                }

                // Check if it's paging                
                var curOffset = uri.GetQueryParameter("offset"); 
                var prvOffset = uriReferrer.GetQueryParameter("offset");
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