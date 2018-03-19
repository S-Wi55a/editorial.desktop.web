using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Features.Details.Loggers
{
    public interface ILegacyDetailsRedirectLogger
    {
        void Log(string originalUrl);
    }

    public class LegacyDetailsRedirectLogger : ILegacyDetailsRedirectLogger
    {
        private readonly ILogger _logger;

        public LegacyDetailsRedirectLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string originalUrl)
        {
            _logger.Trace(originalUrl);
        }
    }
}