using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Books.Commands.DeleteBook;

public sealed class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookCommandValidator(IUnitOfWork unitOfWork, IUserService userService)
    {
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                await unitOfWork.Books
                    .AnyAsync(
                        book => book.Id == BookId.Create(request.Id) &&
                                book.Author.Id == UserId.Create(userService.GetId()!.Value),
                        cancellationToken))
            .WithMessage(ValidationMessages.Book.BookNotYour)
            .WithName(nameof(UserId));
    }
}