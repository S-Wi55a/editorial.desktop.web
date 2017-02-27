using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Csn.Cars.Cache.Builder;
using Csn.Logging;
using Csn.Retail.Editorial.Web.Features.Details;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.Serializers;
using Csn.Serializers.Json;
using Csn.SimpleCqrs;
using Ingress.Autofac;

namespace Csn.Retail.Editorial.Web.Ioc
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(ContextStore<>)).As(typeof(IContextStore<>)).InstancePerRequest();
            builder.RegisterType<AutoMappedMapper>().As<IMapper>().SingleInstance();
            builder.Register(x => GetLogger.For<MvcApplication>()).As<ILogger>().SingleInstance();
            builder.Register(x => CacheStoreBuilder.New().Build()).As<Csn.Cars.Cache.ICacheStore>().SingleInstance();
            builder.RegisterType<Serializer>().As<ISerializer>().SingleInstance();
            builder.RegisterType<SettingsProvider>().As<ISettingsProvider>().SingleInstance();

            //builder.RegisterType<ArticleIdentifierModelBinder>().AsModelBinderForTypes(typeof(ArticleIdentifier));
            builder.AddIngress();
        }
    }

    public class GenericEventHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(LoadGlobalSiteDataOnPageLoad<>)).As(typeof(IAsyncEventHandler<>));
        }
    }
}