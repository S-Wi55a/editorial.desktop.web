using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Redirects;

namespace Csn.Retail.Editorial.Web.Features.Tracking.Providers
{
    public interface ITrackingTagsProvider
    {
        List<KeyValuePair<string, string>> GetTags();
    }

    [AutoBind]
    public class TrackingTagsProvider : ITrackingTagsProvider
    {
        private readonly IRequestContextWrapper _requestContextWrapper;

        public TrackingTagsProvider(IRequestContextWrapper requestContextWrapper)
        {
            _requestContextWrapper = requestContextWrapper;
        }

        public List<KeyValuePair<string, string>> GetTags()
        {
            if (!_requestContextWrapper.QueryString.HasKeys())
            {
                return null;
            }

            return _requestContextWrapper.QueryString.AllKeys.Where(k => k.StartsWith("utm_")).SelectMany(key => _requestContextWrapper.QueryString.GetValues(key), (key, value) => new KeyValuePair<string, string>(key, value)).ToList();
        }
    }
}