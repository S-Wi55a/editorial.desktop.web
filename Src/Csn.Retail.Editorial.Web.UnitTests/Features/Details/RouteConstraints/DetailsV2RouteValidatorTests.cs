using Csn.Retail.Editorial.Web.Features.Details.RouteConstraints;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Details.RouteConstraints
{
    [TestFixture]
    class DetailsV2RouteValidatorTests
    {
        [Test]
        public void AustralianV2Route()
        {
            var settings = Substitute.For<IEditorialRouteSettings>();

            settings.DetailsRouteSegment.Returns("details");
            settings.NetworkIdFormat.Returns("ED-ITM-{0}");

            var validator = new DetailsV2RouteValidator(settings);

            var result = validator.IsValid("/details/this-is-a-test/ED-ITM-1234/");

            Assert.IsTrue(result);
        }

        [Test]
        public void LatamV2Route()
        {
            var settings = Substitute.For<IEditorialRouteSettings>();

            settings.DetailsRouteSegment.Returns("detalle");
            settings.NetworkIdFormat.Returns("ED-LATAM-{0}");

            var validator = new DetailsV2RouteValidator(settings);

            var result = validator.IsValid("/detalle/this-is-a-test/ED-LATAM-1234/");

            Assert.IsTrue(result);
        }

        [Test]
        public void NotApplicableRoute()
        {
            var settings = Substitute.For<IEditorialRouteSettings>();

            settings.DetailsRouteSegment.Returns("detalle");
            settings.NetworkIdFormat.Returns("ED-LATAM-{0}");

            var validator = new DetailsV2RouteValidator(settings);

            var result = validator.IsValid("/detalle/this-is-a-test-1234/");

            Assert.IsFalse(result);
        }
    }
}
