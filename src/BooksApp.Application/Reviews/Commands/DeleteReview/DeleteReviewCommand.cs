using MediatR;

namespace PostsApp.Application.Reviews.Commands.DeleteReview;

public sealed class DeleteReviewCommand : IRequest
{
    public required Guid? UserId { get; init; }
    public required Guid ReviewId { get; init; }
}