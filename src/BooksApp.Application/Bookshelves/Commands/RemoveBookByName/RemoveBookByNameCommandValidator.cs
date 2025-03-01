using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Commands.RemoveBookByName;

public sealed class RemoveBookByNameCommandValidator : AbstractValidator<RemoveBookByNameCommand>
{
    public RemoveBookByNameCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var userId = userService.GetId()!.Value;

        RuleFor(request => request)
            .MustAsync(async (_, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound)
            .WithName(nameof(UserId));
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyBookByName(
                    request.BookshelfName,
                    userId,
                    request.BookId,
                    cancellationToken))
            .WithMessage(ValidationMessages.Bookshelf.NoBookToRemove)
            .WithName(nameof(RemoveBookByNameCommand.BookshelfName));
    }
}