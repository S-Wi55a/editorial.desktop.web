using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.Hero.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Ingress.HttpClient;
using Ingress.HttpClient.Abstracts;
using Ingress.ServiceClient.Abstracts;
using NewRelic.Api.Agent;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.TestApis
{
    public class TestExpHttpClientController : ApiController
    {
        private readonly IRyvussServiceExpHttpClient _ryvussDataService;
        private readonly IMapper _mapper;
        private readonly IHttpClient _httpClient;

        public TestExpHttpClientController(IRyvussServiceExpHttpClient ryvussDataService,
                                IMapper mapper,
                                IHttpClient httpClient)
        {
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("editorial/api/test/exp/carousels")]
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

            var request = new HttpRequestMessage(HttpMethod.Get, $"http://showroom-promotions-api.aws.csprd.com.au/v1/promotions/campaign/{campaignTag}");

            var watch = new Stopwatch();
            watch.Start();

            var response = await _httpClient.SendAsync(new HttpClientInput { Request = request, RetryCount = 0, Timeout = TimeSpan.FromSeconds(3)  });

            var contents = await response.Content.ReadAsStringAsync();

            watch.Stop();

            var result = JsonConvert.DeserializeObject<CampaignAdResult>(contents);

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

    public interface IRyvussExpHttpClientProxy
    {
        Task<T> GetAsync<T>(EditorialRyvussInput input);
    }

    [AutoBind]
    public class RyvussExpHttpClientProxy : IRyvussExpHttpClientProxy
    {
        private readonly IHttpClient _httpClient;

        public RyvussExpHttpClientProxy(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T> GetAsync<T>(EditorialRyvussInput input)
        {
            var queryparams = string.Join("&", GetQueryParams(input).Select(kvp => $"{kvp.Key}={kvp.Value}"));

            var request = new HttpRequestMessage(HttpMethod.Get, $"http://editorial.ryvuss.csprd.com.au/{GetPath(input)}?{queryparams}");

            var response = await _httpClient.SendAsync(new HttpClientInput { Request = request, Timeout = TimeSpan.FromSeconds(3), RetryCount = 0 });

            var contents = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(contents);
        }

        private string GetPath(EditorialRyvussInput input)
        {
            var path = new StringBuilder("/v4");

            if (!string.IsNullOrEmpty(input.ControllerName))
            {
                path.Append($"/{input.ControllerName}");
            }

            path.Append("/editoriallistingretail");

            if (!string.IsNullOrEmpty(input.ServiceProjectionName))
            {
                path.Append($"/{input.ServiceProjectionName}");
            }

            return path.ToString();
        }

        private Dictionary<string, string> GetQueryParams(EditorialRyvussInput input)
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(input.Query))
            {
                queryParams.Add("q", input.Query);
            }

            if (input.IncludeCount)
            {
                queryParams.Add("count", "true");
            }

            if (input.IncludeSearchResults)
            {
                queryParams.Add("sr", $"|{input.SortOrder}|{input.Offset}|{input.Limit}");
            }

            if (!string.IsNullOrEmpty(input.NavigationName))
            {
                var nav = input.PostProcessors == null || !input.PostProcessors.Any()
                    ? input.NavigationName
                    : string.Join("|", input.PostProcessors.Prepend(input.NavigationName));
                queryParams.Add("inav", nav);
            }

            if (input.IncludeMetaData)
            {
                queryParams.Add("metadata", "");
            }

            return queryParams;
        }
    }

    public interface IRyvussServiceExpHttpClient
    {
        Task<RyvussNavResultDto> GetNavAndResults(string query, bool includeResults, string sort = "", int offset = 0);
        Task<RyvussNavResultDto> GetResults(string query, int offset, string sort);
    }

    [AutoBind]
    public class RyvussServiceExpHttpClient : IRyvussServiceExpHttpClient
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IRyvussExpHttpClientProxy _RyvussExpHttpClientProxy;

        public RyvussServiceExpHttpClient(ITenantProvider<TenantInfo> tenantProvider, IRyvussExpHttpClientProxy RyvussExpHttpClientProxy)
        {
            _tenantProvider = tenantProvider;
            _RyvussExpHttpClientProxy = RyvussExpHttpClientProxy;
        }

        public async Task<RyvussNavResultDto> GetNavAndResults(string query, bool includeResults, string sort = "", int offset = 0)
        {
            var postProcessors = new List<string> { "Retail", "FacetSort" };
            AddPostprocessors(postProcessors);

            return await GetRyvusProxyResults(query, postProcessors, includeResults, sort, offset == 0 ? 0 : (offset * PageItemsLimit.ListingPageItemsLimit) - PageItemsLimit.ListingPageItemsLimit);
        }

        public Task<RyvussNavResultDto> GetResults(string query, int offset, string sort)
        {
            return _RyvussExpHttpClientProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput
            {
                Query = query,
                IncludeSearchResults = true,
                Offset = offset,
                Limit = 7,
                SortOrder = sort,
                IncludeCount = true
            });
        }

        private void AddPostprocessors(ICollection<string> postProcessors)
        {
            if (_tenantProvider.Current().SupportsSeoFriendlyListings)
            {
                postProcessors.Add("Seo");
                postProcessors.Add("HideAspect(Service)");
            }
            else
            {
                postProcessors.Add("ShowZero");
            }
            postProcessors.Add("RenderRefinements");
        }

        private Task<RyvussNavResultDto> GetRyvusProxyResults(string query, List<string> postProcessors, bool includeResults = false, string sort = "", int offset = 0)
        {
            var ryvusInput = new EditorialRyvussInput
            {
                Query = query,
                IncludeCount = true,
                IncludeSearchResults = includeResults,
                ControllerName = _tenantProvider.Current().SupportsSeoFriendlyListings ? $"seo-{_tenantProvider.Current().Name}" : null,
                ServiceProjectionName = _tenantProvider.Current().SupportsSeoFriendlyListings ? _tenantProvider.Current().RyvusServiceProjection : "",
                NavigationName = _tenantProvider.Current().RyvusNavName,
                PostProcessors = postProcessors,
                IncludeMetaData = true
            };

            if (includeResults)
            {
                ryvusInput.Offset = offset;
                ryvusInput.Limit = PageItemsLimit.ListingPageItemsLimit;
                ryvusInput.SortOrder = EditorialSortKeyValues.IsValidSort(sort) ? sort : EditorialSortKeyValues.ListingPageDefaultSort;
            }

            return _RyvussExpHttpClientProxy.GetAsync<RyvussNavResultDto>(ryvusInput);
        }
    }
}