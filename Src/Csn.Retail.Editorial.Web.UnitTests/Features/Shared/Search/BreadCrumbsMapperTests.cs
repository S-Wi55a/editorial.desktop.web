﻿using System.Collections.ObjectModel;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Shared.Search
{
    [TestFixture]
    public class BreadCrumbsMapperTests
    {
        private IBreadCrumbMapper _testSubject;
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
            Mapper.Initialize(cfg => new MappingSetupTask(iMapper, _imageMapper, _testSubject, _resultsMessageMapper, _articleUrlMapper).Run(cfg));
            _testSubject = new BreadCrumbMapper(iMapper);
        }

        [Test]
        public void GetAggregatedBreadCrumbsTest()
        {
            //Arrange
            var source = new Collection<BreadCrumbDto>
            {
                new BreadCrumbDto {Aspect = "Aspect", RemoveAction = "Some Action"}                
            };

            //Act
            var results = _testSubject.GetAggregatedBreadCrumbs(source);

            //Assert
            Assert.IsNotNull(results);
            Assert.AreEqual(results.Count, 2);
            Assert.AreEqual(results[1].FacetDisplay, "Clear All");
        }
    }
}
