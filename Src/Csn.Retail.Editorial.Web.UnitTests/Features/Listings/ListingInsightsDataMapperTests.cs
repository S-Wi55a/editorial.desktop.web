using System.Collections.Generic;
using System.IO;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Listings.Mappings;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
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

            var listingPageContext= new ListingPageContext()
            {
                Query = "(And.Service.CarSales._.Type.News.)",
                Sort = "Latest",
                RyvussNavResult = new RyvussNavResultDto { Count = 7 }
            };

            //Act
            var result = testSubject.Map(listingPageContext);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.MetaData.Count, 7);
            Assert.AreEqual(result.MetaData["type"], "news");
            Assert.AreEqual(result.MetaData["service"], "carsales");
            Assert.AreEqual(result.MetaData["contentgroup1"], "news and reviews");
            Assert.AreEqual(result.MetaData["contentgroup2"], "listing");
            Assert.AreEqual(result.MetaData["action"], "search");
            Assert.AreEqual(result.MetaData["sortby"], "Latest");
        }



        [Test]
        [TestCaseSource(nameof(TestCasesTags))]
        public void ListingInsightsForPostType(Expression exp1, Expression exp2, string query, string testValue)
        {
            //Arrange
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://www.carsales.com/editorial/results/?q=&sort=Latest", ""), new HttpResponse(new StringWriter()));
            var expressionParser = Substitute.For<IExpressionParser>();
            var expression = exp1.And(exp2);
            expressionParser.Parse(Arg.Any<string>()).Returns(expression);
            var testSubject = new ListingInsightsDataMapper(expressionParser);

            var listingPageContext = new ListingPageContext()
            {
                Query = query,
                Sort = "Latest",
                RyvussNavResult = new RyvussNavResultDto { Count = 7 }
            };

            //Act
            var result = testSubject.Map(listingPageContext);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.MetaData.Count, 7);
            Assert.AreEqual(result.MetaData["type"], testValue);
        }

        public static IEnumerable<object[]> TestCasesTags
        {
            get
            {
                yield return new object[] { new FacetExpression("Service", "CarSales"), new FacetExpression("PostType", "Car News"), "(And.Service.CarSales._.PostType.Car News.)", "news" };
                yield return new object[] { new FacetExpression("Service", "CarSales"), new FacetExpression("PostType", "Car Advice"), "(And.Service.CarSales._.PostType.Car Advice.)", "advice" };
                yield return new object[] { new FacetExpression("Service", "CarSales"), new FacetExpression("PostType", "Car Videos"), "(And.Service.CarSales._.PostType.Car Videos.)", "video" };
                yield return new object[] { new FacetExpression("Service", "CarSales"), new FacetExpression("PostType", "Car Reviews"), "(And.Service.CarSales._.PostType.Car Reviews.)", "review" };
                yield return new object[] { new FacetExpression("Service", "CarSales"), new FacetExpression("PostType", "Car Features"), "(And.Service.CarSales._.PostType.Car Features.)", "feature" };

                yield return new object[] { new FacetExpression("Service", "BoatSales"), new FacetExpression("Type", "News"), "(And.Service.BoatSales._.Type.News.)", "news" };
                yield return new object[] { new FacetExpression("Service", "BoatSales"), new FacetExpression("Type", "Review"), "(And.Service.BikeSales._.Type.Review.)", "review" };
            }
        }
    }
}
