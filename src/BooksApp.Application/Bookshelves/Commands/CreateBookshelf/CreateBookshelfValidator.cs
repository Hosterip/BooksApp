using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Commands.CreateBookshelf;

public sealed class CreateBookshelfValidator : AbstractValidator<CreateBookshelfCommand>
{
    public CreateBookshelfValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId, cancellationToken))
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                !await unitOfWork.Bookshelves.AnyByName(request.Name, request.UserId, cancellationToken))
            .WithMessage(BookshelfValidationMessages.AlreadyHaveWithSameName)
            .WithName(nameof(CreateBookshelfCommand.Name));
        RuleFor(request => request.Name)
            .MaximumLength(BookshelfMaxLengths.Name);
    }
}