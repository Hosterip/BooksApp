using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Constants.ValidationMessages;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Constants;

namespace PostsApp.Application.Bookshelves.Commands.RemoveBookFromDefaultBookshelf;

public sealed class RemoveBookFromDefaultBookshelfCommandValidator : AbstractValidator<RemoveBookFromDefaultBookshelfCommand>
{
    public RemoveBookFromDefaultBookshelfCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) => 
                await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                await unitOfWork.Bookshelves.AnyBookById(request.BookshelfName, request.UserId, request.BookId))
            .WithMessage(BookshelfValidationMessages.NoBookToRemove)
            .OverridePropertyName("BookId");
        RuleFor(request => request.BookshelfName)
            .Must(bookshelfName => 
                bookshelfName is
                    DefaultBookshelvesNames.Read or
                    DefaultBookshelvesNames.ToRead or
                    DefaultBookshelvesNames.CurrentlyReading)
            .WithMessage(BookshelfValidationMessages.WrongNameOfTheBookshelf);
    }
}