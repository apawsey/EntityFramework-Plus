﻿// Description: Entity Framework Bulk Operations & Utilities (EF Bulk SaveChanges, Insert, Update, Delete, Merge | LINQ Query Cache, Deferred, Filter, IncludeFilter, IncludeOptimize | Audit)
// Website & Documentation: https://github.com/zzzprojects/Entity-Framework-Plus
// Forum & Issues: https://github.com/zzzprojects/EntityFramework-Plus/issues
// License: https://github.com/zzzprojects/EntityFramework-Plus/blob/master/LICENSE
// More projects: http://www.zzzprojects.com/
// Copyright © ZZZ Projects Inc. 2014 - 2016. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Z.EntityFramework.Plus
{
    public static class QueryIncludeFilterExtensions
    {
        /// <summary>
        ///     An IQueryable&lt;T&gt; extension method that include and filter related entities.
        /// </summary>
        /// <typeparam name="T">The type of elements of the query.</typeparam>
        /// <typeparam name="TChild">The type of elements of the child query.</typeparam>
        /// <param name="query">The query to filter included related entities.</param>
        /// <param name="queryIncludeFilter">The query filter to apply on included related entities.</param>
        /// <returns>An IQueryable&lt;T&gt; that include and filter related entities.</returns>
        public static IQueryable<T> IncludeFilter<T, TChild>(this IQueryable<T> query, Expression<Func<T, IEnumerable<TChild>>> queryIncludeFilter) where T : class where TChild : class
        {
            // GET query root
            var includeOrderedQueryable = query as QueryIncludeFilterParentQueryable<T> ?? new QueryIncludeFilterParentQueryable<T>(query);

            // ADD sub query
            includeOrderedQueryable.Childs.Add(new QueryIncludeFilterChild<T, TChild>(queryIncludeFilter));

            // RETURN root
            return includeOrderedQueryable;
        }
    }
}