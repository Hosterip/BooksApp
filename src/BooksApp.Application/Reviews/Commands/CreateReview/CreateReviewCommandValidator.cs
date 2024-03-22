using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Body)
            .MaximumLength(1000)
            .NotEmpty();
        RuleFor(request => request.Rating)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(5);
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
            {
                return await unitOfWork.Users.AnyAsync(user => user.Id == userId);
            })
            .WithMessage(ConstantsUserException.NotFound);
        RuleFor(request => request.BookId)
            .MustAsync(async (bookId, cancellationToken) =>
            {
                return await unitOfWork.Books.AnyAsync(book => book.Id == bookId);
            })
            .WithMessage(ConstantsReviewException.NotFound);
    }
}