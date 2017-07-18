using System;
using System.Linq;

namespace drs_backend_phase1.Extensions
{
    /// <summary>
    ///     Paging Extensions
    /// </summary>
    public static class PagingExtensions
    {
        /// <summary>
        ///     Does WebAPI paging.
        /// </summary>
        /// <param name="query">The query - must be of type IOrderedQueryable.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static object DoPaging(this IOrderedQueryable<object> query, int page, int pageSize)
        {
            if (pageSize > 200) pageSize = 200; // Correct for too large a pagesize
            if (page == 0) page = 1;
            page -= 1;
            var totalCount = query.Count();
            var totalPages = (int) Math.Ceiling((double) totalCount / pageSize);

            var results = query
                .Skip(pageSize * page)
                .Take(pageSize)
                .ToList();

            return new
            {
                TotalPages = totalPages,
                TotalCount = totalCount,
                CurrentPage = page + 1,
                CountPerPage = pageSize,
                Results = results
            };
        }
    }
}