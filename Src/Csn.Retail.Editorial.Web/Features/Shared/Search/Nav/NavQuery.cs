using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Nav
{
    public class NavQuery : IQuery
    {
        private string _sort;
        public string Query { get; set; }
        public int Offset { get; set; }
        public string Sort
        {
            get => string.IsNullOrEmpty(_sort) ? EditorialSortKeyValues.ListingPageDefaultSort : _sort;
            set => _sort = value != null && EditorialSortKeyValues.Items.TryGetValue(value, out var sort)
                ? sort.Key
                : EditorialSortKeyValues.ListingPageDefaultSort;
        }
    }
}