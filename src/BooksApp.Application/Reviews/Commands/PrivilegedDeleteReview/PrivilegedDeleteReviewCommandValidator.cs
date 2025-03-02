using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using FluentValidation;

namespace BooksApp.Application.Reviews.Commands.PrivilegedDeleteReview;

public sealed class PrivilegedDeleteReviewCommandValidator : AbstractValidator<PrivilegedDeleteReviewCommand>
{
    public PrivilegedDeleteReviewCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var userId = userService.GetId()!.Value;
        
        RuleFor(request => request.ReviewId)
            .MustAsync(async (reviewId, cancellationToken) =>
                await unitOfWork.Reviews.AnyById(reviewId, cancellationToken))
            .WithMessage(ValidationMessages.Review.NotFound);

        RuleFor(request => request)
            .MustAsync(async (_, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound)
            .WithName(nameof(userId));

        RuleFor(request => request)
            .MustAsync(async (_, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleById(userId, cancellationToken);

                return user?.Role.Name is RoleNames.Admin or RoleNames.Moderator;
            })
            .WithMessage(ValidationMessages.User.Permission)
            .WithName(nameof(userId));
    }
}