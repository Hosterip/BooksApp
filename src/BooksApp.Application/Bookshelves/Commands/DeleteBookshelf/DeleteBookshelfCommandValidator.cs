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

                return bookshelf != null && !DefaultBookshelvesNames.AllValues.Contains(bookshelf.ReferentialName);
            })
            .WithName(nameof(DeleteBookshelfCommand.BookshelfId))
            .WithMessage(BookshelfValidationMessages.CannotDeleteDefault);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var bookshelf = await unitOfWork.Bookshelves.GetSingleById(request.BookshelfId, cancellationToken);

                return bookshelf != null && bookshelf.User?.Id.Value == request.UserId;
            })
            .WithName(nameof(DeleteBookshelfCommand.UserId))
            .WithMessage(BookshelfValidationMessages.NotYours);
    }
}