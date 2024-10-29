using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Commands.DeleteBookshelf;

public class DeleteBookshelfCommandValidator : AbstractValidator<DeleteBookshelfCommand>
{
    public DeleteBookshelfCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.BookshelfId)
            .MustAsync(async (bookshelfId, cancellationToken) =>
            {
                var bookshelf = await unitOfWork.Bookshelves.GetSingleById(bookshelfId);

                if (bookshelf is null) return false;
                if (DefaultBookshelvesNames.AllValues.Contains(bookshelf.ReferentialName)) return false;
                return true;
            });
    }
}