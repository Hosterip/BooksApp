using FluentValidation;

namespace BooksApp.Application.Reviews.Queries.GetReviews;

public sealed class GetReviewsQueryValidator : AbstractValidator<GetReviewsQuery>
{
    public GetReviewsQueryValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(1);
        RuleFor(request => request.Limit)
            .InclusiveBetween(1, 50);
    }
}