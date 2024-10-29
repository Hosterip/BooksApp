namespace BooksApp.Contracts.Requests.Reviews;

public class GetReviewsRequest
{
    public int? Limit { get; init; }
    public int? Page { get; init; }
}