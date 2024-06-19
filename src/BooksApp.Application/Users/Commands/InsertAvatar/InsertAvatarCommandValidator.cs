using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Users.Commands.InsertAvatar;

public class InsertAvatarCommandValidator : AbstractValidator<InsertAvatarCommand>
{
    public InsertAvatarCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Id)
            .MustAsync(async (id, cancellationToken) => await unitOfWork.Users.AnyById(id))
            .WithMessage(ConstantsUserException.NotFound);
    }
}