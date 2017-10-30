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
                            Facet = "Honda",
                            Type = "FacetBreadCrumb"
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
                            Type = "FacetBreadCrumb",
                            Children = new List<BreadCrumbDto>()
                            {
                                new BreadCrumbDto()
                                {
                                    Aspect = "Model",
                                    Facet = "Civic",
                                    Type = "FacetBreadCrumb"
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
                            Type = "FacetBreadCrumb",
                            Children = new List<BreadCrumbDto>()
                            {
                                new BreadCrumbDto()
                                {
                                    Aspect = "MarketingGroup",
                                    Facet = "3 Series",
                                    Type = "FacetBreadCrumb",
                                    Children = new List<BreadCrumbDto>()
                                    {
                                        new BreadCrumbDto()
                                        {
                                            Aspect = "Model",
                                            Facet = "318i",
                                            Type = "FacetBreadCrumb"
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
                            Type = "FacetBreadCrumb",
                            Children = new List<BreadCrumbDto>()
                            {
                                new BreadCrumbDto()
                                {
                                    Aspect = "MarketingGroup",
                                    Facet = "3 Series",
                                    Type = "FacetBreadCrumb",
                                    Children = new List<BreadCrumbDto>()
                                    {
                                        new BreadCrumbDto()
                                        {
                                            Aspect = "Model",
                                            Facet = "318i",
                                            Type = "FacetBreadCrumb",
                                        }
                                    }
                                }
                            }
                        },
                        new BreadCrumbDto()
                        {
                            Aspect = "Make",
                            Facet = "Holden",
                            Type = "FacetBreadCrumb",
                            Children = new List<BreadCrumbDto>()
                            {
                                new BreadCrumbDto()
                                {
                                    Aspect = "Model",
                                    Facet = "Commodore",
                                    Type = "FacetBreadCrumb"
                                }
                            }
                        },
                        new BreadCrumbDto()
                        {
                            Aspect = "Type",
                            Facet = "Review",
                            Type = "FacetBreadCrumb"
                        },
                        new BreadCrumbDto()
                        {
                            Aspect = "Type",
                            Facet = "News",
                            Type = "FacetBreadCrumb"
                        },
                        new BreadCrumbDto()
                        {
                            Term = "(honda)",
                            Type = "KeywordBreadCrumb"
                        }
                    }
                }
            };

            var result = builder.BuildTags(navResult).ToList();

            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("bmw", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Make).Values.First());
            Assert.AreEqual("318i", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Model).Values.First());
            Assert.AreEqual("review", result.First(t => t.Name == SasAdTags.SasAdTagKeys.ArticleType).Values.First());
            Assert.AreEqual("honda", result.First(t => t.Name == SasAdTags.SasAdTagKeys.Keyword).Values.First());
        }
    }
}
