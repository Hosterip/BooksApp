using BooksApp.Application.Common.Attributes;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.PrivilegedDeleteReview;

[Authorize]
public sealed class PrivilegedDeleteReviewCommand : IRequest
{
    public required Guid ReviewId { get; init; }
}