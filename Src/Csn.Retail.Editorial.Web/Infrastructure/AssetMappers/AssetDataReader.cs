using System;
using System.Collections.Generic;
using System.IO;
using Csn.Logging;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Serializers;

namespace Csn.Retail.Editorial.Web.Infrastructure.AssetMappers
{
    public interface IAssetDataReader
    {
        IDictionary<string, AssetData> Read();
    }

    [AutoBind]
    public class AssetDataReader : IAssetDataReader
    {
        private readonly ISerializer serializer;
        private readonly ILogger logger;

        public AssetDataReader(ISerializer serializer, ILogger logger)
        {
            this.serializer = serializer;
            this.logger = logger;
        }

        public IDictionary<string, AssetData> Read()
        {
            try
            {
                var content = File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}/webpack.assets.json");
                return serializer.Deserialize<IDictionary<string, AssetData>>(content);
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
            }

            return new Dictionary<string, AssetData>();
        }
    }
}