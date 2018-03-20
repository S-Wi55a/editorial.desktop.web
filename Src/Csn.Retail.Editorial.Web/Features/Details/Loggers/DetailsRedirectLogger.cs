using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Features.Details.Loggers
{
    public interface IDetailsRedirectLogger
    {
        void Log(string originalUrl);
    }

    public class DetailsRedirectLogger : IDetailsRedirectLogger
    {
        private readonly ILogger _logger;

        public DetailsRedirectLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string originalUrl)
        {
            _logger.Trace(originalUrl);
        }
    }
}