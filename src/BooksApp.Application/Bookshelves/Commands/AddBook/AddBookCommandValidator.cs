using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Commands.AddBook;

public sealed class AddBookCommandValidator : AbstractValidator<AddBookCommand>
{
    public AddBookCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var userId = userService.GetId()!.Value;
        
        RuleFor(request => request.BookshelfId)
            .MustAsync(async (bookshelfId, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyById(bookshelfId, cancellationToken))
            .WithMessage(ValidationMessages.Bookshelf.NotFound);
        
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                !await unitOfWork.Bookshelves.AnyBookById(request.BookshelfId, request.BookId, cancellationToken))
            .WithMessage(ValidationMessages.Bookshelf.AlreadyExists)
            .WithName(nameof(AddBookCommand.BookId));

        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var bookshelf = await unitOfWork.Bookshelves.GetSingleById(request.BookshelfId, cancellationToken);
                return bookshelf == null || bookshelf.UserId == UserId.Create(userId);
            })
            .WithMessage(ValidationMessages.Bookshelf.NotYours)
            .WithName(nameof(UserId)); 

        RuleFor(request => request.BookId)
            .MustAsync(async (bookId, cancellationToken) =>
                await unitOfWork.Books.AnyById(bookId, cancellationToken))
            .WithMessage(ValidationMessages.Book.NotFound);

        RuleFor(request => request)
            .MustAsync(async (_, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound)
            .WithName(nameof(UserId));
    }
}