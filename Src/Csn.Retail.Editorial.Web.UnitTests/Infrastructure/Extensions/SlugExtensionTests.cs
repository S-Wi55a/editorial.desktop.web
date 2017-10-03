using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Infrastructure.Extensions
{
    public class SlugExtensionTests
    {
        [Test]
        public void GetSlugWhenNoSlugSpecifiedTest()
        {
            //Arrange
            var article = new SearchResultDto ()
            {
                Id = "ED-ITM-1234",
                Headline = "This is a test"
            };

            //Act
            var result = article.GetSlug();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(result, string.Empty);
            Assert.AreEqual("this-is-a-test-1234", result);
        }

        [Test]
        public void GetSlugWhenSlugSpecifiedTest()
        {
            //Arrange
            var article = new SearchResultDto()
            {
                Slug = "this-is-a-test-1234"
            };

            //Act
            var result = article.GetSlug();
            Assert.IsNotNull(result);
            Assert.AreNotEqual(result, string.Empty);
            Assert.AreEqual("this-is-a-test-1234", result);
        }
    }
}
