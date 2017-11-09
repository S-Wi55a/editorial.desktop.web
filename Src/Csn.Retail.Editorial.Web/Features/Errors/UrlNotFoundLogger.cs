using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Features.Errors
{
    public interface IUrlNotFoundLogger
    {
        void Log(string requestUrl);
    }

    public class UrlNotFoundLogger : IUrlNotFoundLogger
    {
        private readonly ILogger _logger;

        public UrlNotFoundLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string requestUrl)
        {
            _logger.Trace(requestUrl);
        }
    }
   
}