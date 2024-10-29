using BooksApp.Application.Common.Results;
using BooksApp.Application.Reviews.Results;
using MediatR;

namespace BooksApp.Application.Reviews.Queries.GetReviews;

public class GetReviewsQuery : IRequest<PaginatedArray<ReviewResult>>
{
    public required Guid BookId { get; init; }
    public required int Page { get; init; }
    public required int Limit { get; init; }
}