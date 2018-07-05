﻿using System;
using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Listings.Constants;

namespace Csn.Retail.Editorial.Web.Features.Shared.Helpers
{
    public interface IPaginationHelper
    {        
        PagingViewModel GetPaginationData(int count, int offset, string sortOrder, string query, string seoFragment, string keyword);
    }

    [AutoBind]
    public class PaginationHelper : IPaginationHelper
    {        
        private const int MinPageNo = 1;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public PaginationHelper(ITenantProvider<TenantInfo> tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public PagingViewModel GetPaginationData(int count, int pg, string sortOrder, string query, string seoFragment, string keyword)
        {
            if (count < 1)
            {
                return new PagingViewModel();
            }

            //int limit = ;
            int offset = pg == 0 ? 0 :(pg * PageItemsLimit.ListingPageItemsLimit) - PageItemsLimit.ListingPageItemsLimit;
            var currentPageNumber = GetCurrentPageNo(offset, PageItemsLimit.ListingPageItemsLimit);
            var totalPages = (int)Math.Ceiling((double)count / PageItemsLimit.ListingPageItemsLimit);
            var fistPageLink = count > 0 ? GeneratePageLink(1, PageItemsLimit.ListingPageItemsLimit, query, seoFragment, sortOrder, keyword) : null; // Only if there's at leaset one record
            var lastPageLink = totalPages >= 2 ? GeneratePageLink(totalPages, PageItemsLimit.ListingPageItemsLimit, query, seoFragment, sortOrder, keyword) : null; // Only if more 1 page
            var previousPageLink = totalPages >= 2 && currentPageNumber > 1 ? GeneratePageLink(currentPageNumber -1, PageItemsLimit.ListingPageItemsLimit, query, seoFragment, sortOrder, keyword) : null; //Only there's room to nevigate to previous page
            var nextPageLink = totalPages >= 2 && currentPageNumber < totalPages ? GeneratePageLink(currentPageNumber + 1, PageItemsLimit.ListingPageItemsLimit, query, seoFragment, sortOrder, keyword) : null; //Only there's room to nevigate to next page

            return new PagingViewModel
            {
                TotalCount = count,
                CurrentPageNo = currentPageNumber,
                TotalPageCount = totalPages,
                First = fistPageLink,
                Last = lastPageLink,
                Previous = previousPageLink,
                Next = nextPageLink,
                Pages = GeneratePageLinks(currentPageNumber, PageItemsLimit.ListingPageItemsLimit, totalPages, query, seoFragment, sortOrder, keyword).ToList(), // Only if more than 2 pages
                DisplayText = GetDisplayText(count, totalPages, offset, currentPageNumber, PageItemsLimit.ListingPageItemsLimit)
            };
        }

        #region Private Methods

        private string GetDisplayText(int count, int totalPages, int offset, int currentPageNumber, int limit)
        {
            if (count < 1) return string.Empty;
            else if (count == 1)
            {
                return count + " Article";

            }
            return offset + 1 + " - " + (currentPageNumber != totalPages ? offset + limit : count) + " of " + string.Format(_tenantProvider.Current().Culture, "{0:N0}", count) + " Articles";
        }

        private IEnumerable<PagingItemViewModel> GeneratePageLinks(long currentPageNo, int itemsPerPage, int totalPages, string query, string seoFragment, string sortOrder, string keyword)
        {
            var list = new List<PagingItemViewModel>();

            if (totalPages == 3)
            {
                list.Add(GeneratePageLink(2, itemsPerPage, query, seoFragment, sortOrder, keyword));
                return list;
            }
            if (totalPages == 4)
            {
                list.Add(GeneratePageLink(2, itemsPerPage, query, seoFragment, sortOrder, keyword));
                list.Add(GeneratePageLink(3, itemsPerPage, query, seoFragment, sortOrder, keyword));
            }
            if (totalPages >= 5)
            {
                if (currentPageNo >= 1 && currentPageNo <= 3)
                {
                    list.Add(GeneratePageLink(2, itemsPerPage, query, seoFragment, sortOrder, keyword));
                    list.Add(GeneratePageLink(3, itemsPerPage, query, seoFragment, sortOrder, keyword));
                    list.Add(GeneratePageLink(4, itemsPerPage, query, seoFragment, sortOrder, keyword));
                }
                else if (currentPageNo > totalPages - 3)
                {
                    list.Add(GeneratePageLink(totalPages - 3, itemsPerPage, query, seoFragment, sortOrder, keyword));
                    list.Add(GeneratePageLink(totalPages - 2, itemsPerPage, query, seoFragment, sortOrder, keyword));
                    list.Add(GeneratePageLink(totalPages - 1, itemsPerPage, query, seoFragment, sortOrder, keyword));
                }
                else
                {
                    list.Add(GeneratePageLink(currentPageNo - 1, itemsPerPage, query, seoFragment, sortOrder, keyword));
                    list.Add(GeneratePageLink(currentPageNo, itemsPerPage, query, seoFragment, sortOrder, keyword));
                    list.Add(GeneratePageLink(currentPageNo + 1, itemsPerPage, query, seoFragment, sortOrder, keyword));
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

        private PagingItemViewModel GeneratePageLink(long pageNo, int limit, string query, string seoFragment, string sortOrder, string keyword)
        {
           // var offset = (pageNo - MinPageNo) * limit;

            return new PagingItemViewModel
            {
                PageNo = pageNo,
                Url = ListingUrlHelper.GetPageAndSortPathAndQuery(query, pageNo, sortOrder, keyword, seoFragment)
            };        
        }

        #endregion
    }
}