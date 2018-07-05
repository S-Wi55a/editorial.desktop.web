using System;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Web.Http;
using Csn.Retail.Editorial.Web.Features.Shared.Hero.Models;
using Ingress.ServiceClient.Abstracts;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.TestApis
{
    public class TestController : ApiController
    {
        private readonly ISmartServiceClient _restClient;

        public TestController(ISmartServiceClient restClient)
        {
            _restClient = restClient;
        }

        [HttpGet]
        [Route("editorial/api/test-ingress")]
        public async Task<IHttpActionResult> GetIngress([FromUri]string make)
        {
            return Ok(await GetDataUsingIngress(make));
        }

        [HttpGet]
        [Route("editorial/api/test-no-ingress")]
        public async Task<IHttpActionResult> GetWithoutIngress([FromUri]string make)
        {
            return Ok(await GetDataWithoutIngress(make));
        }

        private async Task<TimingWrappedResult<CampaignAdResult>> GetDataUsingIngress(string make)
        {
            var campaignTag = $"?Tenant=carsales";

            if (!string.IsNullOrEmpty(make))
                campaignTag += $"&PromotionType=EditorialMakePage&Make={make}";
            else
                campaignTag += "&PromotionType=EditorialHomePage";

            var watch = new Stopwatch();
            watch.Start();

            var result = await _restClient.Service("api-showroom-promotions")
                .Path($"/v1/promotions/campaign/{campaignTag}")
                .GetAsync<CampaignAdResult>()
                .ContinueWith(x => x.Result.Data);

            watch.Stop();

            return new TimingWrappedResult<CampaignAdResult>
            {
                Data = result,
                TotalDuration = watch.ElapsedMilliseconds
            };
        }

        private async Task<TimingWrappedResult<CampaignAdResult>> GetDataWithoutIngress(string make)
        {
            var campaignTag = $"?Tenant=carsales";

            if (!string.IsNullOrEmpty(make))
                campaignTag += $"&PromotionType=EditorialMakePage&Make={make}";
            else
                campaignTag += "&PromotionType=EditorialHomePage";

            var client = new HttpClient
            {
                BaseAddress = new Uri("http://showroom-promotions-api.aws.csprd.com.au")
            };

            var request = new HttpRequestMessage(HttpMethod.Get, $"/v1/promotions/campaign/{campaignTag}");

            var watch = new Stopwatch();
            watch.Start();

            var response = await client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            watch.Stop();

            return new TimingWrappedResult<CampaignAdResult>
            {
                Data = JsonConvert.DeserializeObject<CampaignAdResult>(result),
                TotalDuration = watch.ElapsedMilliseconds
            };
        }
    }

    public class TimingWrappedResult<T>
    {
        public T Data { get; set; }
        public long TotalDuration { get; set; }
    }
}