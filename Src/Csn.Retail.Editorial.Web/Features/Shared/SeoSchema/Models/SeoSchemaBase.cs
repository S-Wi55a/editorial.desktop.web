using Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Shared;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.Shared.SeoSchema.Models
{
    public class SeoSchemaBase
    {
        [JsonProperty(PropertyName = "@context")]
        public string Context { get; } = SchemaContext.ForSchemaOrg;
    }
}