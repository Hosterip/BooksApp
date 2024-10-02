using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Application.Users.Commands.UpdateEmail;

namespace PostsApp.Application.Users.Commands.UpdateUsername;

public class UpdateEmailCommandValidator : AbstractValidator<UpdateEmailCommand>
{

    public UpdateEmailCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(user => user.Id)
            .MustAsync(async (id, cancellationToken) =>
            {
                return await unitOfWork.Users.AnyById(id);
            })
            .WithMessage(UserValidationMessages.NotFound);
        RuleFor(user => user.Email)
            .Length(0, 255)
            .NotEmpty();
        RuleFor(user => user.Email)
            .MustAsync(async (email, cancellationToken) =>
            {
                return !await unitOfWork.Users.AnyAsync(user => user.Email == email) 
                       && new EmailAddressAttribute().IsValid(email);
            })
            .WithMessage(UserValidationMessages.Occupied);
    }
}