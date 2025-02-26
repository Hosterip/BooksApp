using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBook;

public sealed class RemoveBookCommandValidator : AbstractValidator<RemoveBookCommand>
{
    public RemoveBookCommandValidator(IUnitOfWork unitOfWork)
    {
        // Bookshelf
        RuleFor(request => request.BookshelfId)
            .MustAsync(async (bookshelfId, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyById(bookshelfId, cancellationToken))
            .WithMessage(BookshelfValidationMessages.NotFound);

        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var bookshelf = await unitOfWork.Bookshelves.GetSingleById(request.BookshelfId, cancellationToken);
                return bookshelf == null || bookshelf.UserId.Value == request.UserId;
            })
            .WithMessage(BookshelfValidationMessages.NotYours)
            .WithName(nameof(RemoveBookCommand.UserId));
        // Books
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyBookById(request.BookshelfId, request.BookId, cancellationToken))
            .WithMessage(BookshelfValidationMessages.NoBookToRemove)
            .WithName(nameof(RemoveBookCommand.BookId));

        RuleFor(request => request.BookId)
            .MustAsync(async (bookId, cancellationToken) =>
                await unitOfWork.Books.AnyById(bookId, cancellationToken))
            .WithMessage(BookValidationMessages.NotFound);

        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(UserValidationMessages.NotFound);
    }
}