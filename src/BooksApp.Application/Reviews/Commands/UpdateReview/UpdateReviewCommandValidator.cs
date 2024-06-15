using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator(IUnitOfWork unitOfWork)
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
                return await unitOfWork.Users.AnyAsync(user => user.Id.Value == userId);
            })
            .WithMessage(ConstantsUserException.NotFound);
        RuleFor(request => request.ReviewId)
            .MustAsync(async (reviewId, cancellationToken) =>
            {
                return await unitOfWork.Reviews.AnyAsync(review => review.Id.Value == reviewId);
            })
            .WithMessage(ConstantsReviewException.NotFound);
    }
}