using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Ingress.ServiceClient.Abstracts;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi
{
    public interface IEditorialRyvussApiProxy
    {
        Task<SmartServiceResponse<T>> GetAsync<T>(EditorialRyvussInput input);
        SmartServiceResponse<EditorialMetadataDto> GetMetadata(string query);
    }

    [AutoBind]
    public class EditorialRyvussApiProxy : IEditorialRyvussApiProxy
    {
        private const string ServiceName = "api-search-editorials";
        private readonly ISmartServiceClient _smartClient;

        public EditorialRyvussApiProxy(ISmartServiceClient smartClient)
        {
            _smartClient = smartClient;
        }

        public Task<SmartServiceResponse<T>> GetAsync<T>(EditorialRyvussInput input)
        {
            var client = _smartClient.Service(ServiceName)
                .Path("/v4/editoriallistingretail")
                .QueryString("q", input.Query);

            if (!string.IsNullOrEmpty(input.NavigationName))
            {
                var nav = input.PostProcessors == null || !input.PostProcessors.Any() ? input.NavigationName : string.Join("|", input.PostProcessors.Prepend(input.NavigationName));
                client = client.QueryString("inav", nav);
            }

            if (input.IncludeSearchResults)
            {
                client = client.QueryString("sr", $"|{input.SortOrder}|{input.Offset}|{input.Limit}");
            }

            if (input.IncludeCount)
            {
                client = client.QueryString("count", "true");
            }

            return client.GetAsync<T>();
        }


        public SmartServiceResponse<EditorialMetadataDto> GetMetadata(string query)
        {
            var response = _smartClient.Service(ServiceName)
                .Path("v4/editoriallistingretail")
                .QueryString("q", query)
                .QueryString("metadata", "")
                .Get<EditorialMetadataDto>();

            return response;
        }
    }

    public class EditorialMetadataDto
    {
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        // ReSharper disable once InconsistentNaming - needed for Ryvus response parsing
        public string query { get; set; }
        public string Seo { get; set; }
    }
    public class EditorialRyvussInput
    {
        public string Query { get; set; }
        public string NavigationName { get; set; }
        public List<string> PostProcessors { get; set; }
        public bool IncludeCount { get; set; }
        public bool IncludeSearchResults { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string SortOrder { get; set; }
    }
}