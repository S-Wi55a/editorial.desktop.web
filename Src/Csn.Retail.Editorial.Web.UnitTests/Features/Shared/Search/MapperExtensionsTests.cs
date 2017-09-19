using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using NUnit.Framework;
using IMapper = Csn.Retail.Editorial.Web.Infrastructure.Mappers.IMapper;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Shared.Search
{
    [TestFixture]
    public class MapperExtensionsTests
    {
        //private IMapper _iMapper;

        [SetUp]
        public void FixtureSetUp()
        {
            //var iMapper = new AutoMappedMapper();
            //var iBreadCrumbMapper 

            //Mapper.Initialize(cfg => new MappingSetupTask(iMapper).Run(cfg));            
        }

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
                MetaData = new FacetNodeMetaDataDto
                {
                    Refinements = new List<RefinementsNodeDto> { new RefinementsNodeDto {  DisplayName = "Review" } }
                }
            };
            var dtoWithNoRefinements = new FacetNodeDto
            {
                MetaData = new FacetNodeMetaDataDto()                
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
            var dtoWithNoRefinements = new FacetNodeDto
            {
                MetaData = new FacetNodeMetaDataDto()
            };

            //Act
            var result = dto.GetRefinements();
            var resultWithEmptySource = dtoWithNoRefinements.GetRefinements();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.DisplayName, dto.MetaData.Refinements.First().DisplayName);
            Assert.AreEqual(resultWithEmptySource, null);
        }

        
    }
}