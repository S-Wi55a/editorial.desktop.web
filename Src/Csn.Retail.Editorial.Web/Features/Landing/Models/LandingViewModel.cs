using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;

namespace Csn.Retail.Editorial.Web.Features.Landing.Models
{
    public class LandingViewModel
    {
        public List<CarouselViewModel> Carousels { get; set; }
        public Nav Nav { get; set; }
    }

    public class CarouselViewModel
    {
        public string Title { get; set; }
        public List<SearchResult> CarouselItems { get; set; }
        public string ViewAllLink { get; set; }
        public bool HasMrec { get; set; }
        public string NextQuery { get; set; }
    }

    public class Nav
    { 
        public NavResult NavResults { get; set; }
    }
}