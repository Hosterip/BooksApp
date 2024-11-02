using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants;
using BooksApp.Domain.Common.Enums.MaxLengths;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Commands.CreateBookshelf;

public sealed class CreateBookshelfValidator : AbstractValidator<CreateBookshelfCommand>
{
    public CreateBookshelfValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) =>
                await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                !await unitOfWork.Bookshelves.AnyByName(request.Name, request.UserId))
            .WithMessage(BookshelfValidationMessages.AlreadyHaveWithSameName)
            .OverridePropertyName(nameof(CreateBookshelfCommand.Name));
        RuleFor(request => request.Name)
            .MaximumLength((int)BookshelfMaxLengths.Name);
        RuleFor(request => request.Name)
            .Must(name => !DefaultBookshelvesNames.AllValues.Contains(name))
            .WithMessage(BookshelfValidationMessages.AlreadyHaveWithSameName);
    }
}