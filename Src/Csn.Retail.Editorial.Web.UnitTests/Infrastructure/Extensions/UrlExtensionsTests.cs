using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Infrastructure.Extensions
{
    class UrlExtensionsTests
    {
        [Test]
        public void QueryParamsWithSpecialChars()
        {
            var queryParams = new Dictionary<string, string>()
            {
                { "q", "(And.Service.CarSales._.Make.Alfa Romeo.)" },
                { "sortOrder", "Latest" }
            };

            Assert.AreEqual("q=%28And.Service.CarSales._.Make.Alfa%20Romeo.%29&sortOrder=Latest", queryParams.ToQueryString());
        }
    }
}
