using System;
using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Bolt.Common.Extensions;
using Csn.Logging;

namespace Csn.Retail.Editorial.Web.Infrastructure.AssetMappers
{
    public interface IAssetMapProvider
    {
        void Init();
        string Css(string name);
        string Js(string name);
    }

    [AutoBindAsSingleton]
    public class AssetMapProvider : IAssetMapProvider
    {
        private readonly ILogger _logger;
        private const string KeyAssetDomain = "AssetDomain";
        private readonly Lazy<IDictionary<string, AssetData>> source;

        public AssetMapProvider(IAssetDataReader assetDataReader, ILogger logger)
        {
            _logger = logger;
            source = new Lazy<IDictionary<string, AssetData>>(() => Load(assetDataReader));
        }

        private IDictionary<string, AssetData> Load(IAssetDataReader reader)
        {
            var basePath = System.Configuration.ConfigurationManager.AppSettings[KeyAssetDomain]?.TrimEnd('/') ?? string.Empty;
            var result = reader.Read();

            if (basePath.IsEmpty()) return result;

            foreach (var asset in result.Select(item => item.Value))
            {
                asset.Js = FixPathForDomain(asset.Js, basePath);
                asset.Css = FixPathForDomain(asset.Css, basePath);
            }

            return result;
        }

        public void Init()
        {
            // just fire the load of data
            var d = source.Value;
        }

        public string Css(string name)
        {
            var css = source.Value.GetValueOrDefault(name)?.Css;

            if (string.IsNullOrEmpty(css))
            {
                _logger.Error("Unable to load css file for {0}", name);
            }

            return source.Value.GetValueOrDefault(name)?.Css;
        }

        public string Js(string name)
        {
            var js = source.Value.GetValueOrDefault(name)?.Js;

            if (string.IsNullOrEmpty(js))
            {
                _logger.Error("Unable to load js file for {0}", name);
            }

            return source.Value.GetValueOrDefault(name)?.Js;
        }

        private string FixPathForDomain(string path, string basePath)
        {
            if (path.IsEmpty()) return string.Empty;

            path = path.TrimStart('/');
            var sepIndex = path.IndexOf('/');

            return sepIndex == -1 ? $"{basePath}/{path}" : $"{basePath}/{path.Substring(sepIndex + 1)}";
        }
    }
}
