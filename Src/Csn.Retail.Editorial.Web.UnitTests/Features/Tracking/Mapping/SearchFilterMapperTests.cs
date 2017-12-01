using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Features.Tracking.Mapping;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Tracking.Mapping
{
    [TestFixture]
    class SearchFilterMapperTests
    {
        [Test]
        public void FacettedBreadcrumbWithChildren()
        {
            var navResult = new RyvussNavResultDto()
            {
                INav = new RyvussNavDto()
                {
                    BreadCrumbs = new List<BreadCrumbDto>()
                    {
                        new BreadCrumbDto()
                        {
                            AspectDisplay = "Make",
                            Facet = "TestMake",
                            Type = "FacetBreadCrumb",
                            Children = new List<BreadCrumbDto>()
                            {
                                new BreadCrumbDto()
                                {
                                    AspectDisplay = "Model",
                                    Facet = "TestModel",
                                    Type = "FacetBreadCrumb"
                                }
                            }
                        }
                    }
                }
            };

            var result = SearchFilterMapper.Map(navResult).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("TestMake", result[0].Value);
            Assert.AreEqual("TestModel", result[1].Value);
        }

        [Test]
        public void KeywordBreadcrumb()
        {
            var navResult = new RyvussNavResultDto()
            {
                INav = new RyvussNavDto()
                {
                    BreadCrumbs = new List<BreadCrumbDto>()
                    {
                        new BreadCrumbDto()
                        {
                            AspectDisplay = "Keyword",
                            Term = "Test",
                            Type = "KeywordBreadCrumb"
                        }
                    }
                }
            };

            var result = SearchFilterMapper.Map(navResult).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Test", result[0].Value);
        }
    }
}
