using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Features.Tracking.Mapping;
using Expresso.Parser;
using Expresso.Sanitisation;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Tracking.Mapping
{
    [TestFixture]
    class TrackingPropertiesMapperTests
    {
        [Test]
        public void TestGetTrackingProperties_Keyword()
        {
            var contextStore = Substitute.For<IPageContextStore>();

            contextStore.Get().Returns(new ListingPageContext()
            {
                RyvussNavResult = new RyvussNavResultDto()
                {
                    Metadata = new PageLevelSeoMetaDataDto()
                    {
                        Query = "(And.Service.CarSales._.Keywords.keyword(Test).)"
                    }
                }
            });

            var mapper = new TrackingPropertiesBuilder(contextStore, new RoseTreeParser(new RoseTreeSanitiser()));

            var result = mapper.Get().ToList();

            Assert.AreEqual("Keyword", result.First().Key);
            Assert.AreEqual("Test", result.First().Value);
        }
    }
}
