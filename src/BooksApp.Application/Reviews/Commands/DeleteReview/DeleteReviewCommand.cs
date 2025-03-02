using MediatR;

namespace BooksApp.Application.Reviews.Commands.DeleteReview;

public sealed class DeleteReviewCommand : IRequest
{
    public required Guid ReviewId { get; init; }
}