﻿using Autofac;
using Csn.Logging;
using Csn.Retail.Editorial.Web.Features.Details.Loggers;
using Csn.Retail.Editorial.Web.Features.Errors;
using Csn.Retail.Editorial.Web.Features.Listings.Loggers;
using Csn.Retail.Editorial.Web.Features.Redirects;
using Csn.Retail.Editorial.Web.Infrastructure.Filters;

namespace Csn.Retail.Editorial.Web.Ioc
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // set up separate logger for this class so we can log separately
            builder.RegisterType<UrlNotFoundLogger>().WithParameter((p, c) => p.ParameterType == typeof(ILogger), (p, c) =>
            {
                var loggerFactory = c.Resolve<ILoggerFactory>();
                return loggerFactory.For<UrlNotFoundLogger>();
            }).As<IUrlNotFoundLogger>().SingleInstance();

            builder.RegisterType<DetailsRedirectLogger>().WithParameter((p, c) => p.ParameterType == typeof(ILogger), (p, c) =>
            {
                var loggerFactory = c.Resolve<ILoggerFactory>();
                return loggerFactory.For<DetailsRedirectLogger>();
            }).As<IDetailsRedirectLogger>().SingleInstance();

            builder.RegisterType<DetailsRequestLogger>().WithParameter((p, c) => p.ParameterType == typeof(ILogger), (p, c) =>
            {
                var loggerFactory = c.Resolve<ILoggerFactory>();
                return loggerFactory.For<DetailsRequestLogger>();
            }).As<IDetailsRequestLogger>().SingleInstance();

            builder.RegisterType<SeoListingUrlRedirectLogger>().WithParameter((p, c) => p.ParameterType == typeof(ILogger), (p, c) =>
            {
                var loggerFactory = c.Resolve<ILoggerFactory>();
                return loggerFactory.For<SeoListingUrlRedirectLogger>();
            }).As<ISeoListingUrlRedirectLogger>().SingleInstance();

            builder.RegisterType<LegacyListingUrlRedirectLogger>().WithParameter((p, c) => p.ParameterType == typeof(ILogger), (p, c) =>
            {
                var loggerFactory = c.Resolve<ILoggerFactory>();
                return loggerFactory.For<LegacyListingUrlRedirectLogger>();
            }).As<ILegacyListingUrlRedirectLogger>().SingleInstance();

            builder.RegisterType<RedirectLogger>().WithParameter((p, c) => p.ParameterType == typeof(ILogger), (p, c) =>
            {
                var loggerFactory = c.Resolve<ILoggerFactory>();
                return loggerFactory.For<RedirectLogger>();
            }).As<IRedirectLogger>().SingleInstance();

            builder.RegisterType<UnknownTenantLogger>().WithParameter((p, c) => p.ParameterType == typeof(ILogger), (p, c) =>
            {
                var loggerFactory = c.Resolve<ILoggerFactory>();
                return loggerFactory.For<UnknownTenantLogger>();
            }).As<IUnknownTenantLogger>().SingleInstance();
        }
    }
}