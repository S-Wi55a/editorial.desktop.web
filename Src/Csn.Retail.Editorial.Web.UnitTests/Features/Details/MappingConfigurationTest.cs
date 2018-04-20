using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Details.Mappings;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Details
{
    [TestFixture]
    class MappingConfigurationTest
    {
        [Test]
        public void Test()
        {
            var heroMapper = Substitute.For<IHeroSectionMapper>();
            var seoDataMapper = Substitute.For<ISeoDataMapper>();
            var polarNativeAdsMapper = Substitute.For<IPolarNativeAdsDataMapper>();
            var useDropCaseMapper = Substitute.For<IUseDropCaseMapper>();
            var articleTypeLabelMapper = Substitute.For<IArticleTypeLabelMapper>();

            Mapper.Initialize(cfg => new MappingSetupTask(heroMapper, seoDataMapper, polarNativeAdsMapper, useDropCaseMapper, articleTypeLabelMapper).Run(cfg));
            Mapper.AssertConfigurationIsValid();
        }
    }
}
