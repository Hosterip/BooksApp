using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Constants.ValidationMessages;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Book.ValueObjects;
using PostsApp.Domain.Common.Security;

namespace PostsApp.Application.Books.Commands.DeleteBook;

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
            }).WithMessage(BookValidationMessages.PostNotYour);
    }
}