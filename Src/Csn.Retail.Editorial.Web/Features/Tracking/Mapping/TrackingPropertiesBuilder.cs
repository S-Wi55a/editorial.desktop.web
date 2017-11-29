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
        private readonly ISearchResultContextStore _searchResultContextStore;
        private readonly IExpressionParser _expressionParser;

        public TrackingPropertiesBuilder(ISearchResultContextStore searchResultContextStore, IExpressionParser expressionParser)
        {
            _searchResultContextStore = searchResultContextStore;
            _expressionParser = expressionParser;
        }

        public IList<KeyValuePair<string, string>> Get()
        {
            var properties = new List<KeyValuePair<string, string>>();

            var search = _searchResultContextStore.Get();

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