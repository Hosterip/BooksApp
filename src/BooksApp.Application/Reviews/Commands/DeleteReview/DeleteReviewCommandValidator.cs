using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Review.ValueObjects;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Reviews.Commands.DeleteReview;

internal sealed class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                return await unitOfWork.Reviews
                    .AnyAsync(review =>
                        review.User.Id == UserId.CreateUserId(request.UserId) &&
                        review.Id == ReviewId.CreateReviewId(request.ReviewId));
            })
            .WithMessage(UserValidationMessages.Permission)
            .OverridePropertyName(nameof(DeleteReviewCommand.UserId));
    }
}