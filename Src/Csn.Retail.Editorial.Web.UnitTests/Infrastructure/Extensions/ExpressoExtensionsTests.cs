using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Expresso.Parser;
using Expresso.Sanitisation;
using Expresso.Syntax;
using Expresso.Syntax.Rose;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Infrastructure.Extensions
{
    /// <summary>
    /// Test for <seealso cref="ExpressoExtensions"/>
    /// </summary>
    [TestFixture]
    public class ExpressoExtensionsTests
    {
        private IExpressionParser _parser;
        private IExpressionFormatter _expressionFormatter;
        

        [SetUp]
        public void Setup()
        {
            _parser = new RoseTreeParser(new RoseTreeSanitiser());
            _expressionFormatter = new RoseTreeFormatter(new RoseTreeSanitiser());
        }

        [Test]
        public void KeywordAppendTest()
        {
            //Arrange
            var query = "Service.CarSales.";
            var expression = _parser.Parse(query);

            //Act
            var updatedQuery = expression.AppendOrUpdateKeywords("honda");

            //Assert
            Assert.AreNotEqual(updatedQuery, string.Empty);
            Assert.AreEqual(_expressionFormatter.Format(updatedQuery).Contains("keyword(honda)"), true);
        }

        [Test]
        public void KeywordRemovedTest()
        {
            //Arrange
            var query = "(And.Service.CarSales._.Keywords.keyword(honda).)";
            var expression = _parser.Parse(query);

            //Act
            var updatedQuery = expression.AppendOrUpdateKeywords(string.Empty);

            //Assert
            Assert.AreNotEqual(updatedQuery, string.Empty);
            Assert.AreEqual(_expressionFormatter.Format(updatedQuery).Contains("keyword(honda)"), false);
            Assert.AreEqual(_expressionFormatter.Format(updatedQuery).Contains("keyword"), false);
        }

        [Test]
        public void KeywordUpdatedTest()
        {
            //Arrange
            var query = "(And.Service.CarSales._.Keywords.keyword(honda).)";
            var expression = _parser.Parse(query);

            //Act
            var updatedQuery = expression.AppendOrUpdateKeywords("BMW");

            //Assert
            Assert.AreNotEqual(updatedQuery, string.Empty);
            Assert.AreEqual(_expressionFormatter.Format(updatedQuery).Contains("keyword(BMW)"), true);
            Assert.AreEqual(_expressionFormatter.Format(updatedQuery).Contains("keyword(honda)"), false);
        }

        [Test]
        public void GetKeywordsTest()
        {
            //Arrange
            var query = "(And.Service.CarSales._.Keywords.keyword(honda).)";
            var expression = _parser.Parse(query);

            //Act
            var results = expression.GetKeywords();

            //Assert
            Assert.AreEqual(results, "honda");
        }
    }
}