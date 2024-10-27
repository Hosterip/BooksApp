using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Common.Extensions;

public static class PaginationExtension
{
    public static Task<PaginatedArray<T>> PaginationAsync<T>(this IQueryable<T> queryable, int page, int pageSize)
        where T : class
    {
        return PaginatedArray<T>.CreateAsync(queryable, page, pageSize);
    }
}