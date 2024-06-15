using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Security;

namespace PostsApp.Application.Reviews.Commands.DeleteReview;

public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
{
    public DeleteReviewCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleWhereAsync(user => user.Id.Value == request.UserId);
                return RolePermissions.DeleteReview(user!.Role.Name) ||
                       await unitOfWork.Reviews
                           .AnyAsync(review => review.User.Id.Value == request.UserId && request.ReviewId == review.Id.Value);
            })
            .WithMessage(ConstantsUserException.Permission)
            .OverridePropertyName("UserId");
    }
}