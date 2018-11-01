using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public interface IRedirectLogger
    {
        void Log(string requestUrl);
    }

    public class RedirectLogger : IRedirectLogger
    {
        private readonly ILogger _logger;

        public RedirectLogger(ILogger logger)
        {
            _logger = logger;
        }
        public void Log(string requestUrl)
        {
            _logger.Trace(requestUrl);
        }
    }
}