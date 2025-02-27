using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Reviews.Commands.UpdateReview;

public sealed class UpdateReviewCommandValidator : AbstractValidator<UpdateReviewCommand>
{
    public UpdateReviewCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Body)
            .MaximumLength(MaxPropertyLength.Review.Body)
            .NotEmpty();
        RuleFor(request => request.Rating)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(5);
        RuleFor(request => request.ReviewId)
            .MustAsync(async (reviewId, cancellationToken) => 
                await unitOfWork.Reviews.AnyById(reviewId, cancellationToken))
            .WithMessage(ValidationMessages.Review.NotFound);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var review = await unitOfWork.Reviews.GetSingleById(request.ReviewId, cancellationToken);
                if (review is not null)
                    return review.User.Id == UserId.Create(request.UserId);
                return true;
            }).WithMessage(ValidationMessages.Review.NotYours);
    }
}