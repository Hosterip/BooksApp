namespace BooksApp.Contracts.Requests.Reviews;

public class GetReviewsRequest
{
    public required int? Limit { get; init; }
    public required int? Page { get; init; }
}