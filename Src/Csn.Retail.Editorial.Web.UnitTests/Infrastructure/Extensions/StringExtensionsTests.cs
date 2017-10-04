using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Infrastructure.Extensions
{
    [TestFixture]
    class StringExtensionsTests
    {
        [Test]
        public void UrlFriendlyTest()
        {
            var urlFriendly = "Mike Sinclair".MakeUrlFriendly();

            Assert.AreEqual("mike-sinclair", urlFriendly);
        }
    }
}
