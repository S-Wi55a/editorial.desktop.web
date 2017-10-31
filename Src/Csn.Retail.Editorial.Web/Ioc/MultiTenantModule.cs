using Autofac;
using Csn.MultiTenant;
using Csn.MultiTenant.Mvc5;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.MultiTenant;

namespace Csn.Retail.Editorial.Web.Ioc
{
    public class MultiTenantModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NullLogger>().As<ILogger>();

            builder.Register(x => TenantProviderBuilder<TenantInfo>.New()
                    .WithTenantDataMapper(x.Resolve<ITenantDataMapper<TenantInfo>>())
                    .WithLogger(new NullLogger())
                    .Build())
                .As<ITenantProvider<TenantInfo>>()
                .SingleInstance();

            builder.Register(x =>
                MultiTenantViewEngineBuilder<TenantInfo>.New()
                    .TenantProvider(x.Resolve<ITenantProvider<TenantInfo>>())
                    .Build())
                .As<MultiTenantViewEngine<TenantInfo>>().SingleInstance();

            builder.RegisterType<TenantDataMapper<TenantInfo>>().As<ITenantDataMapper<TenantInfo>>().SingleInstance();
        }
    }
}