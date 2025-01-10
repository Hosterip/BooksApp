using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.UpdateName;

public sealed class UpdateNameCommandValidator : AbstractValidator<UpdateNameCommand>
{
    public UpdateNameCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) => 
                await unitOfWork.Users.AnyById(userId, cancellationToken));

        RuleFor(user => user.FirstName)
            .NotEmpty()
            .Length(1, UserMaxLengths.FirstName);

        RuleFor(user => user.MiddleName)
            .MaximumLength(UserMaxLengths.MiddleName);

        RuleFor(user => user.LastName)
            .MaximumLength(UserMaxLengths.LastName);
    }
}