using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using FluentValidation;

namespace BooksApp.Application.Books.Commands.PrivilegedDeleteBook;

public sealed class PrivilegedDeleteBookCommandValidator : AbstractValidator<PrivilegedDeleteBookCommand>
{
    public PrivilegedDeleteBookCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Id)
            .MustAsync(async (bookId, cancellationToken) =>
                await unitOfWork.Books.AnyById(bookId))
            .WithMessage(BookValidationMessages.NotFound);

        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);

        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleById(userId);

                return user?.Role.Name is RoleNames.Admin;
            })
            .WithMessage(UserValidationMessages.Permission);
    }
}