using FluentValidation;
using PostsApp.Application.Books.Commands.CreateBook;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.Common.Enums.MaxLengths;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Application.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Body)
            .MaximumLength((int)ReviewMaxLengths.Body)
            .NotEmpty();
        RuleFor(request => request.Rating)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(5);
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) => await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(request => request.BookId)
            .MustAsync(async (bookId, cancellationToken) =>
            {
                return await unitOfWork.Books.AnyById(bookId);
            })
            .WithMessage(ReviewValidationMessages.NotFound);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                return !await unitOfWork.Reviews.AnyAsync(review =>
                    review.User.Id == UserId.CreateUserId(request.UserId) 
                    && review.Book.Id == BookId.CreateBookId(request.BookId));
            })
            .WithMessage(ReviewValidationMessages.AlreadyHave)
            .OverridePropertyName(nameof(CreateReviewCommand.BookId));
    }
}