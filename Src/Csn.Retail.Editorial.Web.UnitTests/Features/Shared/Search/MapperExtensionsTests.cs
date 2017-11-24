using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
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
                                    Value = "Abarth"
                                }
                            }
                        },
                        new RyvussNavNodeDto
                        {
                            Facets = new List<FacetNodeDto>
                            {
                                new FacetNodeDto
                                {
                                    Action = "(And.Service.CarSales._.Type.Review.)",
                                    Value = "Teview"
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
            Assert.AreEqual(result.Facets.Count, 2);
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
                    Refinements = new List<RefinementsNodeDto> { new RefinementsNodeDto { DisplayName = "Review" } }
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
            var dtoWithoutMake = new RyvussNavNodeDto
            {
                DisplayName = "Article Type"
            };
            var dtoWithMake = new RyvussNavNodeDto
            {
                DisplayName = "Make"
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