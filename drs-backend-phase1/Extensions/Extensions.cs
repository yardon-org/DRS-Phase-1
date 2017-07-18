﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace drs_backend_phase1.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Does WebAPI paging.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="page">The page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <returns></returns>
        public static object DoPaging(this IOrderedQueryable<object> query, int page, int pageSize)
        {
            if (pageSize > 200) pageSize = 200; // Correct for too large a pagesize
            var totalCount = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var results = query
                .Skip(pageSize * page)
                .Take(pageSize)
                .ToList();

            return new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                PageSize = pageSize,
                Results = results
            };
        }
    }
}