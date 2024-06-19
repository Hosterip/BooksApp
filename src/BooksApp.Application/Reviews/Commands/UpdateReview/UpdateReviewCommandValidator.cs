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
            .MustAsync(async (userId, cancellationToken) => await unitOfWork.Users.AnyById(userId))
            .WithMessage(ConstantsUserException.NotFound);
        RuleFor(request => request.ReviewId)
            .MustAsync(async (reviewId, cancellationToken) => await unitOfWork.Reviews.AnyById(reviewId))
            .WithMessage(ConstantsReviewException.NotFound);
    }
}