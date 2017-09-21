using System;
using System.Configuration;

namespace Csn.Retail.Editorial.Web.Features.Shared.Settings
{
    public class EditorialSettings : ConfigurationSection
    {
        private static readonly Lazy<EditorialSettings> Settings = new Lazy<EditorialSettings>(() => (ConfigurationManager.GetSection("EditorialSettings") as EditorialSettings));

        public static EditorialSettings Instance => Settings.Value;        

        [ConfigurationProperty("ImageServerUrlTemplate", IsRequired = true)]
        public string ImageServerUrlTemplate => this["ImageServerUrlTemplate"] as string;
    }
}