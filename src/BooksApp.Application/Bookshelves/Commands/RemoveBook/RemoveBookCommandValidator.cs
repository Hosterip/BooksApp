using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBook;

public sealed class RemoveBookCommandValidator : AbstractValidator<RemoveBookCommand>
{
    public RemoveBookCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var userId = userService.GetId()!.Value;
        
        // Bookshelf
        RuleFor(request => request.BookshelfId)
            .MustAsync(async (bookshelfId, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyById(bookshelfId, cancellationToken))
            .WithMessage(ValidationMessages.Bookshelf.NotFound);

        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var bookshelf = await unitOfWork.Bookshelves.GetSingleById(request.BookshelfId, cancellationToken);
                return bookshelf == null || bookshelf.UserId.Value == userId;
            })
            .WithMessage(ValidationMessages.Bookshelf.NotYours)
            .WithName(nameof(UserId));
        // Books
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyBookById(request.BookshelfId, request.BookId, cancellationToken))
            .WithMessage(ValidationMessages.Bookshelf.NoBookToRemove)
            .WithName(nameof(RemoveBookCommand.BookId));

        RuleFor(request => request.BookId)
            .MustAsync(async (bookId, cancellationToken) =>
                await unitOfWork.Books.AnyById(bookId, cancellationToken))
            .WithMessage(ValidationMessages.Book.NotFound);

        RuleFor(request => request)
            .MustAsync(async (_, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound);
    }
}