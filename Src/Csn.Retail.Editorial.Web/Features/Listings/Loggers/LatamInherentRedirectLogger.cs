using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Features.Listings.Loggers
{
    public interface ILatamInherentListingRedirectLogger
    {
        void Log(string requestUrl);
    }

    public class LatamInherentListingRedirectLogger : ILatamInherentListingRedirectLogger
    {
        private readonly ILogger _logger;

        public LatamInherentListingRedirectLogger(ILogger logger)
        {
            _logger = logger;
        }
        public void Log(string requestUrl)
        {
            _logger.Trace(requestUrl);
        }
    }
}