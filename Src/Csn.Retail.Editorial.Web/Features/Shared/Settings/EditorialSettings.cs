using System;
using System.Configuration;

namespace Csn.Retail.Editorial.Web.Features.Shared.Settings
{
    public interface IEditorialSettings
    {
        string ImageServerUrlTemplate { get; }
        string DetailsUrlFormat { get; }
        string BasePath { get; }
        DetailsRouteVersion DetailsRouteVersion { get; }
        string DetailsRouteSegment { get; }
        string NetworkIdFormat { get; }
    }

    public class EditorialSettings : ConfigurationSection, IEditorialSettings
    {
        private static readonly Lazy<EditorialSettings> Settings = new Lazy<EditorialSettings>(() => (ConfigurationManager.GetSection("EditorialSettings") as EditorialSettings));

        public static EditorialSettings Instance => Settings.Value;        

        [ConfigurationProperty("ImageServerUrlTemplate", IsRequired = true)]
        public string ImageServerUrlTemplate => this["ImageServerUrlTemplate"] as string;

        [Obsolete("This property will be removed. We are in the process of moving to the new DetailsPageUrl property of editorial search results pages instead")]
        [ConfigurationProperty("DetailsUrlFormat", IsRequired = true)]
        public string DetailsUrlFormat => this["DetailsUrlFormat"] as string;

        [ConfigurationProperty("BasePath", IsRequired = true)]
        public string BasePath => this["BasePath"] as string;

        [ConfigurationProperty("DetailsRouteVersion", IsRequired = true)]
        public DetailsRouteVersion DetailsRouteVersion => (DetailsRouteVersion)Enum.Parse(typeof(DetailsRouteVersion), this["BasePath"].ToString());

        [ConfigurationProperty("DetailsRouteSegment", IsRequired = true)]
        public string DetailsRouteSegment => this["DetailsRouteSegment"] as string;

        [ConfigurationProperty("NetworkIdFormat", IsRequired = true)]
        public string NetworkIdFormat => this["NetworkIdFormat"] as string;
    }

    public enum DetailsRouteVersion
    {
        V1,
        V2
    }
}