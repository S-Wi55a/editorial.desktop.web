﻿using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Details.Mappings;
using NSubstitute;
using NUnit.Framework;

namespace Csn.Retail.Editorial.Web.UnitTests.Features.Details
{
    [TestFixture]
    class MappingConfigurationTest
    {
        [Test]
        public void Test()
        {
            var heroMapper = Substitute.For<IHeroSectionMapper>();

            Mapper.Initialize(cfg => new MappingSetupTask(heroMapper).Run(cfg));
            Mapper.AssertConfigurationIsValid();
        }
    }
}
