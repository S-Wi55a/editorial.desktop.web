using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Features.Details.Loggers
{
    public interface IDetailsRequestLogger
    {
        void Log(string originalUrl);
    }

    public class DetailsRequestLogger : IDetailsRequestLogger
    {
        private readonly ILogger _logger;

        public DetailsRequestLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string originalUrl)
        {
            _logger.Trace(originalUrl);
        }
    }
}