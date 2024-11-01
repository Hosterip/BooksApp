using FluentValidation;

namespace BooksApp.Application.Books.Queries.GetBooks;

internal sealed class GetBooksQueryValidator : AbstractValidator<GetBooksQuery>
{
    public GetBooksQueryValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(1);
        RuleFor(request => request.Limit)
            .InclusiveBetween(1, 50);
    }
}