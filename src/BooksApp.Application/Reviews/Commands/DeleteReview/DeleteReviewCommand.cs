using MediatR;

namespace PostsApp.Application.Reviews.Commands.DeleteReview;

public sealed class DeleteReviewCommand : IRequest
{
    public required int UserId { get; init; }
    public required int ReviewId { get; init; }
}