using System.Collections.Generic;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.DisplayAds;
using Csn.Retail.Editorial.Web.Features.DisplayAds.Models;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.MediaMotiveAds
{
    class MediaMotiveAdQueryHandlerTests
    {
        
        [Test]
        public void MultipleTagsWithDuplicates()
        {
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();
            tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "carsales",
                AdUnits = new List<string> { "Tile3" },
                AdSource = "MediaMotive"
            });

            var tagBuilders = new List<IMediaMotiveTagBuilder>()
            {
                new TestTagBuilder()
            };

            var queryHandler = new DisplayAdsQueryHandler(tagBuilders, tenantProvider);

            //Act
            var result = (MediaMotiveAdViewModel) queryHandler.Handle(new DisplayAdsQuery()
            {
                AdType = DisplayAdsTypes.Aside
            });

            Assert.AreEqual("//mm.carsales.com.au/carsales/jserver/make=honda/model=civic/make=bmw", result.ScriptUrl);
        }

        private class TestTagBuilder : IMediaMotiveTagBuilder
        {
            public IEnumerable<MediaMotiveTag> Build(MediaMotiveAdQuery query)
            {
                return new List<MediaMotiveTag>()
                {
                    new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, "honda"),
                    new MediaMotiveTag(SasAdTags.SasAdTagKeys.Model, "civic"),
                    new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, "honda"),
                    new MediaMotiveTag(SasAdTags.SasAdTagKeys.Make, "bmw")
                };
            }

            public bool IsApplicable(MediaMotiveAdQuery query)
            {
                return true;
            }
        }
    }
}
