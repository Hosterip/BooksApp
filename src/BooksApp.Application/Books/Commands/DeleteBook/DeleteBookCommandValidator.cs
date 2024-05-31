using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Constants;
using PostsApp.Domain.Security;

namespace PostsApp.Application.Books.Commands.DeleteBook;

public class DeleteBookCommandValidator : AbstractValidator<DeleteBookCommand>
{
    public DeleteBookCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(post => post)
            .MustAsync(async (request, cancellationToken) =>
            {
                var user = await unitOfWork.Users.GetSingleWhereAsync(user => user.Id == request.UserId);
                if (user is null) return false; 
                var canDelete = RolePermissions.UpdateOrDeleteBook(user.Role.Name);
                return await unitOfWork.Books.AnyAsync(book => 
                    book.Id == request.Id &&
                    (book.Author.Id == request.UserId || canDelete));
            }).WithMessage(ConstantsBookException.PostNotYour);
    }
}