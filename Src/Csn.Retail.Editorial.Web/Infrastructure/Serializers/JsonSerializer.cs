using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Csn.Retail.Editorial.Web.Infrastructure.Serializers
{
    /*public interface ISerializer
    {
        string Serialize<T>(T value);
        T Deserialize<T>(string serializedContent);
    }

    [AutoBindAsSingleton]
    public class JsonSerializer : ISerializer
    {
        private readonly JsonSerializerSettings _settings;

        public JsonSerializer()
        {
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore,
                //Note: It is important to set typenamehandling and format as below to deserialize ryvus response
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
            };
        }

        public string Serialize<T>(T input)
        {
            return JsonConvert.SerializeObject(input, _settings);
        }

        public T Deserialize<T>(Stream content)
        {
            if (content == null) throw new ArgumentException(nameof(content));

            if (content.CanRead == false)
            {
                throw new ArgumentException("Stream must support reading", nameof(content));
            }

            using (var sr = new StreamReader(content))
            {
                using (var reader = new JsonTextReader(sr))
                {
                    var serializer = new Newtonsoft.Json.JsonSerializer
                    {
                        ContractResolver = _settings.ContractResolver,
                        NullValueHandling = _settings.NullValueHandling,
                        DateTimeZoneHandling = _settings.DateTimeZoneHandling,
                        TypeNameHandling = TypeNameHandling.Objects,
                        TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
                    };

                    foreach (var jsonConverter in _settings.Converters)
                    {
                        serializer.Converters.Add(jsonConverter);
                    }

                    return serializer.Deserialize<T>(reader);
                }
            }
        }

        public T Deserialize<T>(string serializedContent)
        {
            return JsonConvert.DeserializeObject<T>(serializedContent, _settings);
        }

        public string SupportedMediaType => RestClient.Constants.ContentTypes.Json;
    }*/
}