using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Listings.Models;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    public class GetListingsResponse
    {
        public ListingsViewModel ListingsViewModel { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}