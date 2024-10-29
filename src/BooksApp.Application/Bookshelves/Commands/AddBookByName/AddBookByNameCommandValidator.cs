using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Commands.AddBookByName;

public class AddBookByNameCommandValidator : AbstractValidator<AddBookByNameCommand>
{
    public AddBookByNameCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(request => request.BookId)
            .MustAsync(async (bookId, cancellationToken) =>
                await unitOfWork.Books.AnyById(bookId))
            .WithMessage(BookValidationMessages.NotFound);

        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyBookByName(request.BookshelfName, request.UserId, request.BookId))
            .WithMessage(BookshelfValidationMessages.AlreadyExists);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
            {
                if (!await unitOfWork.Bookshelves.AnyByName(request.BookshelfName, request.UserId)) return true;

                return !await unitOfWork.Bookshelves.AnyBookByName(request.BookshelfName, request.UserId,
                    request.BookId);
            })
            .WithMessage(BookshelfValidationMessages.NoBookToRemove)
            .OverridePropertyName(nameof(AddBookByNameCommand.BookshelfName));
    }
}