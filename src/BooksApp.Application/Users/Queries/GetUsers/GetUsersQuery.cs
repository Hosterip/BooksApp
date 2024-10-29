using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using MediatR;

namespace BooksApp.Application.Users.Queries.GetUsers;

public class GetUsersQuery : IRequest<PaginatedArray<UserResult>>
{
    public string? Query { get; set; }
    public int? Page { get; set; }
    public int? Limit { get; set; }
}