using System.Globalization;
using System.Web.Mvc;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Shared.Helpers
{
    /// <summary>
    /// <seealso cref="PaginationHelper"/>
    /// </summary>
    [TestFixture]
    public class PaginationHelperTest
    {
        private ITenantProvider<TenantInfo> _tenantProvider;

        [SetUp]
        public void SetUp()
        {
            _tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();

            _tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "carsales",
                Culture = new CultureInfo("en-us")
            });

            var dependencyResolver = Substitute.For<IDependencyResolver>();
            dependencyResolver.GetService<ITenantProvider<TenantInfo>>().Returns(_tenantProvider);

            DependencyResolver.SetResolver(dependencyResolver);
        }

        
        [Test]
        public void GetPagingDataTest()
        {
            //Arrange
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();
            var testSubject = new PaginationHelper(tenantProvider);

            tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "carsales",
                Culture = new CultureInfo("en-us")
            });

            //Act
            var result = testSubject.GetPaginationData(314, 20, 120, "Sort", "test_query", "keyword");


            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.First);
            Assert.IsNotNull(result.Last);
            Assert.AreEqual(result.CurrentPageNo, 7);

            //Assert previous page is valid
            Assert.IsNotNull(result.Previous);            
            Assert.AreEqual(result.Previous.PageNo, 6);
            Assert.AreEqual(result.Previous.Url.Contains("test_query"), true);
            Assert.AreEqual(result.Previous.Url.Contains("keyword"), true);

            //Assert last page is valid
            Assert.IsNotNull(result.Next);
            Assert.AreEqual(result.Next.PageNo, 8);
            Assert.AreEqual(result.Next.Url.Contains("test_query"), true);
            Assert.AreEqual(result.Next.Url.Contains("keyword"), true);

            //Total Page count is valid
            Assert.AreEqual(result.TotalPageCount, 16);
        }

        [Test]
        public void GetEdgeCasesPagingDataTest()
        {
            //Arrange
            var tenantProvider = Substitute.For<ITenantProvider<TenantInfo>>();
            var testSubject = new PaginationHelper(tenantProvider);
            tenantProvider.Current().Returns(new TenantInfo()
            {
                Name = "carsales",
                Culture = new CultureInfo("en-us")
            });

            //Act
            var result = testSubject.GetPaginationData(0, 20, 0, "Sort", "test_query", "keyword");

            //Assert
            Assert.IsNotNull(result);
            Assert.IsNull(result.First);
            Assert.IsNull(result.Last);

            //Assert previous/next pages are empty
            Assert.IsNull(result.Previous);
            Assert.IsNull(result.Next);

            Assert.AreEqual(result.TotalPageCount, 0);
            Assert.AreEqual(result.CurrentPageNo, 0);
        }
    }
}
