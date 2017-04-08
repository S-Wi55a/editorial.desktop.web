using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using HtmlAgilityPack;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Infrastructure.Extensions
{
    [TestFixture]
    class HtmlNodeExtensionsTests
    {
        [Test]
        public void ParagraphContainingNoChildElements()
        {
            var doc = new HtmlDocument();
            var content = "<p>This is a test.</p>";
            doc.LoadHtml(content);

            var result = doc.DocumentNode.FirstChild.GetFirstPararaphText();

            Assert.AreEqual("This is a test.", result);
        }

        [Test]
        public void ParagraphContainingLineBreak()
        {
            var doc = new HtmlDocument();
            var content = "<p>This is a test<br>Of things to come. With a <a href='http://wwww.theage.com.au'>small test</a></p>";
            doc.LoadHtml(content);

            var result = doc.DocumentNode.FirstChild.GetFirstPararaphText();

            Assert.AreEqual("This is a test", result);
        }

        [Test]
        public void ParagraphContainingAnchorFollowedByLineBreak()
        {
            var doc = new HtmlDocument();
            var content = "<p>With a <a href='http://wwww.theage.com.au'>small test</a>. This is a test<br>Of things to come.</p>";
            doc.LoadHtml(content);

            var result = doc.DocumentNode.FirstChild.GetFirstPararaphText();

            Assert.AreEqual("With a small test. This is a test", result);
        }
    }
}