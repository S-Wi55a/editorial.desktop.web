using Csn.Retail.Editorial.Web.Features.Details;
using Csn.Retail.Editorial.Web.Features.Shared.Formatters;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Expresso.Syntax;
using Ingress.ServiceClient.Abstracts;
using NSubstitute;
using NUnit.Framework;

/*using System.Web.Mvc;
using Moq;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using System.Web;
*/
namespace Csn.Retail.Editorial.Web.UnitTests.Redirections
{
    public class ListingsUrlBuilderTests
    {
        private ISmartServiceClient _client;
        private IEditorialRyvussApiProxy _ryvussProxy;
        private IDetailsRedirectLogger _logger;
        private IExpressionFormatter _expressionFormatter;
 
        [SetUp]
        public void Setup()
        {
            _ryvussProxy = Substitute.For<IEditorialRyvussApiProxy>();
            _client = Substitute.For<ISmartServiceClient>();
            _logger = Substitute.For<IDetailsRedirectLogger>();
            _expressionFormatter = Substitute.For<IExpressionFormatter>();
        }

        [TestCase(true, "q",  "abc", 30, "latest", "q?sortOrder=latest&offset=30")]
        [TestCase(true, "q", "abc", 0, "", "q")]
        [TestCase(true, "q", "abc", 30, "", "q?offset=30")]
        [TestCase(false, "q", "abc", 30, "latest", "?q=q&sortOrder=latest&keyword=abc&offset=30")]
        [TestCase(false, "q", "abc", 0, "", "?q=q&keyword=abc")]
        [TestCase(false, "q", "abc", 30, "", "?q=q&keyword=abc&offset=30")]
        [TestCase(false, "q",  "", 0, "", "?q=q")]
        public void TestGetQueryParametersForSlug(bool isSeo, string query, string keyword, long offset, string sortOrder,
            string expectedResult)
        {
            var metadata = new EditorialSeoDto();
            var url = isSeo
                ? ListingsUrlFormatter.GetQueryString(query, offset, sortOrder)
                : ListingsUrlFormatter.GetQueryString(query, offset, sortOrder, keyword);
            Assert.That(url, Is.EqualTo(expectedResult));
        }
    }
}