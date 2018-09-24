using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.DisplayAds;
using Csn.Retail.Editorial.Web.Features.DisplayAds.GoogleAd;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.DisplayAds
{
    class GoogleAdQueryHandlerTests
    {
        [Test]
        public void MultipleTagsWithDuplicates()
        {
            
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();
            tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "soloautos"
            });

            var queryHandler = new GoogleAdQueryHandler(tenantProvider);

            //Act
            var result = queryHandler.Handle(new DisplayAdQuery()
            {
                AdPlacement = DisplayAdPlacements.Aside
            });

            Assert.AreEqual("[{\"Width\":300,\"Height\":250},{\"Width\":300,\"Height\":600}]", result.Dimensions);
        }
    }
}
