using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Details.Models;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class GetArticleResponse
    {
        public ArticleViewModel ArticleViewModel { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
    }
}