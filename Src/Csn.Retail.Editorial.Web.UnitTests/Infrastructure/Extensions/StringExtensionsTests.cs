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
            Assert.AreEqual("mike-sinclair", "Mike Sinclair".MakeUrlFriendly());
        }

        [Test]
        public void LowercaseAlphaNumericOnlyTest()
        {
            Assert.AreEqual("alpharomeo", "Alpha Romeo".ToLowerAlphaNumericOnly());
        }

        [Test]
        public void AlphaNumericOnlyTest()
        {
            Assert.AreEqual("AlphaRomeo", "Alpha Romeo".AlphaNumericOnly());
        }
    }
}