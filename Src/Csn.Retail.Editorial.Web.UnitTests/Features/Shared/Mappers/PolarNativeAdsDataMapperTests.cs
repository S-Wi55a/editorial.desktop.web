using System.Collections.Generic;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Mappers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Shared.Mappers
{
    [TestFixture]
    class PolarNativeAdsDataMapperTests
    {
        [Test]
        public void TagsWithMakeModel()
        {
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();

            tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "carsales",
                DisplayPolarAds = true
            });

            var mapper = new PolarNativeAdsDataMapper(tenantProvider);

            var breadCrumbs = new List<BreadCrumbDto>()
            {
                new BreadCrumbDto()
                {
                    Aspect = "Make",
                    Facet = "Mazda",
                    Type = "FacetBreadCrumb",
                    Children = new List<BreadCrumbDto>()
                    {
                        new BreadCrumbDto()
                        {
                            Aspect = "Model",
                            Facet = "CX-5",
                            Type = "FacetBreadCrumb"
                        }
                    }
                }
            };

            var result = mapper.Map(breadCrumbs, null);

            Assert.AreEqual("MazdaCX5", result.MakeModel);
        }

        [Test]
        public void DoNotDisplayPolarAdsWhenDisabled()
        {
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();

            tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "carsales",
                DisplayPolarAds = false
            });

            var mapper = new PolarNativeAdsDataMapper(tenantProvider);

            var breadCrumbs = new List<BreadCrumbDto>()
            {
                new BreadCrumbDto()
                {
                    Aspect = "Make",
                    Facet = "Honda",
                    Type = "FacetBreadCrumb"
                }
            };

            var result = mapper.Map(breadCrumbs, null);

            Assert.IsNull(result);
        }
    }
}
