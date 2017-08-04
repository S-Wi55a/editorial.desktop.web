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
            var specDataMapper = Substitute.For<ISpecDataMapper>();
            var useDropCaseMapper = Substitute.For<IUseDropCaseMapper>();

            Mapper.Initialize(cfg => new MappingSetupTask(heroMapper, seoDataMapper, polarNativeAdsMapper, specDataMapper, useDropCaseMapper).Run(cfg));
            Mapper.AssertConfigurationIsValid();
        }
    }
}
