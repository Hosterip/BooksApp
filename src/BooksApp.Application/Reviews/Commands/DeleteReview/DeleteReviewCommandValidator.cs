using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Review.ValueObjects;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Reviews.Commands.DeleteReview;

public sealed class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var userId = userService.GetId()!.Value;

        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                return await unitOfWork.Reviews
                    .AnyAsync(review =>
                        review.User.Id == UserId.Create(userId) &&
                        review.Id == ReviewId.Create(request.ReviewId), cancellationToken);
            })
            .WithMessage(ValidationMessages.User.Permission)
            .WithName(nameof(UserId));
    }
}