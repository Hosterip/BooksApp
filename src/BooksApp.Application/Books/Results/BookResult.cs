using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Books.Results;

public record BookResult
{
    public required int Id { get; init; }
    public required int LikeCount { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required UserResult Author { get; init; }
}