using System.Collections.Generic;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Landing.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;
using Ingress.ServiceClient.Abstracts;

namespace Csn.Retail.Editorial.Web.Features.Landing
{
    [AutoBind]
    public class GetLandingQueryHandler : IAsyncQueryHandler<GetLandingQuery, GetLandingResponse>
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IEditorialRyvussApiProxy _ryvussProxy;
        private readonly IMapper _mapper;
        private ISmartServiceClient _restClient;

        public GetLandingQueryHandler(ITenantProvider<TenantInfo> tenantProvider, IEditorialRyvussApiProxy ryvussProxy, IMapper mapper, ISmartServiceClient restClient)
        {
            _tenantProvider = tenantProvider;
            _ryvussProxy = ryvussProxy;
            _mapper = mapper;
            _restClient = restClient;
        }

        public async Task<GetLandingResponse> HandleAsync(GetLandingQuery query)
        {

            var navResults = GetNav();

            var campaignAd = GetAdUnit();

            await Task.WhenAll(navResults, campaignAd);

            return new GetLandingResponse
            {
                LandingViewModel = new LandingViewModel
                {
                    Title = "Search All News & Reviews",
                    Nav = new Models.Nav()
                    {
                        NavResults = navResults.Result
                    },
                    Carousels = new List<CarouselViewModel>
                    {
                        new CarouselViewModel
                        {
                            Title = "Featured",
                            Link = "#",
                            HasMrec = true,
                            CategoryItems = new List<SearchResult>
                            {
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                },
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                },
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                },
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                },
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                },
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                }

                            }
                        },
                        new CarouselViewModel
                        {
                            Title = "Reviews",
                            Link = "#",
                            HasMrec = false,
                            CategoryItems = new List<SearchResult>
                            {
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                },
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                },
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                },
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                },
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                },
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                },
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                },
                                new SearchResult()
                                {
                                    DisqusArticleId=  "EDITORIAL-110025",
                                    Headline = "LA MOTOR SHOW: Chevrolet drops Corvette ZR1\'s top",
                                    SubHeading = "First Corvette ZR1 convertible since 1970 revealed; could come Down Under",
                                    ImageUrl = "https://carsales.pxcrush.net/general/editorial/LA17-Chevrolet-Corvette-ZR1-1.jpg",
                                    DateAvailable = "November 30th",
                                    ArticleDetailsUrl = "/editorial/details/la-motor-show-chevrolet-drops-corvette-zr1s-top-110025/",
                                    Label = null,
                                    Type = "News"
                                }

                            }
                        }
                    },
                    CampaignAd = campaignAd.Result

                }
            };
        }

        /// <summary>
        /// This is just a temp solution until we properly architect this part out
        /// </summary>
        /// <returns></returns>
        private async Task<NavResult> GetNav()
        {
            var queryString = _tenantProvider.Current().SupportsSeoFriendlyListings ? string.Empty : $"Service.{_tenantProvider.Current().Name}.";

            var postProcessors = new List<string>();
            postProcessors.AddRange(new[] { "Retail", "FacetSort", "RenderRefinements" });

            if (_tenantProvider.Current().SupportsSeoFriendlyListings)
            {
                postProcessors.Add("Seo");
                postProcessors.Add("HideAspect(Service)");
            }
            else
            {
                postProcessors.Add("ShowZero");
            }

            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput
            {
                Query = queryString,
                IncludeCount = true,
                ControllerName = _tenantProvider.Current().SupportsSeoFriendlyListings ? $"seo-{_tenantProvider.Current().Name}" : "",
                ServiceProjectionName = _tenantProvider.Current().SupportsSeoFriendlyListings ? _tenantProvider.Current().Name : "",
                NavigationName = _tenantProvider.Current().RyvusNavName,
                PostProcessors = postProcessors,
                IncludeMetaData = true
            });

            var resultData = !result.IsSucceed ? null : result.Data;

            if (resultData == null) return null;

            return _mapper.Map<NavResult>(resultData);
        }

        private async Task<CampaignAdResult> GetAdUnit()
        {
            return await _restClient.Service("api-retail-homepage")
                .Path("v2/carsales")
                .GetAsync<HomePageModel>()
                //.WithDefaultData(new HeroAdUnit())
                .ContinueWith(x => x.Result.Data.CampaignAd);
        }


    }
}