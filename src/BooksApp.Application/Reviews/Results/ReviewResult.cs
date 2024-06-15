using PostsApp.Application.Common.Results;

namespace PostsApp.Application.Reviews.Results;

public class ReviewResult
{
    public required string Id { get; init; }
    public required int Rating { get; init; }
    public required string BookId { get; init; }
    public required string Body { get; init; }
    public required UserResult User { get; init; }
}