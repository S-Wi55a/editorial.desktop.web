﻿using System;
using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Shared
{
    public class RyvussNavResultDto
    {
        public int Count { get; set; }
        public RyvussNavDto INav { get; set; }
        public List<SearchResultDto> SearchResults { get; set; }
    }

    public class RyvussNavDto
    {
        public List<RyvussNavNodeDto> Nodes { get; set; }
        public List<BreadCrumbDto> BreadCrumbs { get; set; }
    }

    public class RyvussNavNodeDto
    {
        public bool IsSelected { get; set; }
        public string MultiSelectMode { get; set; }
        public List<FacetNodeDto> Facets { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public RefinementsMetaDataDto MetaData { get; set; }
    }

    public class RefinementsMetaDataDto
    {
        public List<RefinementsNodeDto> Refinements { get; set; }
    }

    public class RefinementsNodeDto
    {
        public string DisplayName { get; set; }
        public List<FacetNodeDto> Facets { get; set; }
    }

    // Turn these into generic types so the metadata type can
    // be injected in
    public class FacetNodeDto
    {
        public bool IsSelected { get; set; }
        public string Value { get; set; }
        public string DisplayValue { get; set; }
        public string Action { get; set; }
        public int Count { get; set; }
        public string Expression { get; set; }
        public FacetNodeMetaDataDto MetaData { get; set; }
    }

    public class FacetNodeMetaDataDto
    {
        public List<bool> IsRefineable { get; set; }
        public List<Refinement> Refinement { get; set; }

        public List<RefinementsNodeDto> Refinements { get; set; }
    }

    public class BreadCrumbDto
    {
        public string Aspect { get; set; }
        public string AspectDisplay { get; set; }
        public string Facet { get; set; }
        public string FacetDisplay { get; set; }
        public string RemoveAction { get; set; }
    }

    public class SearchResultDto
    {
        public string Id { get; set; }
        public string Slug { get; set; }
        public string Headline { get; set; }
        public DateTime DateAvailable { get; set; }
        public string PhotoPath { get; set; }
    }
}