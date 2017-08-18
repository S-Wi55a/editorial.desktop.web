using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Ingress.ServiceClient.Abstracts;
using Newtonsoft.Json.Linq;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi
{
    public interface IEditorialRyvussApiProxy
    {
        Task<SmartServiceResponse<RyvussResult>> GetAsync(EditorialRyvussApiInput input);
        Task<SmartServiceResponse<object>> GetAsyncProxy(EditorialRyvussApiInput input);
        Task<SmartServiceResponse<T>> GetAsync<T>(EditorialRyvussNavInput input);
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

        public Task<SmartServiceResponse<RyvussResult>> GetAsync(EditorialRyvussApiInput input)
        {
            return _smartClient.Service(ServiceName)
                .Path("/v4/editoriallistingretail")
                .QueryString("q", input.RyvussPredicates)
                .QueryString("count", "true")
                .QueryString("inav", "RetailNav|Retail|ShowZero")
                .QueryString("sr", $"|latest|{input.Offset}|{input.Limit}")
                .GetAsync<RyvussResult>();
        }

        public Task<SmartServiceResponse<object>> GetAsyncProxy(EditorialRyvussApiInput input)
        {
            return _smartClient.Service(ServiceName)
                .Path("/v4/EditorialListing")
                .QueryString("q", input.RyvussPredicates)
                .QueryString("count", "true")
                .QueryString("inav", "true")
                .QueryString("sr", "||10|")
                //.QueryString("sr", "|Latest|{0}|{1}".FormatWith(input.Offset, input.Limit))
                .GetAsync<object>();
        }

        public Task<SmartServiceResponse<T>> GetAsync<T>(EditorialRyvussNavInput input)
        {
            return _smartClient.Service(ServiceName)
                .Path("/v4/editoriallistingretail")
                .QueryString("q", input.Query)
                .QueryString("inav", "RetailNav|Retail|ShowZero")
                .QueryString("sr", $"|latest|{input.Offset}|{input.Limit}")
                .QueryString("count", "true")
                .GetAsync<T>();
        }
    }

    public class EditorialRyvussApiInput
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string RyvussPredicates { get; set; }
    }

    public class EditorialRyvussNavInput
    {
        public string Query { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }

    }

    public class RyvussResult
    {
        public string Count { get; set; }
        public RyvussNav INav { get; set; }
        public List<SearchResult> SearchResults { get; set; }
    }

    public class SearchResult
    {
        public string Id { get; set; }
        public string Headline { get; set; }
        public DateTime DateAvailable { get; set; }
        public string PhotoPath { get; set; }
    }

    public class RyvussNav
    {
        public List<RyvussNavNode> Nodes { get; set; }
        public List<BreadCrumbs> BreadCrumbs { get; set; }

    }

    public class BreadCrumbs
    {
        public string Aspect { get; set; }
        public string AspectDisplay { get; set; }
        public string Facet { get; set; }
        public string FacetDisplay { get; set; }
        public string RemoveAction { get; set; }
    }

    public class RyvussNavNode
    {
        public bool IsSelected { get; set; }
        public string PlaceholderExpression { get; set; }
        public string MultiSelectMode { get; set; }
        public string RemoveAction { get; set; }
        public List<FacetNode> Facets { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
    }

    public class FacetNode
    {
        public bool IsSelected { get; set; }
        public string Value { get; set; }
        public string DisplayValue { get; set; }
        public string Action { get; set; }
        public int Count { get; set; }
        public RyvussNav Refinements { get; set; }
        public string Expression { get; set; }
    }
}