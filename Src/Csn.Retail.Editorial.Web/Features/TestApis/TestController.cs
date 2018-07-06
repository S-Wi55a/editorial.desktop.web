using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc.Html;
using Csn.Retail.Editorial.Web.Features.Shared.Hero.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Ingress.ServiceClient.Abstracts;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.TestApis
{
    public class TestController : ApiController
    {
        private readonly ISmartServiceClient _restClient;
        private readonly IRyvussDataService _ryvussDataService;
        private readonly IMapper _mapper;

        public TestController(ISmartServiceClient restClient,
                                IRyvussDataService ryvussDataService,
                                IMapper mapper)
        {
            _restClient = restClient;
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("editorial/api/test-ingress")]
        public async Task<IHttpActionResult> GetIngress([FromUri]string make)
        {
            return Ok(await GetDataUsingIngress(make));
        }

        [HttpGet]
        [Route("editorial/api/test-ingress-carousels")]
        public async Task<IHttpActionResult> GetIngressWithCarousels([FromUri]string make)
        {
            return Ok(await GetCarouselsWithIngress(make));
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

        private async Task<List<object>> GetCarouselsWithIngress(string make)
        {
            // build up the list of ryvuss queries...each one can be just latest search results
            var carouselTask = GetCarousels();

            // make nav call
            var navTask = GetNavResults();

            // make promotion api call
            var adTask = GetDataUsingIngress(make);

            await Task.WhenAll(carouselTask, navTask, adTask);

            var stats = new List<Stats>
            {
                new Stats()
                {
                    Name = "Promotion",
                    TotalDuration = adTask.Result.TotalDuration
                },
                new Stats()
                {
                    Name = "Nav",
                    TotalDuration = navTask.Result.TotalDuration
                }
            };

            stats.AddRange(carouselTask.Result.Select(i => new Stats(){ Name = "CarouselData", TotalDuration = i.TotalDuration}));

            return new List<object>{ stats, carouselTask.Result, navTask.Result, adTask.Result};
        }

        private async Task<TimingWrappedResult<RyvussNavResultDto>> GetNavResults()
        {
            var watch = new Stopwatch();
            watch.Start();

            var ryvussResults = await _ryvussDataService.GetNavAndResults(string.Empty, false);

            watch.Stop();

            return new TimingWrappedResult<RyvussNavResultDto>
            {
                Data = ryvussResults,
                TotalDuration = watch.ElapsedMilliseconds
            };
        }

        private async Task<TimingWrappedResult<List<SearchResult>>> GetCarouselData()
        {
            var watch = new Stopwatch();
            watch.Start();

            var result = await _ryvussDataService.GetResults("Service.Carsales.", 0, "Latest");

            watch.Stop();

            var landingResults = _mapper.Map<NavResult>(result);

            return new TimingWrappedResult<List<SearchResult>>
            {
                Data = landingResults.SearchResults,
                TotalDuration = watch.ElapsedMilliseconds
            };
        }

        private async Task<List<TimingWrappedResult<List<SearchResult>>>> GetCarousels()
        {
            var tasks = new List<int>{1, 2, 3, 4, 5}.Select(x => GetCarouselData()).ToList();

            await Task.WhenAll(tasks);

            return tasks.Where(t => t.Result != null).Select(t => t.Result).ToList();
        }
    }

    public class TimingWrappedResult<T>
    {
        public T Data { get; set; }
        public long TotalDuration { get; set; }
    }

    public class Stats
    {
        public string Name { get; set; }
        public long TotalDuration { get; set; }
    }
}