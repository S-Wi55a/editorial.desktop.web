using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    public class GetListingsQuery : IQuery
    {
        private string _sortOrder;
        public string Q { get; set; }
        public int Offset { get; set; }
        public string SortOrder
        {
            get => string.IsNullOrEmpty(_sortOrder) ? EditorialSortKeyValues.ListingPageDefaultSort : _sortOrder;
            set => _sortOrder = EditorialSortKeyValues.Items.TryGetValue(value, out var sortOrder)
                ? sortOrder.Key
                : EditorialSortKeyValues.ListingPageDefaultSort;
        }
        public string Keyword { get; set; }
    }
}