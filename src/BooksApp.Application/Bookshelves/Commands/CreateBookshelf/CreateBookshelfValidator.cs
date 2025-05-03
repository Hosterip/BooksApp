using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentValidation;

namespace BooksApp.Application.Bookshelves.Commands.CreateBookshelf;

public sealed class CreateBookshelfValidator : AbstractValidator<CreateBookshelfCommand>
{
    public CreateBookshelfValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        var userId = userService.GetId()!.Value;

        RuleFor(request => request.Name)
            .MustAsync(async (name, cancellationToken) =>
                !await unitOfWork.Bookshelves.AnyByName(name, userId, cancellationToken))
            .WithMessage(ValidationMessages.Bookshelf.AlreadyHaveWithSameName);
        RuleFor(request => request.Name)
            .MaximumLength(MaxPropertyLength.Bookshelf.Name);
    }
}