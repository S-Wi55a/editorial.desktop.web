using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Redirections
{
    public class ListingsUrlBuilderTests
    {
        [TestCase(true, "/query/",  "abc", 30, "latest", "/editorial/beta-results/query/?sort=latest&offset=30")]
        [TestCase(true, "/query/", "abc", 0, "", "/editorial/beta-results/query/")]
        [TestCase(true, "/query/", "abc", 30, "", "/editorial/beta-results/query/?offset=30")]
        [TestCase(false, "query", "abc", 30, "latest", "/editorial/beta-results/?q=query&sort=latest&keywords=abc&offset=30")]
        [TestCase(false, "query", "abc", 0, "", "/editorial/beta-results/?q=query&keywords=abc")]
        [TestCase(false, "query", "abc", 30, "", "/editorial/beta-results/?q=query&keywords=abc&offset=30")]
        [TestCase(false, "query",  "", 0, "", "/editorial/beta-results/?q=query")]
        public void TestGetQueryParametersForSlug(bool isSeo, string query, string keyword, long offset, string sortOrder,
            string expectedResult)
        {
            var metadata = new EditorialSeoDto();
            var url = isSeo
                ? ListingsUrlFormatter.GetSeoUrl(query, offset, sortOrder)
                : ListingsUrlFormatter.GetQueryString(query, offset, sortOrder, keyword);
            Assert.AreEqual(expectedResult, url);
        }
    }
}