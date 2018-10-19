using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Shared.Search
{
    [TestFixture]
    public class MapperExtensionsTests
    {
        [Test]
        public void FacetNodeIsRefineable()
        {
            //Arrange
            var dto = new FacetNodeDto
            {
                MetaData = new FacetNodeMetaDataDto
                {
                    IsRefineable = new List<bool> { true }
                }
            };
            var dtoWithEmptyList = new FacetNodeDto
            {
                MetaData = new FacetNodeMetaDataDto
                {
                    IsRefineable = new List<bool>()
                }
            };

            //Act
            var result = dto.IsRefineable();
            var resultWithEmptySource = dtoWithEmptyList.IsRefineable();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result, true);
            Assert.IsNotNull(resultWithEmptySource);
            Assert.AreEqual(resultWithEmptySource, false);
        }

        [Test]
        public void GetRefinementFromFacetNode()
        {
            //Arrange
            var dto = new FacetNodeDto
            {
                IsSelected = true,
                Refinements = new RyvussNavDto
                {
                    Nodes = new List<RyvussNavNodeDto>
                    {
                        new RyvussNavNodeDto
                        {
                            Facets = new List<FacetNodeDto>
                            {
                                new FacetNodeDto
                                {
                                    Action = "(And.Service.CarSales._.(Or.Make.BMW._.Make.Abarth.))",
                                    DisplayValue = "Series 1"
                                }
                            },
                            Name = "MarketingGroup"
                        },
                        new RyvussNavNodeDto
                        {
                            Facets = new List<FacetNodeDto>
                            {
                                new FacetNodeDto
                                {
                                    Action = "(And.Service.CarSales._.Type.Review.)",
                                    DisplayValue = "Nested-Review"
                                }
                            }
                        }
                    }
                }                
            };
            var dtoWithNoRefinements = new FacetNodeDto
            {
                IsSelected = false
            };

            //Act
            var result = dto.GetRefinements();
            var resultWithEmptySource = dtoWithNoRefinements.GetRefinements();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Facets.Count, 1);
            Assert.AreEqual(result.Facets.First().DisplayValue, "Series 1");
            Assert.AreEqual(resultWithEmptySource, null);
        }

        [Test]
        public void GetRefinementFromRyvussNavNode()
        {
            //Arrange
            var dto = new RyvussNavNodeDto
            {                
                MetaData = new RefinementsMetaDataDto
                {
                    Refinements = new List<RyvussNavNodeDto> { new RyvussNavNodeDto { DisplayName = "Review" } }
                }
            };
            var dtoWithNoRefinements = new RyvussNavNodeDto
            {
                MetaData = new RefinementsMetaDataDto()
            };

            //Act
            var result = dto.GetRefinements();
            var resultWithEmptySource = dtoWithNoRefinements.GetRefinements();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.DisplayName, dto.MetaData.Refinements.First().DisplayName);
            Assert.AreEqual(resultWithEmptySource, null);
        }

        [Test]
        public void GetDisplayNameTest()
        {
            //Arrange
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();
            tenantProvider.Current().ReturnsForAnyArgs(new TenantInfo()
            {
                Culture = new CultureInfo("es")
            });
            var dtoWithoutMake = new RyvussNavNodeDto
            {
                Name = "Article Type",
                DisplayName = "Article Type"
            };
            var dtoWithMake = new RyvussNavNodeDto
            {
                Name = "Make"
            };

            //Act
            var resultWithoutMake = dtoWithoutMake.GetDisplayName();
            var resultWithMake = dtoWithMake.GetDisplayName();

            //Assert
            Assert.IsNotNull(resultWithoutMake);
            Assert.AreEqual(resultWithMake, "Make/Model");
            Assert.AreEqual(resultWithoutMake, "Article Type");
        }
    }
}