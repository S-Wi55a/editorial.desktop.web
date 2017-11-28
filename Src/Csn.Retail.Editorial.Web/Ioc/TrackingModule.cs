using Autofac;
using Csn.WebMetrics.Editorial.Interfaces;
using Csn.WebMetrics.Editorial.Ioc;
using Csn.WebMetrics.Ext.Interfaces;

namespace Csn.Retail.Editorial.Web.Ioc
{
    public class TrackingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => ObjectFactory.Instance.Resolve<IEditorialDetailsTrackingContainerProvider>()).As<IEditorialDetailsTrackingContainerProvider>();
            builder.Register(x => ObjectFactory.Instance.Resolve<IEditorialListingTrackingContainerProvider>()).As<IEditorialListingTrackingContainerProvider>();
            builder.Register(x => ObjectFactory.Instance.Resolve<IWebMetricsTrackingScriptBuilder>()).As<IWebMetricsTrackingScriptBuilder>();
        }
    }
}