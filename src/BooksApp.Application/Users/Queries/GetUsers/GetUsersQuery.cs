using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using MediatR;

namespace BooksApp.Application.Users.Queries.GetUsers;

public class GetUsersQuery : IRequest<PaginatedArray<UserResult>>
{
    public string? Query { get; init; }
    public int? Page { get; init; }
    public int? Limit { get; init; }
}