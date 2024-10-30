using FluentValidation;

namespace BooksApp.Application.Reviews.Queries.GetReviews;

internal sealed class GetReviewsQueryValidator : AbstractValidator<GetReviewsQuery>
{
    public GetReviewsQueryValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(1);
        RuleFor(request => request.Limit)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100);
    }
}