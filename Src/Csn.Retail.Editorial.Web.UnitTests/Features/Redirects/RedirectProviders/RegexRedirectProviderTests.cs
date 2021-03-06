﻿using System;
using Csn.Retail.Editorial.Web.Features.Redirects;
using Csn.Retail.Editorial.Web.Features.Redirects.RedirectProviders;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Redirects.RedirectProviders
{
    class RegexRedirectProviderTests
    {
        [TestCase("https://www.carsales.com.au/editorial/news-article-type/", "/editorial/news/")]
        [TestCase("https://www.carsales.com.au/editorial/audi/review-article-type/suv-bodytype/", "/editorial/audi/review/suv-bodytype/")]
        public void TestArticleTypeRedirects(string requestUrl, string expectedResult)
        {
            var redirectProvider = new RegexRedirectProvider();

            var redirectRule = new RedirectRule()
            {
                MatchRule = "(.*?)\\/((.*?)-article-type)\\/(.*?)",
                RedirectInstruction = "$1/$3/$4",
                Name = "Test",
                RuleType = RedirectRuleType.Regex
            };

            Assert.AreEqual(expectedResult, redirectProvider.GetRedirectUrl(redirectRule, new Uri(requestUrl)));
        }

        [TestCase("https://www.carsales.com.au/editorial/news/2016/honda/test-news-12345/", "/editorial/details/test-news-12345/")]
        [TestCase("https://www.carsales.com.au/editorial/reviews/2015/honda/crv/2016-honda-crv-review-12345", "/editorial/details/2016-honda-crv-review-12345/")]
        public void TestAusLegacyDetailsPageRedirects(string requestUrl, string expectedResult)
        {
            var redirectProvider = new RegexRedirectProvider();

            var redirectRule = new RedirectRule()
            {
                MatchRule = "\\/editorial\\/(news|reviews|advice|videos|features|riding-advice|tips|tow-tests|motoracing|engine-reviews|products)\\/(.*)?\\/(.*-\\d+)\\/?",
                RedirectInstruction = "/editorial/details/$3/",
                Name = "Test",
                RuleType = RedirectRuleType.Regex
            };

            Assert.AreEqual(expectedResult, redirectProvider.GetRedirectUrl(redirectRule, new Uri(requestUrl)));
        }

        [TestCase("https://soloautos.mx/noticias/listado/actualidad/", "/noticias/actualidad/")]
        public void TestSoloautosListadosRedirects(string requestUrl, string expectedResult)
        {
            var redirectProvider = new RegexRedirectProvider();

            var redirectRule = new RedirectRule()
            {
                MatchRule = "\\/noticias\\/listado\\/(actualidad|pruebas|las-mejores-compras|compra-de-auto|consejos|comparativas)\\/?",
                RedirectInstruction = "/noticias/$1/",
                Name = "Test",
                RuleType = RedirectRuleType.Regex
            };

            Assert.AreEqual(expectedResult, redirectProvider.GetRedirectUrl(redirectRule, new Uri(requestUrl)));
        }
    }
}
