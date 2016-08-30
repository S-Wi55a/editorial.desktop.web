using Csn.Hystrix.Abstractions;
using Csn.Hystrix.RestClient;
using Csn.Hystrix.RestClient.Dtos;
using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Infrastructure.HystrixRestClientUtils
{
    public class HystrixRestReporter : IHystrixRestReporter
    {
        private readonly ILogger _logger;

        public HystrixRestReporter(ILogger logger)
        {
            _logger = logger;
        }

        public void Report(HystrixRestReport reportData)
        {
            var logType = LogType.Trace;

            if (reportData.CircuitStatus == StatusCode.Failed.ToString()) logType = LogType.Error;
            if (reportData.CircuitStatus == StatusCode.ShortCircuited.ToString()) logType = LogType.Warn;
            if (reportData.CircuitStatus == StatusCode.Timeout.ToString()) logType = LogType.Error;

            string appId;
            string cid;
            string originId;

            reportData.RequestContext.TryGetValue("x-appid", out appId);
            reportData.RequestContext.TryGetValue("x-cid", out cid);
            reportData.RequestContext.TryGetValue("x-origin", out originId);

            _logger.Log(logType, @"Hystrix rest request status 
                                    [ CircuitStatus: {0}, 
                                      CacheHit: {1}, 
                                      Duration: {2}ms, 
                                      Execution: {3}ms, 
                                      OccurredAt: {4}, 
                                      RequestBaseUrl: {5}, 
                                      RequestPath: {6}, 
                                      RequestMethod: {7}, 
                                      ResponseStatus: {8}, 
                                      AppId: {9},
                                      CID: {10},
                                      Origin: {11},
                                      Message: {12} ]",
                reportData.CircuitStatus,
                reportData.CacheHit,
                (long)reportData.Duration.TotalMilliseconds,
                (long)reportData.Execution.TotalMilliseconds,
                reportData.OccurredAt,
                reportData.RequestBaseUrl,
                reportData.RequestPath,
                reportData.RequestMethod,
                reportData.ResponseStatus,
                appId,
                cid,
                originId,
                reportData.Message);
        }
    }
}