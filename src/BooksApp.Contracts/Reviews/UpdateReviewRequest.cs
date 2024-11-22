namespace BooksApp.Contracts.Reviews;

public class UpdateReviewRequest
{
    public required Guid ReviewId { get; init; }
    public required int Rating { get; init; }
    public required string Body { get; init; }
}