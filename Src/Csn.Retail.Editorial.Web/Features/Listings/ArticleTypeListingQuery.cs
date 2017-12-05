﻿using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    public class ArticleTypeListingQuery : IQuery
    {
        public ArticleType ArticleType { get; set; }
    }
}