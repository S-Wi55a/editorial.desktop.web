using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Shared.Helpers
{
    /// <summary>
    /// <seealso cref="PaginationHelper"/>
    /// </summary>
    [TestFixture]
    public class PaginationHelperTest
    {
        
        [Test]
        public void GetPagingDataTest()
        {
            //Arrange
            var testSubject = new PaginationHelper();

            //Act
            var result = testSubject.GetPaginationData(314, 20, 120, "Sort", "test_query");


            //Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.First);
            Assert.IsNotNull(result.Last);
            Assert.AreEqual(result.CurrentPageNo, 7);

            //Assert previous page is valid
            Assert.IsNotNull(result.Previous);            
            Assert.AreEqual(result.Previous.PageNo, 6);
            Assert.AreEqual(result.Previous.Skip, 100);
            Assert.AreEqual(result.Previous.Query, "test_query");

            //Assert last page is valid
            Assert.IsNotNull(result.Next);
            Assert.AreEqual(result.Next.PageNo, 8);
            Assert.AreEqual(result.Next.Skip, 140);
            Assert.AreEqual(result.Next.Query, "test_query");
            
            //Total Page count is valid
            Assert.AreEqual(result.TotalPageCount, 16);
        }

        [Test]
        public void GetEdgeCasesPagingDataTest()
        {
            //Arrange
            var testSubject = new PaginationHelper();

            //Act
            var result = testSubject.GetPaginationData(0, 20, 0, "Sort", "test_query");

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
