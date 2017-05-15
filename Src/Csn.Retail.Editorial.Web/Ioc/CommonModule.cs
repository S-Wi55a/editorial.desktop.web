using System.Web;
using Autofac;
using Csn.Cars.Cache.Builder;
using Csn.Logging;
using Csn.Logging.NLog3;
using Csn.Retail.Editorial.Web.Features.Details.Loggers;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.Serializers;
using Csn.Serializers.Json;
using Csn.SimpleCqrs;
using Csn.WebMetrics.Editorial.Interfaces;
using Csn.WebMetrics.Editorial.Ioc;
using Csn.WebMetrics.Ext.Interfaces;
using Ingress.Autofac;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Csn.Retail.Editorial.Web.Ioc
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ContextStore<>)).As(typeof(IContextStore<>)).InstancePerRequest();
            builder.RegisterType<AutoMappedMapper>().As<IMapper>().SingleInstance();
            builder.Register(x => GetLogger.For<MvcApplication>()).As<ILogger>().SingleInstance();
            builder.RegisterType<NLogLoggerFactory>().As<ILoggerFactory>().SingleInstance();
            builder.Register(x => CacheStoreBuilder.New().Build()).As<Csn.Cars.Cache.ICacheStore>().SingleInstance();
            builder.RegisterType<Serializer>().As<ISerializer>().SingleInstance();
            builder.RegisterType<SettingsProvider>().As<ISettingsProvider>().SingleInstance();

            // Tracking
            builder.Register(x => ObjectFactory.Instance.Resolve<IEditorialDetailsTrackingContainerProvider>()).As<IEditorialDetailsTrackingContainerProvider>();
            builder.Register(x => ObjectFactory.Instance.Resolve<IWebMetricsTrackingScriptBuilder>()).As<IWebMetricsTrackingScriptBuilder>();

            builder.AddIngress(new IngressSetupOptions
            {
                AssembliesToScanAndRegister = new[]
                {
                    typeof(AppShellClient.AppShellClient).Assembly
                }
            });

            builder.Register(x => new HttpContextWrapper(HttpContext.Current)).As<HttpContextBase>();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                StringEscapeHandling = StringEscapeHandling.EscapeHtml

            };

            // set up separate logger for this class so we can log separately
            builder.RegisterType<ArticleNotFoundLogger>().WithParameter((p, c) => p.ParameterType == typeof(ILogger), (p, c) =>
            {
                var loggerFactory = c.Resolve<ILoggerFactory>();
                return loggerFactory.For<ArticleNotFoundLogger>();
            }).As<IArticleNotFoundLogger>().SingleInstance();
        }
    }

    public class GenericEventHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(LoadGlobalSiteDataOnPageLoad<>)).As(typeof(IAsyncEventHandler<>));
            builder.RegisterGeneric(typeof(LoadGoogleAnalyticsTrackingScriptOnPageLoad<>)).As(typeof(IAsyncEventHandler<>));
        }
    }
}