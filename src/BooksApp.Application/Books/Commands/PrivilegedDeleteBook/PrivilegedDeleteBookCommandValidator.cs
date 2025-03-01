using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Books.Commands.PrivilegedDeleteBook;

public sealed class PrivilegedDeleteBookCommandValidator : AbstractValidator<PrivilegedDeleteBookCommand>
{
    public PrivilegedDeleteBookCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var userId = userService.GetId()!.Value;

        RuleFor(request => request.Id)
            .MustAsync(async (bookId, cancellationToken) =>
                await unitOfWork.Books.AnyById(bookId, cancellationToken))
            .WithMessage(ValidationMessages.Book.NotFound);

        RuleFor(request => request)
            .MustAsync(async (_, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound)
            .WithName(nameof(UserId));

        RuleFor(request => request)
            .MustAsync(async (_, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleById(userId, cancellationToken);

                return user?.Role.Name is RoleNames.Admin;
            })
            .WithMessage(ValidationMessages.User.Permission)
            .WithName(nameof(UserId));
    }
}