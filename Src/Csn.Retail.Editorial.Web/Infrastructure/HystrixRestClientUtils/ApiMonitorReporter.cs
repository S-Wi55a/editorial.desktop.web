using Csn.Hystrix.RestClient.Dtos;
using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Infrastructure.HystrixRestClientUtils
{
    public class ApiMonitorReporter : Csn.Hystrix.RestClient.IHystrixRestReporter
    {
        private readonly ILogger _logger;

        public ApiMonitorReporter(ILogger logger)
        {
            _logger = logger;
        }

        public void Report(HystrixRestReport reportData)
        {
            if (!_logger.IsInfoEnabled)
                return;

            var tags = new LogTags
            {
                ["api-execution"] = reportData.Execution.TotalMilliseconds,
                ["api-duration"] = reportData.Duration.TotalMilliseconds,
                ["api-status"] = reportData.CircuitStatus,
                ["api-cachehit"] = reportData.CacheHit,
                ["api-name"] = reportData.RequestBaseUrl
            };

            _logger.Log(LogType.Info, new Log(tags, "Hystrix on '{0}'", reportData.RequestBaseUrl));
        }
    }
}