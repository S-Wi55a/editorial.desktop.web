using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.LatestArticles
{
    public class LatestArticlesQuery : IQuery
    {
        public string Id { get; set; }
        public string Offset { get; set; }
        public string Limit { get; set; }
        public string ServiceName { get; set; }
    }
}