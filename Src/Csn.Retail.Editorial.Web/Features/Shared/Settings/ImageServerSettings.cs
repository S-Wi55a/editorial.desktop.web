using System;
using System.Configuration;

namespace Csn.Retail.Editorial.Web.Features.Shared.Settings
{
    public interface IImageServerSettings
    {
        string ImageServerUrlTemplate { get; }
    }

    public class ImageServerSettings : ConfigurationSection, IImageServerSettings
    {
        private static readonly Lazy<ImageServerSettings> Settings = new Lazy<ImageServerSettings>(() => (ConfigurationManager.GetSection("ImageServerSettings") as ImageServerSettings));

        public static ImageServerSettings Instance => Settings.Value;

        [ConfigurationProperty("ImageServerUrlTemplate", IsRequired = true)]
        public string ImageServerUrlTemplate => this["ImageServerUrlTemplate"] as string;
    }
}