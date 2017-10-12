using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    public class GetListingsQuery : IQuery
    {
        private int _limit;
        private string _sortORder;
        public string Q { get; set; }
        public int Skip { get; set; }

        public int Limit
        {
            get => _limit == 0 ? 20 : _limit;
            set => _limit = value;
        }

        public string SortOrder
        {
            get => string.IsNullOrEmpty(_sortORder) ? "Latest" : _sortORder;
            set => _sortORder = value;
        }
    }
}