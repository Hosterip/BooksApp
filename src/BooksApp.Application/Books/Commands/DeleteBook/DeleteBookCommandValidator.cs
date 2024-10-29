using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Book.ValueObjects;
using BooksApp.Domain.Common.Security;
using FluentValidation;

namespace BooksApp.Application.Books.Commands.DeleteBook;

public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(post => post)
            .MustAsync(async (request, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleById(request.UserId);
                if (user is null) return false;
                var canDelete = RolePermissions.UpdateOrDeleteBook(user.Role.Name);
                return await unitOfWork.Books.AnyAsync(book =>
                    book.Id == BookId.CreateBookId(request.Id) &&
                    (book.Author.Id == user.Id || canDelete));
            }).WithMessage(BookValidationMessages.BookNotYour);
    }
}