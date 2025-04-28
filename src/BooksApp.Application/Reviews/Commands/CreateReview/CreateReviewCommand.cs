using BooksApp.Application.Common.Attributes;
using BooksApp.Application.Reviews.Results;
using MediatR;

namespace BooksApp.Application.Reviews.Commands.CreateReview;

[Authorize]
public class CreateReviewCommand : IRequest<ReviewResult>
{
    public required int Rating { get; init; }
    public required Guid BookId { get; init; }
    public required string Body { get; init; }
}