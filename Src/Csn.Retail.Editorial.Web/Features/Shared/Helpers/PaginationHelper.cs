using System;
using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Helpers
{
    public interface IPaginationHelper
    {        
        PagingViewModel GetPaginationData(int count, int limit, int offset, string sortOrder, string query, string keyword);
    }

    [AutoBind]
    public class PaginationHelper : IPaginationHelper
    {        
        private const int MinPageNo = 1;

        public PagingViewModel GetPaginationData(int count, int limit, int offset, string sortOrder, string query, string keyword)
        {
            if (count < 1)
            {
                return new PagingViewModel();
            }
            var currentPageNumber = GetCurrentPageNo(offset, limit);
            var totalPages = limit != 0 ? (int)Math.Ceiling((double)count / limit)  : 0;
            var fistPageLink = count > 0 ? GeneratePageLink(1, limit, query, sortOrder, keyword) : null; // Only if there's at leaset one record
            var lastPageLink = totalPages >= 2 ? GeneratePageLink(totalPages, limit, query, sortOrder, keyword) : null; // Only if more 1 page
            var previousPageLink = totalPages >= 2 && currentPageNumber > 1 ? GeneratePageLink(currentPageNumber -1, limit, query, sortOrder, keyword) : null; //Only there's room to nevigate to previous page
            var nextPageLink = totalPages >= 2 && currentPageNumber < totalPages ? GeneratePageLink(currentPageNumber + 1, limit, query, sortOrder, keyword) : null; //Only there's room to nevigate to next page

            return new PagingViewModel
            {
                TotalCount = count,
                CurrentPageNo = currentPageNumber,
                TotalPageCount = totalPages,
                First = fistPageLink,
                Last = lastPageLink,
                Previous = previousPageLink,
                Next = nextPageLink,
                Pages = GeneratePageLinks(currentPageNumber, limit, totalPages, query, sortOrder, keyword).ToList(), // Only if more than 2 pages
                DisplayText = GetDisplayText(count, totalPages, offset, currentPageNumber, limit)
            };
        }        

        private string GetDisplayText(int count, int totalPages, int offset, int currentPageNumber, int limit)
        {
            if (count < 1) return string.Empty;
            return offset + 1 + " - " + (currentPageNumber != totalPages ? offset + limit : count) + " of " + count + " Article(s)";
        }

        private IEnumerable<PagingItemViewModel> GeneratePageLinks(long currentPageNo, int itemsPerPage, int totalPages, string query, string sortOrder, string keyword)
        {
            var list = new List<PagingItemViewModel>();

            if (totalPages == 3)
            {
                list.Add(GeneratePageLink(2, itemsPerPage, query, sortOrder, keyword));
                return list;
            }
            if (totalPages == 4)
            {
                list.Add(GeneratePageLink(2, itemsPerPage, query, sortOrder, keyword));
                list.Add(GeneratePageLink(3, itemsPerPage, query, sortOrder, keyword));
            }
            if (totalPages >= 5)
            {
                if (currentPageNo >= 1 && currentPageNo <= 3)
                {
                    list.Add(GeneratePageLink(2, itemsPerPage, query, sortOrder, keyword));
                    list.Add(GeneratePageLink(3, itemsPerPage, query, sortOrder, keyword));
                    list.Add(GeneratePageLink(4, itemsPerPage, query, sortOrder, keyword));
                }
                else if (currentPageNo > totalPages - 3)
                {
                    list.Add(GeneratePageLink(totalPages - 3, itemsPerPage, query, sortOrder, keyword));
                    list.Add(GeneratePageLink(totalPages - 2, itemsPerPage, query, sortOrder, keyword));
                    list.Add(GeneratePageLink(totalPages - 1, itemsPerPage, query, sortOrder, keyword));
                }
                else
                {
                    list.Add(GeneratePageLink(currentPageNo - 1, itemsPerPage, query, sortOrder, keyword));
                    list.Add(GeneratePageLink(currentPageNo, itemsPerPage, query, sortOrder, keyword));
                    list.Add(GeneratePageLink(currentPageNo + 1, itemsPerPage, query, sortOrder, keyword));
                }
            }
            
            return list;
        }

        private int GetCurrentPageNo(int limit, int itemsPerPage)
        {
            if (itemsPerPage == 0)
                return MinPageNo;

            var result = (limit / itemsPerPage) + MinPageNo;
            return result < MinPageNo ? MinPageNo : result;
        }

        #region Private Methods

        private PagingItemViewModel GeneratePageLink(long pageNo, int limit, string query, string sortOrder, string keyword)
        {
            var offset = (pageNo - MinPageNo) * limit;

            return new PagingItemViewModel
            {
                PageNo = pageNo,
                Url = $"/?q={query}&offset={offset}&sortOrder={sortOrder}&keyword={keyword}"
            };        
        }
        #endregion
    }
}