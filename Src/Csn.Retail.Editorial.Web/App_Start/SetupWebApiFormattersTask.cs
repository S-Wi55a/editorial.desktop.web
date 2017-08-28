using System.Web.Http;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.StartUpTasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Csn.Retail.Editorial.Web.App_Start
{
    [AutoBind]
    public class SetupWebApiFormattersTask : IStartUpTask
    {
        public void Run()
        {
            var formatters = GlobalConfiguration.Configuration.Formatters;

            formatters.Remove(formatters.XmlFormatter);
            formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new JsonConverter[] { new StringEnumConverter() }
            };
        }
    }
}