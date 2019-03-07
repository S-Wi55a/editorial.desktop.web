using Autofac;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Settings;

namespace Csn.Retail.Editorial.Web.Ioc
{
    public class SettingsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => EditorialRouteSettings.Instance).As<IEditorialRouteSettings>().SingleInstance();
            builder.Register(x => ImageServerSettings.Instance).As<IImageServerSettings>().SingleInstance();
            builder.Register(x => VideosApiSettings.Instance).As<VideosApiSettings>().SingleInstance();
            builder.Register(x => ReactNetSettings.Instance).As<ReactNetSettings>().SingleInstance();
            builder.Register(x => SeoSchemaSettings.Instance).As<ISeoSchemaSettings>().SingleInstance();
        }
    }
}