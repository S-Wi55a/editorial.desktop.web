using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Features.Listings.Loggers
{
    public interface ILegacyListingUrlRedirectLogger
    {
        void Log(string requestUrl);
    }

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