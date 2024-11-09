using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Commands.DeleteBookshelf;

public sealed class DeleteBookshelfCommandValidator : AbstractValidator<DeleteBookshelfCommand>
{
    public DeleteBookshelfCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var bookshelf = await unitOfWork.Bookshelves.GetSingleById(request.BookshelfId, cancellationToken);

                if (bookshelf is null &&
                    bookshelf?.User?.Id.Value == request.UserId) return false;
                return bookshelf != null && !DefaultBookshelvesNames.AllValues.Contains(bookshelf.ReferentialName);
            })
            .OverridePropertyName(nameof(DeleteBookshelfCommand.BookshelfId))
            .WithMessage(BookshelfValidationMessages.CannotDeleteDefault);
    }
}