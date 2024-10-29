using BooksApp.Application.Users.Results;
using MediatR;

namespace BooksApp.Application.Users.Queries.GetSingleUser;

public class GetSingleUserQuery : IRequest<UserResult>
{
    public required Guid Id { get; init; }
}