namespace PostsApp.Common.Contracts.Requests.Review;

public class GetReviewsRequest
{
    public required int? Limit { get; init; }
    public required int? Page { get; init; }
}