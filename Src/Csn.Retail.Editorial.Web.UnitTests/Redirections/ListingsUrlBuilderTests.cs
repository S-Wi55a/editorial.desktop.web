using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Redirections
{
    public class ListingsUrlBuilderTests
    {
        [TestCase(true, "q",  "abc", 30, "latest", "q?sort=latest&offset=30")]
        [TestCase(true, "q", "abc", 0, "", "q")]
        [TestCase(true, "q", "abc", 30, "", "q?offset=30")]
        [TestCase(false, "q", "abc", 30, "latest", "?q=q&sort=latest&keywords=abc&offset=30")]
        [TestCase(false, "q", "abc", 0, "", "?q=q&keywords=abc")]
        [TestCase(false, "q", "abc", 30, "", "?q=q&keywords=abc&offset=30")]
        [TestCase(false, "q",  "", 0, "", "?q=q")]
        public void TestGetQueryParametersForSlug(bool isSeo, string query, string keyword, long offset, string sortOrder,
            string expectedResult)
        {
            var metadata = new EditorialSeoDto();
            var url = isSeo
                ? ListingsUrlFormatter.GetSeoUrl(query, offset, sortOrder)
                : ListingsUrlFormatter.GetQueryString(query, offset, sortOrder, keyword);
            Assert.That(url, Is.EqualTo(expectedResult));
        }
    }
}