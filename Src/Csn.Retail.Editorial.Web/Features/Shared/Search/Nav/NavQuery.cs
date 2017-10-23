using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Nav
{
    public class NavQuery : IQuery
    {
        private string _sortOrder;
        public string Query { get; set; }
        public int Offset { get; set; }
        public string SortOder
        {
            get => string.IsNullOrEmpty(_sortOrder) ? EditorialSortKeyValues.ListingPageDefaultSort : _sortOrder;
            set => _sortOrder = EditorialSortKeyValues.Items.TryGetValue(value, out var sortOrder)
                ? sortOrder.Key
                : EditorialSortKeyValues.ListingPageDefaultSort;
        }
    }
}