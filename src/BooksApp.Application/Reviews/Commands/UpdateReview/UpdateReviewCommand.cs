using MediatR;
using PostsApp.Application.Reviews.Results;

namespace PostsApp.Application.Reviews.Commands.UpdateReview;

public class UpdateReviewCommand : IRequest<ReviewResult>
{
    public required int ReviewId { get; init; }
    public required int UserId { get; init; }
    public required int Rating { get; init; }
    public required string Body { get; init; }
}