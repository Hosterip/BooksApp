namespace BooksApp.Contracts.Reviews;

public class CreateReviewRequest
{
    public required Guid BookId { get; init; }
    public required int Rating { get; init; }
    public required string Body { get; init; }
}