using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Landing.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Landing
{
    [AutoBind]
    public class GetLandingQueryHandler : IAsyncQueryHandler<GetLandingQuery, GetLandingResponse>
    {

        public async Task<GetLandingResponse> HandleAsync(GetLandingQuery query)
        {
            return new GetLandingResponse
            {
                LandingViewModel = new LandingViewModel
                {
                    Carousels = new List<CarouselViewModel>
                    {
                        new CarouselViewModel()
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
                                }
           
                            }
                        },
                        new Category
                        {
                            Title = "Review",
                            Link = "#",
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
                                }

                            }
                        }
                    }
                }
            };
        }

    }
}