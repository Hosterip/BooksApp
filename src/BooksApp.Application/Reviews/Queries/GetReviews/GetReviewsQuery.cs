using MediatR;
using PostsApp.Application.Common.Results;
using PostsApp.Application.Reviews.Results;

namespace PostsApp.Application.Reviews.Queries.GetReviews;

public class GetReviewsQuery : IRequest<PaginatedArray<ReviewResult>>
{
    public required Guid BookId { get; init; }
    public required int Page { get; init; }
    public required int PageSize { get; init; }
}