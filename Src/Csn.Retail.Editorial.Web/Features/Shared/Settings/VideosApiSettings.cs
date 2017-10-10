using System;
using System.Configuration;

namespace Csn.Retail.Editorial.Web.Features.Shared.Settings
{
    public class VideosApiSettings : ConfigurationSection
    {
        private static readonly Lazy<VideosApiSettings> Settings = new Lazy<VideosApiSettings>(() => (ConfigurationManager.GetSection("VideosApiSettings") as VideosApiSettings));

        public static VideosApiSettings Instance => Settings.Value;

        [ConfigurationProperty("Url", IsRequired = true)]
        public string Url => this["Url"] as string;
    }
}