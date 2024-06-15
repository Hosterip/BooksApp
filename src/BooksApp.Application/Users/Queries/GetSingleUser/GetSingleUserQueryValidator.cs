using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Users.Queries.GetSingleUser;

public class GetSingleUserQueryValidator : AbstractValidator<GetSingleUserQuery>
{
    public GetSingleUserQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.Id)
            .MustAsync(async (id, cancellationToken) =>
            {
                return await unitOfWork.Users.AnyAsync(user => user.Id.Value == id);
            })
            .WithMessage(ConstantsUserException.NotFound);
    }
}