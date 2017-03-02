using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Csn.Hystrix.RestClient;
using Csn.Hystrix.RestClient.Dtos;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using System.Net;
using System.Net.Http;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.TrackingApi
{
    public interface ITrackingApiProxy
    {
        TrackingApiDto GetTracking(TrackingApiInput input);
    }

    [AutoBind]
    public class TrackingApiProxy : ITrackingApiProxy
    {
        private const string HostName = "TrackingScriptApiProxy";
        private readonly IFluentHystrixRestClientFactory _restClient;

        public TrackingApiProxy(IFluentHystrixRestClientFactory restClient)
        {
            _restClient = restClient;
        }

        public TrackingApiDto GetTracking(TrackingApiInput input)
        {
            var response = _restClient.HostName(HostName)
                .Path("v1/api/tracking/script")
                .QueryParams("Application", input.ApplicationName)
                .Get<TrackingApiDto>();

            return response.Result;
        }
    }
}