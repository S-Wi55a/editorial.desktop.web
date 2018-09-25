using Csn.Retail.Editorial.Web.Features.Details.RouteConstraints;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Details.RouteConstraints
{
    [TestFixture]
    class DetailsV1RouteValidatorTests
    {
        [Test]
        public void AustralianV1Route()
        {
            var settings = Substitute.For<IEditorialSettings>();

            settings.DetailsRouteSegment.Returns("details");

            var validator = new DetailsV1RouteValidator(settings);

            var result = validator.IsValid("/details/this-is-a-test-1234/");

            Assert.IsTrue(result);
        }

        [Test]
        public void NotApplicableRoute()
        {
            var settings = Substitute.For<IEditorialSettings>();

            settings.DetailsRouteSegment.Returns("details");

            var validator = new DetailsV1RouteValidator(settings);

            var result = validator.IsValid("/results/this-is-a-test-1234/");

            Assert.IsFalse(result);
        }
    }
}
