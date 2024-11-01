using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Queries.BookshelfByName;

internal sealed class BookshelfByNameQueryValidator : AbstractValidator<BookshelfByNameQuery>
{
    public BookshelfByNameQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyByName(request.Name, request.UserId))
            .OverridePropertyName(nameof(BookshelfByNameQuery.Name))
            .WithMessage(BookshelfValidationMessages.NotFound);
    }
}