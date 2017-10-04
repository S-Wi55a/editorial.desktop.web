using System.Collections.Generic;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using NSubstitute;
using NUnit.Framework;
using IMapper = Csn.Retail.Editorial.Web.Infrastructure.Mappers.IMapper;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Shared.Search
{
    [TestFixture]
    public class MapperTests
    {
        private IImageMapper _imageMapperMocks;
        private IBreadCrumbMapper _iBreadCrumbMapperMock;
        private IMapper _testSubject;


        [SetUp]
        public void Setup()
        {

            _testSubject = new AutoMappedMapper();
            _imageMapperMocks = Substitute.For<IImageMapper>();
            _iBreadCrumbMapperMock = Substitute.For<IBreadCrumbMapper>();
            Mapper.Initialize(cfg => new MappingSetupTask(_testSubject, _imageMapperMocks, _iBreadCrumbMapperMock).Run(cfg));

        }
        [Test]
        public void GetNavNodeFromRefinementsNodeDtoTest()
        {
            //Arrange
            var source = new RefinementsNodeDto
            {
                Name = "Sources",
                Facets = new List<FacetNodeDto>
                {
                    new FacetNodeDto
                    {
                        MetaData = new FacetNodeMetaDataDto
                        {
                            Refinements = new List<RefinementsNodeDto> {new RefinementsNodeDto {Name = "Apples" }}
                        }
                    }
                }
            };

            //Act
            var results = _testSubject.Map<NavNode>(source);

            //Assert
            Assert.IsNotNull(results, "Results must have some value");
            Assert.IsNull(results.MultiSelectMode);
        }
    }
}
