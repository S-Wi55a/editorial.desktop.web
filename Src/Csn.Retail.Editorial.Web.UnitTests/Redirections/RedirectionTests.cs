using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Details;
using Csn.Retail.Editorial.Web.Features.Listings;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Redirects;
using Expresso.Expressions;
using Expresso.Syntax;
using Ingress.ServiceClient.Abstracts;
using NSubstitute;
using NUnit.Framework;


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
            var slug = metadata.GetQueryParametersForSlug(keyword, offset, sortOrder);
            Assert.That(slug, Is.EqualTo(expectedResult));
        }
    }
}