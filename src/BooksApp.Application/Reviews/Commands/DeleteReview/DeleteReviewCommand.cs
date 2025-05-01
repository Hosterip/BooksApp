using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.DeleteReview;

[Authorize]
public sealed class DeleteReviewCommand : IRequest
{
    public required Guid ReviewId { get; init; }
}