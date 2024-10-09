using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Enums.MaxLengths;
using PostsApp.Domain.User.ValueObjects;

namespace PostsApp.Application.Reviews.Commands.UpdateReview;

public class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Body)
            .MaximumLength((int)ReviewMaxLengths.Body)
            .NotEmpty();
        RuleFor(request => request.Rating)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(5);
        RuleFor(request => request.ReviewId)
            .MustAsync(async (reviewId, cancellationToken) => await unitOfWork.Reviews.AnyById(reviewId))
            .WithMessage(ReviewValidationMessages.NotFound);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var review = await unitOfWork.Reviews.GetSingleById(request.ReviewId);
                if (review is not null)
                    return review.User.Id == UserId.CreateUserId(request.UserId);
                return true;
            }).WithMessage(ReviewValidationMessages.NotYours);
    }
}