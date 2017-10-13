using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;

namespace Csn.Retail.Editorial.Web.Features.Listings.Models
{
    public class ListingsViewModel
    {
        public NavResult NavResults { get; set; }
        
        // Pagin/Sorting should be part of this model rather than NavResult - fix it with Redux stores updates
    }
}