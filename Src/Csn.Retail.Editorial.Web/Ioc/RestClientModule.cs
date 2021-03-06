﻿using System.Collections.Generic;
using System.Linq;
using Autofac;
using Csn.Hystrix.Abstractions;
using Csn.Hystrix.Builders;
using Csn.Hystrix.Reporters;
using Csn.Hystrix.RestClient;
using Csn.Hystrix.RestClient.Builders;
using Csn.Hystrix.RestClient.Impl;
using Csn.Logging;
using Csn.Logging.NLog3;
using Csn.RestClient;
using Csn.Retail.Editorial.Web.Infrastructure.HystrixRestClientUtils;

namespace Csn.Retail.Editorial.Web.Ioc
{
    public class RestClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => CircuitBreakerFactoryBuilder
                .New()
                .WithLoggerFactory(x.Resolve<ILoggerFactory>())
                .WithReporter(new LogCircuitReporter(x.Resolve<Csn.Logging.ILogger>()))
                .Build()).As<ICircuitBreakerFactory>().SingleInstance();

            builder.Register(
                x => new ConfigBasedHostSettingsProvider("HystrixRestClientSettings", x.Resolve<Csn.Logging.ILogger>()))
                .As<IHostSettingsProvider>()
                .SingleInstance();

            builder.Register(x => Csn.RestClient.FluentRestClientBuilder.New
                .WithSerializer(x.Resolve<Csn.Serializers.ISerializer>())
                .WithFilters(x.Resolve<IEnumerable<IRequestFilter>>())
                .WithLogger(x.Resolve<ILogger>()).Build())
                .As<IRestClient>()
                .SingleInstance();

            builder.RegisterType<HystrixRestRequestHeaderInterceptor>().As<IHystrixRestRequestInterceptor>();
            builder.Register(x => new HystrixRestReporter(GetLogger.For<HystrixRestReporter>())).As<IHystrixRestReporter>().SingleInstance();
            builder.Register(x => new ApiMonitorReporter(GetLogger.For<ApiMonitorReporter>())).As<IHystrixRestReporter>().SingleInstance();
            builder.Register(x => FluentHystrixRestClientFactoryBuilder
                .New()
                .WithInterceptors(x.Resolve<IEnumerable<IHystrixRestRequestInterceptor>>()?.ToArray() ?? new IHystrixRestRequestInterceptor[] { })
                .WithRestClient(x.Resolve<IRestClient>())
                .WithSerializer(x.Resolve<Csn.Serializers.ISerializer>())
                .WithSettingsProvider(x.Resolve<IHostSettingsProvider>())
                .WithCircuitBreaker(x.Resolve<ICircuitBreakerFactory>())
                .WithLoggerFactory(x.Resolve<ILoggerFactory>())
                .WithReporters(x.Resolve<IEnumerable<IHystrixRestReporter>>()?.ToArray())
                .Build()).As<IFluentHystrixRestClientFactory>().SingleInstance();
        }
    }


}