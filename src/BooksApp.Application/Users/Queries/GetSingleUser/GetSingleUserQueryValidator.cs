using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Users.Queries.GetSingleUser;

public sealed class GetSingleUserQueryValidator : AbstractValidator<GetSingleUserQuery>
{
    public GetSingleUserQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Id)
            .MustAsync(async (id, cancellationToken) =>
                await unitOfWork.Users.AnyById(id, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound);
    }
}