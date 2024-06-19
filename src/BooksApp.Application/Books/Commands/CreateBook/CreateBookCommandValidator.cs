using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Common.Security;

namespace PostsApp.Application.Books.Commands.CreateBook;

public class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(post => post.Title).NotEmpty().Length(1, 255);
        RuleFor(post => post.Description).NotEmpty().Length(1, 1000);
        RuleFor(post => post.UserId)
            .MustAsync(async (id, cancellationToken) => await unitOfWork.Users.AnyById(id))
            .WithMessage(ConstantsUserException.NotFound);
        RuleFor(request => request.UserId).MustAsync(async (id, cancellationToken) =>
        {
            var user = await unitOfWork.Users.GetSingleById(id);
            if (user is null) return false;
            return RolePermissions.CreateBook(user.Role.Name);
        }).WithMessage(ConstantsBookException.MustBeAnAuthor);
        
    }
}