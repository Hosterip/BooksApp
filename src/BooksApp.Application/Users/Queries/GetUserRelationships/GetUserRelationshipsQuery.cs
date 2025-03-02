using BooksApp.Application.Common.Results;
using BooksApp.Application.Users.Results;
using MediatR;

namespace BooksApp.Application.Users.Queries.GetUserRelationships;

public class GetUserRelationshipsQuery : IRequest<PaginatedArray<UserResult>>
{
    public string? Query { get; init; }
    public int? Page { get; init; }
    public int? Limit { get; init; }
    public required Guid UserId { get; init; }
    public required RelationshipType RelationshipType { get; init; } 
}

public enum RelationshipType
{
    Followers,
    Following
}