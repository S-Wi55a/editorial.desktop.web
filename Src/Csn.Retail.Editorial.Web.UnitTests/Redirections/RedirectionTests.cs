using Csn.Retail.Editorial.Web.Features.Details;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Infrastructure.Redirects;
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
    public class RedirectionTests
    {
        private ISmartServiceClient _client;
        private IEditorialRyvussApiProxy _ryvussProxy;
        private IDetailsRedirectLogger _logger;
        private IExpressionFormatter _expressionFormatter;
        private LegacyUrlRedirectStrategy _legacyUrlRedirectStrategy;

        [SetUp]
        public void Setup()
        {
            _ryvussProxy = Substitute.For<IEditorialRyvussApiProxy>();
            _client = Substitute.For<ISmartServiceClient>();
            _logger = Substitute.For<IDetailsRedirectLogger>();
            _expressionFormatter = Substitute.For<IExpressionFormatter>();

            _legacyUrlRedirectStrategy = new LegacyUrlRedirectStrategy(_logger, _ryvussProxy);
        }

        [TestCase(true, "abc", "30", "latest", "?offset=30&sortOrder=latest")]
        [TestCase(true, "abc", "", "", "")]
        [TestCase(true, "abc", "30", "", "?offset=30")]
        [TestCase(false, "abc", "30", "latest", "&offset=30&sortOrder=latest&keyword=abc")]
        [TestCase(false, "abc", "", "", "&keyword=abc")]
        [TestCase(false, "abc", "30", "", "&offset=30&keyword=abc")]
        [TestCase(false, "", "", "", "")]
        public void TestGetQueryParametersForSlug(bool isSeo, string keyword, string offset, string sortOrder,
            string expectedResult)
        {

            var metadata = new EditorialMetadataDto();
            var slug = metadata.GetQueryParameters(keyword, offset, sortOrder);
            Assert.That(slug, Is.EqualTo(expectedResult));
        }


       /* [TestCase("?q=(Service=[ConstructionSales]&(Make=[Caron]|Make=[Caterpillar]))&sort=Latest&SearchAction=Refinement", "?offset=30&sortOrder=latest")]
        public void IntegratedTest_LegacyUrlRedirectStrategy(string url, string expectedResult)
        {
            // Mock out the context to run the action filter.
            var request = new Mock<HttpRequestBase>();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.SetupGet(c => c.Request).Returns(request.Object);
            httpContext.Object.Request.QueryString.Set("q", url);

            var actionExecutedContext = new ActionExecutingContext
            {
                HttpContext = httpContext.Object
            };

            var redirection = _legacyUrlRedirectStrategy.GetRedirectionInstructions(actionExecutedContext);

        }*/

    }
}