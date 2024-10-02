using FluentValidation;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Users.Commands.UpdateName;

public class UpdateNameCommandValidator : AbstractValidator<UpdateNameCommand>
{
    public UpdateNameCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync( async (userId, cancellationToken) => await unitOfWork.Users.AnyById(userId));
    }
}