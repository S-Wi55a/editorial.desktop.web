using Csn.Retail.Editorial.Web.Features.Listings.Models;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    public class GetListingsResponse
    {
        public ListingsViewModel ListingsViewModel { get; set; }
        public bool RedirectRequired { get; set; }
        public string RedirectUrl { get; set; }
    }
}