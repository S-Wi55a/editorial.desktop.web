using System;
using System.Configuration;

namespace Csn.Retail.Editorial.Web.Infrastructure.Settings
{
    public class ReactNetSettings : ConfigurationSection
    {
        private static readonly Lazy<ReactNetSettings> Settings = new Lazy<ReactNetSettings>(() => (ConfigurationManager.GetSection("ReactNetSettings") as ReactNetSettings));

        public static ReactNetSettings Instance => Settings.Value;

        [ConfigurationProperty("ReuseJavaScriptEngines", IsRequired = false, DefaultValue = true)]
        public bool ReuseJavaScriptEngines => bool.Parse(this["ReuseJavaScriptEngines"].ToString());

        [ConfigurationProperty("UseDebugReact", IsRequired = false, DefaultValue = false)]
        public bool UseDebugReact => bool.Parse(this["UseDebugReact"].ToString());

        [ConfigurationProperty("StartEngines", IsRequired = false, DefaultValue = 25)]
        public int StartEngines => int.Parse(this["StartEngines"].ToString());

        [ConfigurationProperty("MaxEngines", IsRequired = false, DefaultValue = 50)]
        public int MaxEngines => int.Parse(this["MaxEngines"].ToString());
    }
}