using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class SerializerExtensions
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public static MvcHtmlString JsonSerialize(this HtmlHelper html, object value)
        {
            return MvcHtmlString.Create(JsonConvert.SerializeObject(value, Settings));
        }
    }
}