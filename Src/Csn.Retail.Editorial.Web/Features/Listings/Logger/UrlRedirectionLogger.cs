using Csn.Logging;
using Csn.Retail.Editorial.Web.Features.Errors;

namespace Csn.Retail.Editorial.Web.Features.Listings.Logger
{
    public interface IUrlRedirectionLogger
    {
        void Log(string requestUrl);
    }

    public class UrlRedirectionLogger : IUrlRedirectionLogger
    {
        private readonly ILogger _logger;

        public UrlRedirectionLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string requestUrl)
        {
            _logger.Trace(requestUrl);
        }
    }
}