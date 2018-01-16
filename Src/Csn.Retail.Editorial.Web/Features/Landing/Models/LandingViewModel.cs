﻿using System;
using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.HeroAdUnit.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.Landing.Models
{
    public class LandingViewModel
    {
        public List<CarouselViewModel> Carousels { get; set; }
        public Nav Nav { get; set; }
        public string Title { get; set; }
        public CampaignAdResult CampaignAd { get; set; }
        public PolarNativeAdsData PolarNativeAdsData { get; set; }
        public Dictionary<string, string> InsightsData { get; set; }
        public string HeroTitle { get; set; }
    }

    public class CarouselViewModel
    {
        public string Title { get; set; }
        public List<SearchResult> CarouselItems { get; set; }
        public string ViewAllLink { get; set; }
        public bool HasMrec { get; set; }
        public string NextQuery { get; set; }
        public CarouselTypes CarouselType { get; set; }
        public PolarAds PolarAds { get; set; }
    }

    public class Nav
    { 
        public NavResult NavResults { get; set; }
    }
}