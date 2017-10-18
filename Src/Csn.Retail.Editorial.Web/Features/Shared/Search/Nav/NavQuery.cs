using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Nav
{
    public class NavQuery : IQuery
    {
        private int _limit;
        private string _sortORder;
        public string Query { get; set; }
        public int Skip { get; set; }

        public int Limit
        {
            get => _limit == 0 ? 20 : _limit;
            set => _limit = value;
        }

        public string SortOder
        {
            get => _sortORder == string.Empty ? "Latest" : _sortORder;
            set => _sortORder = value;
        }
    }
}