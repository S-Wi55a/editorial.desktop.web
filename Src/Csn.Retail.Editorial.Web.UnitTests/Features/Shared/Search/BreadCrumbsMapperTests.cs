using System.Collections.Generic;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Shared.Search
{
    [TestFixture]
    public class BreadCrumbsMapperTests
    {
        private IImageMapper _imageMapper;
        private IResultsMessageMapper _resultsMessageMapper;
        private IArticleUrlMapper _articleUrlMapper;
        [SetUp]
        public void Setup()
        {
            var iMapper = new AutoMappedMapper();
            _imageMapper = Substitute.For<IImageMapper>();
            _resultsMessageMapper = Substitute.For<IResultsMessageMapper>();
            _articleUrlMapper = Substitute.For<IArticleUrlMapper>();
            Mapper.Initialize(cfg => new MappingSetupTask(iMapper, _imageMapper, _resultsMessageMapper, _articleUrlMapper).Run(cfg));
        }

        [Test]
        public void GetAggregatedBreadCrumbsTest()
        {
            var source = new RyvussNavDto
            {

                BreadCrumbs = new List<BreadCrumbDto>
                {
                    new BreadCrumbDto
                    {
                        Aspect = "Aspect1",
                        RemoveAction = "Some Action",
                        Type = "FacetBreadCrumb",
                        Children = new List<BreadCrumbDto>
                        {
                            new BreadCrumbDto
                            {
                                Aspect = "TestAspect1",
                                Facet = "TestFacet1",
                                Type = "FacetBreadCrumb"
                            }
                        }
                    },
                    new BreadCrumbDto
                    {
                        Aspect = "Aspect2",
                        RemoveAction = "Some Action",
                        Type = "FacetBreadCrumb",
                        Children = new List<BreadCrumbDto>
                        {
                            new BreadCrumbDto
                            {
                                Aspect = "TestAspect2",
                                Facet = "TestFacet2",
                                Type = "FacetBreadCrumb",
                            }
                        }
                    }
                },
                Nodes = new List<RyvussNavNodeDto>()
            };

            //Act
            var results = Mapper.Map<Nav>(source);

            //Assert
            Assert.IsNotNull(results);
            Assert.IsNotNull(results.BreadCrumbs);
            Assert.AreEqual(results.BreadCrumbs.Count, 5);
            Assert.AreEqual(results.BreadCrumbs[4].FacetDisplay, "Clear All");
        }
    }
}