using System.Globalization;
using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Redirections
{
    public class ListingsUrlBuilderTests
    {
        private IEditorialRouteSettings _routeSettings;
        private ITenantProvider<TenantInfo> _tenantProvider;

        [SetUp]
        public void SetUp()
        {
            _tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();
            _routeSettings = Substitute.For<IEditorialRouteSettings>();
            _routeSettings.BasePath.Returns("/editorial/");
            _routeSettings.ResultsSegment.Returns("results");
            _tenantProvider.Current().Returns(new TenantInfo
            {
                Name = "carsales"
            });

            var dependencyResolver = Substitute.For<IDependencyResolver>();
            dependencyResolver.GetService<ITenantProvider<TenantInfo>>().Returns(_tenantProvider);
            dependencyResolver.GetService<IEditorialRouteSettings>().Returns(_routeSettings);
            DependencyResolver.SetResolver(dependencyResolver);
        }

        [TestCase(true, "/query/",  "abc", 30, "latest", "/editorial/query/?sb=latest&pg=30")]
        [TestCase(true, "/query/", "abc", 0, "", "/editorial/query/")]
        [TestCase(true, "/query/", "abc", 30, "", "/editorial/query/?pg=30")]
        [TestCase(false, "query", "abc", 30, "latest", "/editorial/results/?q=query&sb=latest&keywords=abc&pg=30")]
        [TestCase(false, "query", "abc", 0, "", "/editorial/results/?q=query&keywords=abc")]
        [TestCase(false, "query", "abc", 30, "", "/editorial/results/?q=query&keywords=abc&pg=30")]
        [TestCase(false, "query",  "", 0, "", "/editorial/results/?q=query")]
        public void TestGetQueryParametersForSlug(bool isSeo, string query, string keyword, long offset, string sortOrder,
            string expectedResult)
        {
            var url = isSeo
                ? ListingUrlHelper.GetSeoUrl(query, offset, sortOrder)
                : ListingUrlHelper.GetPathAndQueryString(query, offset, sortOrder, keyword);
            Assert.AreEqual(expectedResult, url);
        }
    }
}