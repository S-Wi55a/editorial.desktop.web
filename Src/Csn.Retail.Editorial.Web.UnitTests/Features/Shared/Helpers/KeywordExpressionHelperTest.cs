using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Expresso.Parser;
using Expresso.Sanitisation;
using Expresso.Syntax;
using Expresso.Syntax.Rose;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Shared.Helpers
{
    /// <summary>
    /// Test for <seealso cref="KeywordExpressionHelper"/>
    /// </summary>
    [TestFixture]
    public class KeywordExpressionHelperTest
    {
        private IExpressionParser _parser;
        private IExpressionFormatter _expressionFormatter;
        private KeywordExpressionHelper testSubject;

        [SetUp]
        public void Setup()
        {
            _parser = new RoseTreeParser(new RoseTreeSanitiser());
            _expressionFormatter = new RoseTreeFormatter(new RoseTreeSanitiser());
            testSubject = new KeywordExpressionHelper(_parser, _expressionFormatter);
        }

        [Test]
        public void KeywordAppendTest()
        {
            //Arrange
            var query = "Service.CarSales.";
            
            //Act
            var updatedQuery = testSubject.AppendOrUpdate(query, "honda");

            //Assert
            Assert.AreNotEqual(updatedQuery, string.Empty);
            Assert.AreEqual(updatedQuery.Contains("keyword(honda)"), true);
        }

        [Test]
        public void KeywordRemovedTest()
        {
            //Arrange
            var query = "(And.Service.CarSales._.Keywords.keyword(honda).)";

            //Act
            var updatedQuery = testSubject.AppendOrUpdate(query, string.Empty);

            //Assert
            Assert.AreNotEqual(updatedQuery, string.Empty);
            Assert.AreEqual(updatedQuery.Contains("keyword(honda)"), false);
        }

        [Test]
        public void KeywordUpdatedTest()
        {
            //Arrange
            var query = "(And.Service.CarSales._.Keywords.keyword(honda).)";

            //Act
            var updatedQuery = testSubject.AppendOrUpdate(query, "BMW");

            //Assert
            Assert.AreNotEqual(updatedQuery, string.Empty);
            Assert.AreEqual(updatedQuery.Contains("keyword(BMW)"), true);
            Assert.AreEqual(updatedQuery.Contains("keyword(honda)"), false);
        }
    }
}
