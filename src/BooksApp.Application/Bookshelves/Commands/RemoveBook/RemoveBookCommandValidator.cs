using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Constants.ValidationMessages;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Bookshelves.Commands.RemoveBook;

public class RemoveBookCommandValidator : AbstractValidator<RemoveBookCommand>
{
    public RemoveBookCommandValidator(IUnitOfWork unitOfWork)
    {
        // Bookshelf
        RuleFor(request => request.BookshelfId)
            .MustAsync(async (bookshelfId, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyById(bookshelfId))
            .WithMessage(BookshelfValidationMessages.NotFound);
        
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                var bookshelf = await unitOfWork.Bookshelves.GetSingleById(request.BookshelfId);
                return bookshelf == null || bookshelf.User?.Id.Value == request.UserId;
            })
            .WithMessage(BookshelfValidationMessages.NotYours);
        // Books
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyBookById(request.BookshelfId, request.BookId))
            .WithMessage(BookshelfValidationMessages.NoBookToRemove)
            .OverridePropertyName(nameof(RemoveBookCommand.BookId));
        
        RuleFor(request => request.BookId)
            .MustAsync(async (bookId, cancellationToken) =>
                await unitOfWork.Books.AnyById(bookId))
            .WithMessage(BookValidationMessages.NotFound);
        
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);
    }
}