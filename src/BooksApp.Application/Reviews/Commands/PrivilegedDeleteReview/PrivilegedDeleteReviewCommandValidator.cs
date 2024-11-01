using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using FluentValidation;

namespace BooksApp.Application.Reviews.Commands.PrivilegedDeleteReview;

internal sealed class PrivilegedDeleteReviewCommandValidator : AbstractValidator<PrivilegedDeleteReviewCommand>
{
    public PrivilegedDeleteReviewCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.ReviewId)
            .MustAsync(async (reviewId, cancellationToken) =>
                await unitOfWork.Reviews.AnyById(reviewId))
            .WithMessage(ReviewValidationMessages.NotFound);

        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);

        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleById(userId);

                return user?.Role.Name is RoleNames.Admin or RoleNames.Moderator;
            })
            .WithMessage(UserValidationMessages.Permission);
    }
}