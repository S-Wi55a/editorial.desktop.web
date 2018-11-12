using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public interface IRedirectLogger
    {
        void Log(RedirectInstruction redirectInstruction, string requestUrl);
    }

    public class RedirectLogger : IRedirectLogger
    {
        private readonly ILogger _logger;

        public RedirectLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Log(RedirectInstruction redirectInstruction, string requestUrl)
        {
            _logger.Trace("RedirectName:{0} {1} -> {2}", redirectInstruction.Name, requestUrl, redirectInstruction.RedirectResult.Url);
        }
    }
}