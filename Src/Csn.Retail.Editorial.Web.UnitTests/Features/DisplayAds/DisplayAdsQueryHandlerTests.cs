using System.Collections.Generic;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.DisplayAds;
using Csn.Retail.Editorial.Web.Features.DisplayAds.GoogleAd.Models;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.DisplayAds
{
    class DisplayAdsQueryHandlerTests
    {
        [Test]
        public void MultipleTagsWithDuplicates()
        {
            
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();
            tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "soloautos",
                DisplayAdsSource = DisplayAdsSource.GoogleAd
            });

            var tagBuilders = new List<IMediaMotiveTagBuilder>();

            var queryHandler = new DisplayAdsQueryHandler(tagBuilders, tenantProvider);

            //Act
            var result = (GoogleAdViewModel) queryHandler.Handle(new DisplayAdsQuery()
            {
                AdType = DisplayAdsTypes.Aside
            });

            Assert.AreEqual("[{\"Width\":300,\"Height\":250},{\"Width\":300,\"Height\":600}]", result.Dimensions);
        }
    }
}
