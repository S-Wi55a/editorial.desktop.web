using Csn.Logging;
using Csn.Retail.Editorial.Web.Features.Errors;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Listings.Logger
{
    public interface ILegacyListingUrlRedirectLogger
    {
        void Log(string requestUrl);
    }

    [AutoBind]
    public class LegacyListingUrlRedirectLogger : ILegacyListingUrlRedirectLogger
    {
        private readonly ILogger _logger;

        public LegacyListingUrlRedirectLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string requestUrl)
        {
            _logger.Trace(requestUrl);
        }
    }
}