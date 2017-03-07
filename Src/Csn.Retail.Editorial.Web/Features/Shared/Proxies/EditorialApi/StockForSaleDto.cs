using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public class StockForSaleDto
    {
        public List<StockListingItem> Items { get; set; }

        public string ListingPageUrl { get; set; }
    }

    public class StockListingItem
    {
        public string Title { get; set; }
        public List<string> Attributes { get; set; }
        public string NetworkId { get; set; }
        public string PhotoUrl { get; set; }
        public string Price { get; set; }
        public string Location { get; set; }
        public string DetailsPageUrl { get; set; }
    }
}

