using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Features.Details.Loggers
{
    public interface IArticleNotFoundLogger
    {
        void Log(string requestUrl);
    }

    public class ArticleNotFoundLogger : IArticleNotFoundLogger
    {
        private readonly ILogger _logger;

        public ArticleNotFoundLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(string requestUrl)
        {
            _logger.Trace(requestUrl);
        }
    }
}