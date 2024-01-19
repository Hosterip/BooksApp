using Microsoft.EntityFrameworkCore;

namespace PostsApp.Application.Common.Results;

public class PaginatedArray<T>
{
    private PaginatedArray(T[] items, int page, int pageSize, int totalCount)
    {
        Items = items;
        Page = page;
        TotalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        TotalCount = totalCount;
    }
    public T[] Items { get; }
    public int Page { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public static async Task<PaginatedArray<T>> CreateAsync(IQueryable<T> queryable, int page, int pageSize)
    {
        var totalCount = await queryable.CountAsync();
        var items = await queryable.Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync();

        return new PaginatedArray<T>(items, page, pageSize, totalCount);
    }
}