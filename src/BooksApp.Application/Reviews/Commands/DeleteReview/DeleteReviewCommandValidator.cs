using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Security;
using PostsApp.Domain.Review.ValueObjects;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Application.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleById(request.UserId);
                return RolePermissions.DeleteReview(user!.Role.Name) ||
                       await unitOfWork.Reviews
                           .AnyAsync(review => review.User.Id == UserId.CreateUserId(request.UserId) && review.Id == ReviewId.CreateReviewId(request.ReviewId));
            })
            .WithMessage(ConstantsUserException.Permission)
            .OverridePropertyName("UserId");
    }
}