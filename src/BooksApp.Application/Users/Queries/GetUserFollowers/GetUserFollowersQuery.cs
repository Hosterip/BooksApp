using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using MediatR;

namespace BooksApp.Application.Users.Queries.GetUserFollowers;

public class GetUserFollowersQuery : IRequest<PaginatedArray<UserResult>>
{
    public string? Query { get; init; }
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public required Guid? CurrentUserId { get; init; }
    public required Guid UserId { get; init; }
}