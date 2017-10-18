using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.MediaMotiveAds.TagBuilders
{
    class ListingsBreadCrumbTagBuilderTests
    {
        [Test]
        public void SingleBreadCrumbNoChildren()
        {
            var builder = new ListingsBreadCrumbTagBuilder();

            var navResult = new RyvussNavResultDto()
            {
                INav = new RyvussNavDto()
                {
                    BreadCrumbs = new List<BreadCrumbDto>()
                    {
                        new BreadCrumbDto()
                        {
                            Aspect = "Make",
                            Facet = "Honda"
                        }
                    }
                }
            };

            var result = builder.BuildTags(navResult).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("honda", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Make).Values.First());
        }

        [Test]
        public void BreadCrumbWithChildren()
        {
            var builder = new ListingsBreadCrumbTagBuilder();

            var navResult = new RyvussNavResultDto()
            {
                INav = new RyvussNavDto()
                {
                    BreadCrumbs = new List<BreadCrumbDto>()
                    {
                        new BreadCrumbDto()
                        {
                            Aspect = "Make",
                            Facet = "Honda",
                            Children = new List<BreadCrumbDto>()
                            {
                                new BreadCrumbDto()
                                {
                                    Aspect = "Model",
                                    Facet = "Civic"
                                }
                            }
                        }
                    }
                }
            };

            var result = builder.BuildTags(navResult).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("honda", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Make).Values.First());
            Assert.AreEqual("civic", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Model).Values.First());
        }

        [Test]
        public void BreadCrumbWithMultipleLevelsOfChildren()
        {
            var builder = new ListingsBreadCrumbTagBuilder();

            var navResult = new RyvussNavResultDto()
            {
                INav = new RyvussNavDto()
                {
                    BreadCrumbs = new List<BreadCrumbDto>()
                    {
                        new BreadCrumbDto()
                        {
                            Aspect = "Make",
                            Facet = "BMW",
                            Children = new List<BreadCrumbDto>()
                            {
                                new BreadCrumbDto()
                                {
                                    Aspect = "MarketingGroup",
                                    Facet = "3 Series",
                                    Children = new List<BreadCrumbDto>()
                                    {
                                        new BreadCrumbDto()
                                        {
                                            Aspect = "Model",
                                            Facet = "318i"
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var result = builder.BuildTags(navResult).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("bmw", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Make).Values.First());
            Assert.AreEqual("318i", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Model).Values.First());
        }

        [Test]
        public void MultipleBreadCrumbsWithChildren()
        {
            var builder = new ListingsBreadCrumbTagBuilder();

            var navResult = new RyvussNavResultDto()
            {
                INav = new RyvussNavDto()
                {
                    BreadCrumbs = new List<BreadCrumbDto>()
                    {
                        new BreadCrumbDto()
                        {
                            Aspect = "Make",
                            Facet = "BMW",
                            Children = new List<BreadCrumbDto>()
                            {
                                new BreadCrumbDto()
                                {
                                    Aspect = "MarketingGroup",
                                    Facet = "3 Series",
                                    Children = new List<BreadCrumbDto>()
                                    {
                                        new BreadCrumbDto()
                                        {
                                            Aspect = "Model",
                                            Facet = "318i"
                                        }
                                    }
                                }
                            }
                        },
                        new BreadCrumbDto()
                        {
                            Aspect = "Make",
                            Facet = "Holden",
                            Children = new List<BreadCrumbDto>()
                            {
                                new BreadCrumbDto()
                                {
                                    Aspect = "Model",
                                    Facet = "Commodore"
                                }
                            }
                        },
                        new BreadCrumbDto()
                        {
                            Aspect = "Type",
                            Facet = "Review"
                        },
                        new BreadCrumbDto()
                        {
                            Aspect = "Type",
                            Facet = "News"
                        }
                    }
                }
            };

            var result = builder.BuildTags(navResult).ToList();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("bmw", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Make).Values.First());
            Assert.AreEqual("318i", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Model).Values.First());
            Assert.AreEqual("review", result.First(t => t.Name == SasAdTags.SasAdTagKeys.ArticleType).Values.First());
        }
    }
}
