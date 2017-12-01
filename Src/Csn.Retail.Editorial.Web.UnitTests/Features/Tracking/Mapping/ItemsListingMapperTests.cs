using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Features.Tracking.Mapping;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Tracking.Mapping
{
    class ItemsListingMapperTests
    {
        [Test]
        public void TestMakesListingSingle()
        {
            var result = ItemsListingMapper.MapMakes(new SearchResultDto()
            {
                Items = new List<EditorialListingItemDto>()
                {
                    new EditorialListingItemDto()
                    {
                        Make = "Test"
                    },
                    new EditorialListingItemDto()
                    {
                        Year = 0
                    }
                }
            });

            Assert.AreEqual("Test", result);
        }

        [Test]
        public void TestMakesListingMultiple()
        {
            var result = ItemsListingMapper.MapMakes(new SearchResultDto()
            {
                Items = new List<EditorialListingItemDto>()
                {
                    new EditorialListingItemDto()
                    {
                        Make = "Test"
                    },
                    new EditorialListingItemDto()
                    {
                        Make = "Test2"
                    },
                    new EditorialListingItemDto()
                    {
                        Make = "Test"
                    }
                }
            });

            Assert.AreEqual("Test,Test2", result);
        }

        [Test]
        public void TestModelsListingMultiple()
        {
            var result = ItemsListingMapper.MapModels(new SearchResultDto()
            {
                Items = new List<EditorialListingItemDto>()
                {
                    new EditorialListingItemDto()
                    {
                        Make = "Test",
                        Model = "TestModel"
                    },
                    new EditorialListingItemDto()
                    {
                        Make = "TestMake2",
                        Model = "TestModel2"
                    },
                }
            });

            Assert.AreEqual("TestModel,TestModel2", result);
        }
    }
}
