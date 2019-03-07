using Csn.Retail.Editorial.Web.Features.Shared.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Csn.Retail.Editorial.Web.Features.Shared.Settings
{
    public interface ISeoSchemaSettings
    {
        string ArticleTypesForReviewSchema { get; }
        string ArticleTypesForNewsSchema { get; }
        string LogoImageUrlPath { get; }
    }

    public class SeoSchemaSettings: ConfigurationSection, ISeoSchemaSettings
    {
        private static readonly Lazy<SeoSchemaSettings> Settings = new Lazy<SeoSchemaSettings>(() => (ConfigurationManager.GetSection("SeoSchemaSettings") as SeoSchemaSettings));
        public static SeoSchemaSettings Instance => Settings.Value;

        [ConfigurationProperty("ArticleTypesForReviewSchema", IsRequired = true)]
        public string ArticleTypesForReviewSchema => this["ArticleTypesForReviewSchema"] as string;
        
        [ConfigurationProperty("ArticleTypesForNewsSchema", IsRequired = true)]
        public string ArticleTypesForNewsSchema => this["ArticleTypesForNewsSchema"] as string;

        [ConfigurationProperty("LogoImageUrlPath", IsRequired = true)]
        public string LogoImageUrlPath => this["LogoImageUrlPath"] as string;
    }
}