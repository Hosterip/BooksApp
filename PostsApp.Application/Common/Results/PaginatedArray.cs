using Microsoft.EntityFrameworkCore;

namespace PostsApp.Application.Common.Results;

public class PaginatedArray<T>
{
    private PaginatedArray(T[] propItems, int propPage, int propPageSize, int propTotalCount)
    {
        items = propItems;
        page = propPage;
        pageSize = propPageSize;
        totalCount = propTotalCount;
    }
    public T[] items { get; set; }
    public int page { get; set; }
    public int pageSize { get; set; }
    public int totalCount { get; set; }

    public bool HasNextPage => page * pageSize < totalCount;
    public bool HasPreviousPage => page > 1;

    public static async Task<PaginatedArray<T>> CreateAsync(IQueryable<T> queryable, int page, int pageSize)
    {
        var totalCount = await queryable.CountAsync();
        var items = await queryable.Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync();

        return new PaginatedArray<T>(items, page, pageSize, totalCount);
    }
}