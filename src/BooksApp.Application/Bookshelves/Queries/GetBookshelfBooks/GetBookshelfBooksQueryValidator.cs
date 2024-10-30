using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Queries.GetBookshelfBooks;

internal sealed class GetBookshelfBooksQueryValidator : AbstractValidator<GetBookshelfBooksQuery>
{
    public GetBookshelfBooksQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.BookshelfId)
            .MustAsync(async (bookshelfId, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyById(bookshelfId))
            .WithMessage(BookshelfValidationMessages.NotFound);
    }
}