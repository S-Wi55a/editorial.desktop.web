using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Nav
{
    public class NavResult
    {
        public int Count { get; set; }
        public Nav INav { get; set; }
        public List<SearchResult> SearchResults { get; set; }
        public string NoResultsMessage { get; set; }
        public string NoResultsInstructionMessage { get; set; }

        //Should move to ListingViewModel
        public PagingViewModel Paging { get; set; } 
        public SortingViewModel Sorting { get; set; }
        public string PendingQuery { get; set; }
        public string Keyword { get; set; }
    }

    public class Nav
    {
        public List<NavNode> Nodes { get; set; }
        public List<BreadCrumb> BreadCrumbs { get; set; }
    }

    public class NavNode
    {
        public string MultiSelectMode { get; set; }
        public List<FacetNode> Facets { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }

    public class FacetNode
    {
        public bool IsSelected { get; set; }
        public string Value { get; set; }
        public string DisplayValue { get; set; }
        public string Action { get; set; }
        public int Count { get; set; }
        public string Expression { get; set; }
        public bool IsRefineable { get; set; }
        public Refinement Refinement { get; set; }
        public NavNode Refinements { get; set; }
    }

    public class BreadCrumb
    {
        public string Aspect { get; set; }
        public string AspectDisplay { get; set; }
        public string Facet { get; set; }
        public string FacetDisplay { get; set; }
        public string RemoveAction { get; set; }
        public string Term { get; set; }
    }

    public class SearchResult
    {
        public string Headline { get; set; }
        public string ImageUrl { get; set; }
        public string DateAvailable { get; set; }
        public string ArticleDetailsUrl { get; set; }        
        public string Label { get; set; }

    }
}