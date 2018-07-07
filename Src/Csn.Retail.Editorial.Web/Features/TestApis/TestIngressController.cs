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
    public class TestIngressController : ApiController
    {
        private readonly ISmartServiceClient _restClient;
        private readonly IRyvussDataService _ryvussDataService;
        private readonly IMapper _mapper;

        public TestIngressController(ISmartServiceClient restClient,
                                IRyvussDataService ryvussDataService,
                                IMapper mapper)
        {
            _restClient = restClient;
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("editorial/api/test/ingress/promotion")]
        public async Task<IHttpActionResult> GetPromotion([FromUri]string make)
        {
            return Ok(await GetPromotionData(make));
        }

        [HttpGet]
        [Route("editorial/api/test/ingress/carousels")]
        public async Task<IHttpActionResult> GetCarousels([FromUri]string make, [FromUri]int count = 5)
        {
            return Ok(await GetCarouselsData(make, count));
        }

        private async Task<TimingWrappedResult<CampaignAdResult>> GetPromotionData(string make)
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

        private async Task<TimingWrappedResult<List<object>>> GetCarouselsData(string make, int count)
        {
            var watch = new Stopwatch();
            watch.Start();

            // build up the list of ryvuss queries...each one can be just latest search results
            var carouselTask = GetCarousels(count);

            // make nav call
            var navTask = GetNavResults();

            // make promotion api call
            var adTask = GetPromotionData(make);

            await Task.WhenAll(carouselTask, navTask, adTask);

            watch.Stop();

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

            return new TimingWrappedResult<List<object>>
            {
                Data = new List<object> { stats, carouselTask.Result, navTask.Result, adTask.Result },
                TotalDuration = watch.ElapsedMilliseconds
            };
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

        private async Task<List<TimingWrappedResult<List<SearchResult>>>> GetCarousels(int count)
        {
            var tasks = new int[count].Select(x => GetCarouselData()).ToList();

            await Task.WhenAll(tasks);

            return tasks.Where(t => t.Result != null).Select(t => t.Result).ToList();
        }
    }
}