using System;
using System.Configuration;

namespace Csn.Retail.Editorial.Web.Features.Shared.Settings
{
    public interface IEditorialRouteSettings
    {
        string DetailsUrlFormat { get; }
        string BasePath { get; }
        string ResultsSegment { get; }
        string DetailsRouteSegment { get; }
        string NetworkIdFormat { get; }
    }

    public class EditorialRouteSettings : ConfigurationSection, IEditorialRouteSettings
    {
        private static readonly Lazy<EditorialRouteSettings> Settings = new Lazy<EditorialRouteSettings>(() => (ConfigurationManager.GetSection("EditorialRouteSettings") as EditorialRouteSettings));

        public static EditorialRouteSettings Instance => Settings.Value;        

        [Obsolete("This property will be removed. We are in the process of moving to the new DetailsPageUrl property of editorial search results pages instead")]
        [ConfigurationProperty("DetailsUrlFormat", IsRequired = true)]
        public string DetailsUrlFormat => this["DetailsUrlFormat"] as string;

        [ConfigurationProperty("BasePath", IsRequired = true)]
        public string BasePath => this["BasePath"] as string;

        [ConfigurationProperty("ResultsSegment", IsRequired = true)]
        public string ResultsSegment => this["ResultsSegment"] as string;

        [ConfigurationProperty("DetailsRouteSegment", IsRequired = true)]
        public string DetailsRouteSegment => this["DetailsRouteSegment"] as string;

        [ConfigurationProperty("NetworkIdFormat", IsRequired = true)]
        public string NetworkIdFormat => this["NetworkIdFormat"] as string;
    }
}