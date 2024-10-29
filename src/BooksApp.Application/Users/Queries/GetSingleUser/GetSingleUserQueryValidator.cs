using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Users.Queries.GetSingleUser;

public class GetSingleUserQueryValidator : AbstractValidator<GetSingleUserQuery>
{
    public GetSingleUserQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Id)
            .MustAsync(async (id, cancellationToken) => { return await unitOfWork.Users.AnyById(id); })
            .WithMessage(UserValidationMessages.NotFound);
    }
}