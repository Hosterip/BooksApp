using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Books.Commands.DeleteBook;

public sealed class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request)
            .MustAsync(async (request, cancellationToken) =>
                await unitOfWork.Books
                    .AnyAsync(
                        book => book.Id == BookId.CreateBookId(request.Id) &&
                                book.Author.Id == UserId.CreateUserId(request.UserId),
                        cancellationToken)
            ).WithMessage(BookValidationMessages.BookNotYour);
    }
}