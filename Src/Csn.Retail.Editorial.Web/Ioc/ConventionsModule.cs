using Autofac;
using System;
using System.Linq;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Ioc
{
    public class ConventionsModule : Module
    {
        private const string AssemblyStartsWithMatch = "Csn.Retail.Editorial.";

        protected override void Load(ContainerBuilder builder)
        {
            var assembliesToScan = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith(AssemblyStartsWithMatch)).ToArray();

            builder.RegisterAssemblyTypes(assembliesToScan)
                .Where(t => t.CustomAttributes.Any(x => x.AttributeType == typeof(AutoBindSelfAttribute)))
                .AsSelf();

            builder.RegisterAssemblyTypes(assembliesToScan)
                .Where(t => t.CustomAttributes.Any(x => x.AttributeType == typeof(AutoBindSelfAsSingletonAttribute)))
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(assembliesToScan)
                .Where(t => t.CustomAttributes.Any(x => x.AttributeType == typeof(AutoBindAsSingletonAttribute)))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(assembliesToScan)
                .Where(t => t.CustomAttributes.Any(x => x.AttributeType == typeof(AutoBindAttribute)))
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(assembliesToScan)
                .Where(t => t.CustomAttributes.Any(x => x.AttributeType == typeof(AutoBindAsPerRequestAttribute)))
                .AsImplementedInterfaces()
                .InstancePerRequest();
        }
    }
}