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
            get => _sortOrder == string.Empty ? "Latest" : _sortOrder;
            set => _sortOrder = value;
        }
    }
}