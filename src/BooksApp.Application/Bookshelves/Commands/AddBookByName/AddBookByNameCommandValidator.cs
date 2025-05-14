using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Commands.AddBookByName;

public sealed class AddBookByNameCommandValidator : AbstractValidator<AddBookByNameCommand>
{
    public AddBookByNameCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var userId = userService.GetId()!.Value;

        RuleFor(request => request.BookId)
            .MustAsync(async (bookId, cancellationToken) =>
                await unitOfWork.Books.AnyById(bookId, cancellationToken))
            .WithMessage(ValidationMessages.Book.NotFound);

        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                !await unitOfWork.Bookshelves.AnyBookByName(request.BookshelfName, userId, request.BookId,
                    cancellationToken))
            .WithMessage(ValidationMessages.Bookshelf.AlreadyExists)
            .WithName(nameof(AddBookByNameCommand.BookshelfName));
    }
}