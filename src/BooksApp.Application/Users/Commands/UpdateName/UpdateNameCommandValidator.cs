using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Domain.Common.Constants.MaxLengths;
using BooksApp.Domain.User.ValueObjects;
using FluentValidation;

namespace BooksApp.Application.Users.Commands.UpdateName;

public sealed class UpdateNameCommandValidator : AbstractValidator<UpdateNameCommand>
{
    public UpdateNameCommandValidator()
    {
        RuleFor(user => user.FirstName)
            .NotEmpty()
            .Length(1, MaxPropertyLength.User.FirstName);

        RuleFor(user => user.MiddleName)
            .MaximumLength(MaxPropertyLength.User.MiddleName);

        RuleFor(user => user.LastName)
            .MaximumLength(MaxPropertyLength.User.LastName);
    }
}