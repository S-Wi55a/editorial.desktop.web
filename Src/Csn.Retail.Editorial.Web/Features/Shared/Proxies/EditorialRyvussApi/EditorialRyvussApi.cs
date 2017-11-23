using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Ingress.ServiceClient.Abstracts;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi
{
    public interface IEditorialRyvussApiProxy
    {
        Task<SmartServiceResponse<T>> GetAsync<T>(EditorialRyvussInput input);
        SmartServiceResponse<T> Get<T>(EditorialRyvussInput input);
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
            return _smartClient.Service(ServiceName)
                .Path(GetPath(input))
                .QueryString(GetQueryParams(input))
                .GetAsync<T>();
        }

        //ToDo - Input from string to EditorialRyvussInput
        public SmartServiceResponse<T> Get<T>(EditorialRyvussInput input)
        {
            return _smartClient.Service(ServiceName)
                .Path(GetPath(input))
                .QueryString(GetQueryParams(input))
                .Get<T>();
        }

        private string GetPath(EditorialRyvussInput input)
        {
            var path = new StringBuilder("/v4");

            if (!string.IsNullOrEmpty(input.ControllerName))
            {
                path.Append($"/{input.ControllerName}");
            }

            path.Append("/editoriallistingretail");

            if (!string.IsNullOrEmpty(input.ServiceProjectionName))
            {
                path.Append($"/{input.ServiceProjectionName}");
            }

            return path.ToString();
        }

        private Dictionary<string, string> GetQueryParams(EditorialRyvussInput input)
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(input.Query))
            {
                queryParams.Add("q", input.Query);
            }

            if (input.IncludeCount)
            {
                queryParams.Add("count", "true");
            }

            if (input.IncludeSearchResults)
            {
                queryParams.Add("sr", $"|{input.SortOrder}|{input.Offset}|{input.Limit}");
            }

            if (!string.IsNullOrEmpty(input.NavigationName))
            {
                var nav = input.PostProcessors == null || !input.PostProcessors.Any()
                    ? input.NavigationName
                    : string.Join("|", input.PostProcessors.Prepend(input.NavigationName));
                queryParams.Add("inav", nav);
            }

            if (input.IncludeMetaData)
            {
                queryParams.Add("metadata", "");
            }

            return queryParams;
        }
    }

   

    public class EditorialRyvussInput
    {
        public string Query { get; set; }
        public string NavigationName { get; set; }
        public string ControllerName { get; set; }
        public string ServiceProjectionName { get; set; }

        public List<string> PostProcessors { get; set; }
        public bool IncludeCount { get; set; }
        public bool IncludeSearchResults { get; set; }
        public bool IncludeMetaData { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string SortOrder { get; set; }
    }
}