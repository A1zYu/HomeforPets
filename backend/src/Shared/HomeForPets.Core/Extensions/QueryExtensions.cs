﻿using System.Linq.Expressions;
using HomeForPets.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeForPets.Core.Extensions;

public static class QueryExtensions
{
    public static async Task<PagedList<T>> ToPagedList<T>(
        this IQueryable<T> source,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var totalCount = await source.CountAsync(cancellationToken);
        
        var items =await source.Skip((page-1) * pageSize)
            .Take(pageSize).ToListAsync(cancellationToken);
        return new PagedList<T>()
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
}