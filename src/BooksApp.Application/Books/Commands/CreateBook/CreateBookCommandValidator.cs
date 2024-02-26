using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common;
using PostsApp.Domain.Constants;

namespace PostsApp.Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(post => post.Title).NotEmpty().Length(1, 255);
        RuleFor(post => post.Description).NotEmpty().Length(1, 1000);
        RuleFor(post => post.UserId).MustAsync(async (id, cancellationToken) =>
        {
            return await unitOfWork.Users
                .AnyAsync(user => 
                    user.Id == id);
        }).WithMessage(ConstantsUserException.NotFound);
        RuleFor(request => request.UserId).MustAsync(async (id, cancellationToken) =>
        {
            var user = await unitOfWork.Users.GetSingleWhereAsync(user => id == user.Id);
            if (user is null) return false;
            return RolePermissions.CreateBook(user.Role.Name);
        }).WithMessage(ConstantsBookException.MustBeAnAuthor);
        
    }
}