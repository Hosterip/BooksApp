using BooksApp.Application.Reviews.Results;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.CreateReview;

public class CreateReviewCommand : IRequest<ReviewResult>
{
    public required int Rating { get; init; }
    public required Guid UserId { get; init; }
    public required Guid BookId { get; init; }
    public required string Body { get; init; }
}