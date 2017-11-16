using System.IO;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Listings.Mappings;
using Expresso.Expressions;
using Expresso.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Listings
{
    [TestFixture]
    public class ListingInsightsDataMapperTests
    {
        [Test]
        public void ListingInsightsDataTest()
        {
            //Arrange
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://www.carsales.com/editorial/results/?q=&sort=Latest", ""), new HttpResponse(new StringWriter()));
            var expressionParser = Substitute.For<IExpressionParser>();
            var expression = new FacetExpression("Service", "Carsales").And(new FacetExpression("Type", "News"));
            expressionParser.Parse(Arg.Any<string>()).Returns(expression);
            var testSubject = new ListingInsightsDataMapper(expressionParser);

            //Act
            var result = testSubject.Map("(And.Service.CarSales._.Type.News.)", "Latest");

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, 6);
            Assert.AreEqual(result["type"], "news");
            Assert.AreEqual(result["service"], "carsales");
            Assert.AreEqual(result["contentgroup1"], "news and reviews");
            Assert.AreEqual(result["contentgroup2"], "listing");
            Assert.AreEqual(result["action"], "search");
            Assert.AreEqual(result["sortby"], "Latest");
        }
    }
}
