using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Constants.ValidationMessages;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Constants;

namespace PostsApp.Application.Bookshelves.Commands.AddBookToDefaultBookshelf;

public class AddBookByRefNameCommandValidator : AbstractValidator<AddBookByRefNameCommand>
{
    public AddBookByRefNameCommandValidator(IUnitOfWork unitOfWork)
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
            {
                if (!await unitOfWork.Bookshelves.AnyByRefName(request.BookshelfRefName, request.UserId)) return true;

                return !await unitOfWork.Bookshelves.AnyBookByRefName(request.BookshelfRefName, request.UserId,
                    request.BookId);
            })
            .WithMessage(BookshelfValidationMessages.NoBookToRemove)
            .OverridePropertyName("RefName");
    }
}