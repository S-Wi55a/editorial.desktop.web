using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Csn.Retail.Editorial.Web.Features.Shared.Hero.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Ingress.ServiceClient.Abstracts;

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
        public async Task<IHttpActionResult> GetCarousels([FromUri]string make, [FromUri]bool includePromotion = true, [FromUri]int count = 5)
        {
            return Ok(await GetCarouselsData(make, count, includePromotion));
        }

        private async Task<List<TimingWrappedResult>> GetPromotionData(string make)
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

            return new List<TimingWrappedResult>
            {
                new TimingWrappedResult()
                {
                    Name = "promotion",
                    TotalDuration = watch.ElapsedMilliseconds
                }
            };
        }

        private async Task<TotalTiming> GetCarouselsData(string make, int count, bool includePromotionData)
        {
            var watch = new Stopwatch();
            watch.Start();

            var tasks = new List<Task<List<TimingWrappedResult>>>();

            // build up the list of ryvuss queries...each one can be just latest search results
            tasks.Add(GetCarousels(count));

            // make nav call
            tasks.Add(GetNavResults());

            // make promotion api call
            if (includePromotionData)
            {
                tasks.Add(GetPromotionData(make));
            }

            await Task.WhenAll(tasks);

            watch.Stop();

            var results = tasks.Select(t => t.Result).ToList();

            var timingResults = new List<TimingWrappedResult>();

            foreach (var result in results)
            {
                timingResults.AddRange(result);
            }

            return new TotalTiming()
            {
                TotalDuration = watch.ElapsedMilliseconds,
                Timings = timingResults
            };
        }

        private async Task<List<TimingWrappedResult>> GetNavResults()
        {
            var watch = new Stopwatch();
            watch.Start();

            var ryvussResults = await _ryvussDataService.GetNavAndResults(string.Empty, false);

            watch.Stop();

            return new List<TimingWrappedResult>
            {
                new TimingWrappedResult()
                {
                    Name = "Nav",
                    TotalDuration = watch.ElapsedMilliseconds
                }
            };
        }

        private async Task<TimingWrappedResult> GetCarouselData()
        {
            var watch = new Stopwatch();
            watch.Start();

            var result = await _ryvussDataService.GetResults("Service.Carsales.", 0, "Latest");

            watch.Stop();

            var landingResults = _mapper.Map<NavResult>(result);

            return new TimingWrappedResult()
            {
                //Data = landingResults.SearchResults,
                Name = "Carousel",
                TotalDuration = watch.ElapsedMilliseconds
            };
        }

        private async Task<List<TimingWrappedResult>> GetCarousels(int count)
        {
            var tasks = new int[count].Select(x => GetCarouselData()).ToList();

            await Task.WhenAll(tasks);

            return tasks.Where(t => t.Result != null).Select(t => t.Result).ToList();
        }
    }

    public class TotalTiming
    {
        public long TotalDuration { get; set; }
        public List<TimingWrappedResult> Timings { get; set; }
    }
}