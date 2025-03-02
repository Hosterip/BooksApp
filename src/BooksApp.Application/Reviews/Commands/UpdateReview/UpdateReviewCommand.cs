using BooksApp.Application.Reviews.Results;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.UpdateReview;

public class UpdateReviewCommand : IRequest<ReviewResult>
{
    public required Guid ReviewId { get; init; }
    public required int Rating { get; init; }
    public required string Body { get; init; }
}