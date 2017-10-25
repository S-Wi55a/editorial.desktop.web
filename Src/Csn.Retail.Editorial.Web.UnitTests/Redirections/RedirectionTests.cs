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
        
        [TestCase("Make.bmw.", RyvussSyntax.V4)]
        [TestCase("(And.Service.Bikesales._.(Or.Make.Kawasaki._.Make.Suzuki.))", RyvussSyntax.V4)]
        [TestCase("((Make=[Digga]&Service=[ConstructionSales])&Author=[Carene Chong])", RyvussSyntax.V2)]
        public void CheckIfBinarySyntaxConversionsWork(string url, RyvussSyntax syntax)
        {
            //Arrange
            var ryvussProxy = Substitute.For<IEditorialRyvussApiProxy>();
            var client = Substitute.For<ISmartServiceClient>();
            var logger = Substitute.For<IDetailsRedirectLogger>();
            var expressionFormatter = Substitute.For<IExpressionFormatter>();

            var legacyUrlRedirectStrategy = new LegacyUrlRedirectStrategy(client, logger);

            var binarySyntx = legacyUrlRedirectStrategy.GetRyvussSyntax(url);

            Assert.That(binarySyntx, Is.EqualTo(syntax));
        }
    }
}