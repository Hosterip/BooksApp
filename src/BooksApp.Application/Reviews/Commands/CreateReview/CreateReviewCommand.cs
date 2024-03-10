using MediatR;
using PostsApp.Application.Reviews.Results;

namespace PostsApp.Application.Reviews.Commands.CreateReview;

public class CreateReviewCommand : IRequest<ReviewResult>
{
    public required int Rating { get; init; }
    public required int UserId { get; init; }
    public required int BookId { get; init; }
    public required string Body { get; init; }
}