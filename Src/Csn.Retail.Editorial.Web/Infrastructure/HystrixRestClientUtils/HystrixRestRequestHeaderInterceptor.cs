using Csn.Hystrix.RestClient;
using Csn.Hystrix.RestClient.Dtos;
using Csn.RestClient.Dto;
using Csn.Retail.Editorial.Web.Infrastructure.RequestWrapper;

namespace Csn.Retail.Editorial.Web.Infrastructure.HystrixRestClientUtils
{
    public class HystrixRestRequestHeaderInterceptor : IHystrixRestRequestInterceptor
    {
        private const string AppId = "<NotSet>";
        private const string AppIdHeader = "x-appid";
        private const string CidHeader = "x-cid";
        private const string OriginKey = "x-origin";
        private readonly IRequestWrapper _request;

        public HystrixRestRequestHeaderInterceptor(IRequestWrapper request)
        {
            _request = request;
        }

        public void Intercept<T>(T restRequest) where T : HystrixRestRequest
        {
            var cid = _request.CorrelationId;
            restRequest.AddHeader(new Header(AppIdHeader, AppId));
            restRequest.AddHeader(new Header(CidHeader, cid));

            restRequest.AddRequestContext(AppIdHeader, AppId);
            restRequest.AddRequestContext(CidHeader, cid);
            restRequest.AddRequestContext(OriginKey, _request.Origin);
        }
    }
}