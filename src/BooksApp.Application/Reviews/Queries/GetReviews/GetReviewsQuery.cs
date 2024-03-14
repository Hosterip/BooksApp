using MediatR;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Reviews.Results;

namespace PostsApp.Application.Reviews.Queries.GetReviews;

public class GetReviewsQuery : IRequest<PaginatedArray<ReviewResult>>
{
    public required int BookId { get; init; }
    public int? Page { get; set; }
    public int? PageSize { get; set; }
}