using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc.Html;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;
using Csn.Retail.Editorial.Web.Features.Shared.Hero.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Features.Shared.Services;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Ingress.ServiceClient.Abstracts;
using NewRelic.Api.Agent;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.TestApis
{
    public class TestHttpClientController : ApiController
    {
        private readonly IRyvussHttpClientDataService _ryvussDataService;
        private readonly IMapper _mapper;

        public TestHttpClientController(IRyvussHttpClientDataService ryvussDataService,
                                IMapper mapper)
        {
            _ryvussDataService = ryvussDataService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("editorial/api/test/http-client/promotion")]
        public async Task<IHttpActionResult> GetPromotion([FromUri]string make)
        {
            return Ok(await GetPromotionData(make));
        }

        [HttpGet]
        [Route("editorial/api/test/http-client/carousels")]
        public async Task<IHttpActionResult> GetCarousels([FromUri]string make, [FromUri]int count)
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

            stats.AddRange(carouselTask.Result.Select(i => new Stats() { Name = "CarouselData", TotalDuration = i.TotalDuration }));

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

    public interface IRyvussApiHttpClientProxy
    {
        Task<T> GetAsync<T>(EditorialRyvussInput input);
    }

    [AutoBind]
    public class RyvussApiHttpClientProxy : IRyvussApiHttpClientProxy
    {
        public async Task<T> GetAsync<T>(EditorialRyvussInput input)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://editorial.ryvuss.csprd.com.au")
            };

            var request = new HttpRequestMessage(HttpMethod.Get, $"{GetPath(input)}?{GetQueryParams(input)}");

            var response = await client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
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

    public interface IRyvussHttpClientDataService
    {
        Task<RyvussNavResultDto> GetNavAndResults(string query, bool includeResults, string sort = "", int offset = 0);
        Task<RyvussNavResultDto> GetResults(string query, int offset, string sort);
    }

    [AutoBind]
    public class RyvussHttpClientDataService : IRyvussHttpClientDataService
    {
        private readonly IRyvussApiHttpClientProxy _ryvussProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public RyvussHttpClientDataService(IRyvussApiHttpClientProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
        }

        public async Task<RyvussNavResultDto> GetNavAndResults(string query, bool includeResults, string sort = "", int offset = 0)
        {
            var postProcessors = new List<string> { "Retail", "FacetSort" };
            AddPostprocessors(postProcessors);

            return await GetRyvusProxyResults(query, postProcessors, includeResults, sort, offset);
        }

        public async Task<RyvussNavResultDto> GetResults(string query, int offset, string sort)
        {
            return await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput
            {
                Query = query,
                IncludeSearchResults = true,
                Offset = offset,
                Limit = 7,
                SortOrder = sort,
                IncludeCount = true
            });
        }

        #region Private
        private async Task<RyvussNavResultDto> GetRyvusProxyResults(string query, List<string> postProcessors, bool includeResults = false, string sort = "", int offset = 0)
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

            return await _ryvussProxy.GetAsync<RyvussNavResultDto>(ryvusInput);
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
        #endregion
    }
}