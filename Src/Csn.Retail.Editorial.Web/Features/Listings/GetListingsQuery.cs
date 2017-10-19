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
            get => string.IsNullOrEmpty(_sortOrder) ? "Latest" : _sortOrder;
            set => _sortOrder = value;
        }
        public string Keyword { get; set; }
    }
}