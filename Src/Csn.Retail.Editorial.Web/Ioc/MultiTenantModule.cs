using System;
using System.Linq;
using System.Web;
using Autofac;
using Bolt.Common.Extensions;
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

            builder.Register(x => TenantProviderBuilder<TenantInfo>.New().WithCurrentTenantNamePicker(new TenantNamePicker())
                    .WithTenantDataMapper(x.Resolve<ITenantDataMapper<TenantInfo>>()).WithCurrentTenantNamePicker(new TenantNamePicker())
                    .WithLogger(new NullLogger())
                    .Build())
                .As<ITenantProvider<TenantInfo>>()
                .SingleInstance();

            builder.Register(x =>
                MultiTenantViewEngineBuilder<TenantInfo>.New()
                    .TenantProvider( x.Resolve<ITenantProvider<TenantInfo>>())
                    .Build())
                .As<MultiTenantViewEngine<TenantInfo>>().SingleInstance();

            builder.RegisterType<TenantDataMapper<TenantInfo>>().As<ITenantDataMapper<TenantInfo>>().SingleInstance();
        }
    }

    public class TenantNamePicker : ICurrentTenantNamePicker
    {
        public string Get()
        {
            var host = HttpContext.Current.Request.Url.Host.Replace("www.", string.Empty).Split('.').FirstOrDefault();

            if (!host.IsSame("redbook")) return host;
            var vertical = HttpContext.Current.Request.Url.AbsolutePath.Split(new[] { "/editorial" }, StringSplitOptions.None)[1].Split('/').FirstOrDefault();

            return Enum.TryParse<Vertical>(vertical, true, out var redbookVertical) ? $"redbook-{redbookVertical.ToString().ToLower()}" : "redbook-cars";
        }
    }

}