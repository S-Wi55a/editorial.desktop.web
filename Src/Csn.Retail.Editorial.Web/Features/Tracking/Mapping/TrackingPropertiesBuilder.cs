using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Tracking.Mapping
{
    public interface ITrackingPropertiesBuilder
    {
        IList<KeyValuePair<string, string>> Get();
    }

    [AutoBind]
    public class TrackingPropertiesBuilder : ITrackingPropertiesBuilder
    {
        private readonly IPageContextStore _pageContextStore;
        private readonly IExpressionParser _expressionParser;

        public TrackingPropertiesBuilder(IPageContextStore pageContextStore, IExpressionParser expressionParser)
        {
            _pageContextStore = pageContextStore;
            _expressionParser = expressionParser;
        }

        public IList<KeyValuePair<string, string>> Get()
        {
            var properties = new List<KeyValuePair<string, string>>();

            var pageContext = _pageContextStore.Get();

            var search = pageContext?.PageContextType == PageContextTypes.Listing ? pageContext as ListingPageContext : null;

            if (search?.RyvussNavResult?.Metadata?.Query == null) return null;

            var expression = _expressionParser.Parse(search.RyvussNavResult.Metadata.Query);

            var keyword = expression.GetKeywords();

            if (!string.IsNullOrEmpty(keyword))
            {
                properties.Add(new KeyValuePair<string, string>("Keyword", keyword));
            }

            return properties;
        }
    }
}