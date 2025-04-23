using BooksApp.Application.Common.Attributes;
using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Reviews.Commands.CreateReview;

[Authorize]
public sealed class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var userId = userService.GetId()!.Value;
        
        RuleFor(request => request.Body)
            .MaximumLength(MaxPropertyLength.Review.Body)
            .NotEmpty();
        RuleFor(request => request.Rating)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(5);
        RuleFor(request => request.BookId)
            .MustAsync(async (bookId, cancellationToken) => await unitOfWork.Books.AnyById(bookId, cancellationToken))
            .WithMessage(ValidationMessages.Book.NotFound);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                return !await unitOfWork.Reviews.AnyAsync(review =>
                    review.User.Id == UserId.Create(userId)
                    && review.Book.Id == BookId.Create(request.BookId), 
                    cancellationToken);
            })
            .WithMessage(ValidationMessages.Review.AlreadyHave)
            .WithName(nameof(CreateReviewCommand.BookId));
    }
}