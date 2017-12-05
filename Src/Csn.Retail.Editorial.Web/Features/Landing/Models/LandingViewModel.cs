﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;

namespace Csn.Retail.Editorial.Web.Features.Landing.Models
{
    public class LandingViewModel
    {
        public List<CarouselViewModel> Carousels { get; set; }
    }

    public class CarouselViewModel
    {
        public string Title { get; set; }
        public List<SearchResult> CategoryItems { get; set; }
        public string Link { get; set; }
        public bool HasMrec { get; set; }
    }
}