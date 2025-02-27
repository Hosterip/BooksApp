using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Queries.BookshelfByName;

public sealed class BookshelfByNameQueryValidator : AbstractValidator<BookshelfByNameQuery>
{
    public BookshelfByNameQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyByName(request.Name, request.UserId, cancellationToken))
            .WithName(nameof(BookshelfByNameQuery.Name))
            .WithMessage(ValidationMessages.Bookshelf.NotFound);
    }
}