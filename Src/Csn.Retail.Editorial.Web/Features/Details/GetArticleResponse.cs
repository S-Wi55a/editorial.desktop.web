﻿using System.Net;
using Csn.Retail.Editorial.Web.Features.Details.Models;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class GetArticleResponse
    {
        public ArticleViewModel ArticleViewModel { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public string RedirectUrl { get; set; }
        public string DetailsPageUrlPath { get; set; }
    }
}