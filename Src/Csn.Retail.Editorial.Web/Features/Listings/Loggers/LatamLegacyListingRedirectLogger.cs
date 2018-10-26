using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Features.Listings.Loggers
{
    public interface ILatamLegacyListingRedirectLogger
    {
        void Log(string requestUrl);
    }

    public class LatamLegacyListingRedirectLogger : ILatamLegacyListingRedirectLogger
    {
        private readonly ILogger _logger;

        public LatamLegacyListingRedirectLogger(ILogger logger)
        {
            _logger = logger;
        }
        public void Log(string requestUrl)
        {
            _logger.Trace(requestUrl);
        }
    }
}