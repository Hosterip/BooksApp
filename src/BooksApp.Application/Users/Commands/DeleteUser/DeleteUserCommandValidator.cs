﻿using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync(async (id, cancellationToken) =>
                await unitOfWork.Users.AnyById(id, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound);
    }
}