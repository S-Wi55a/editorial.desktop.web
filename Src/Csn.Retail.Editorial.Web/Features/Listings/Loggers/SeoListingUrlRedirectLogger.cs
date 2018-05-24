using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Features.Listings.Loggers
{
    public interface ISeoListingUrlRedirectLogger
    {
        void Log(string requestUrl);
    }

    public class SeoListingUrlRedirectLogger : ISeoListingUrlRedirectLogger
    {
        private readonly ILogger _logger;

        public SeoListingUrlRedirectLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string requestUrl)
        {
            _logger.Trace(requestUrl);
        }
    }
}