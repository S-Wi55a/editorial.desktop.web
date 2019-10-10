using System;
using System.Configuration;

namespace Csn.Retail.Editorial.Web.Features.Shared.Settings
{
    public interface IEditorialRouteSettings
    {
        string BasePath { get; }
        string ResultsSegment { get; }
    }

    public class EditorialRouteSettings : ConfigurationSection, IEditorialRouteSettings
    {
        private static readonly Lazy<EditorialRouteSettings> Settings = new Lazy<EditorialRouteSettings>(() => (ConfigurationManager.GetSection("EditorialRouteSettings") as EditorialRouteSettings));

        public static EditorialRouteSettings Instance => Settings.Value;        

        [ConfigurationProperty("BasePath", IsRequired = true)]
        public string BasePath => this["BasePath"] as string;

        [ConfigurationProperty("ResultsSegment", IsRequired = true)]
        public string ResultsSegment => this["ResultsSegment"] as string;
    }
}