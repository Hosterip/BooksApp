using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Constants.ValidationMessages;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Constants;

namespace PostsApp.Application.Bookshelves.Commands.AddBookToDefaultBookshelf;

public class AddBookToDefaultBookshelfCommandValidator : AbstractValidator<AddBookToDefaultBookshelfCommand>
{
    public AddBookToDefaultBookshelfCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) => 
                await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(request => request.BookId)
            .MustAsync(async (bookId, cancellationToken) => 
                await unitOfWork.Books.AnyById(bookId))
            .WithMessage(BookValidationMessages.NotFound);
        RuleFor(request => request.BookshelfName)
            .Must(bookshelfName => 
                bookshelfName.ToLowerInvariant().Trim() is
                    DefaultBookshelvesNames.Read or
                    DefaultBookshelvesNames.ToRead or
                    DefaultBookshelvesNames.CurrentlyReading)
            .WithMessage(BookshelfValidationMessages.WrongNameOfTheBookshelf);
    }
}