using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Users.Queries.GetUserRelationships;

public sealed class GetUserRelationshipsQueryValidator : AbstractValidator<GetUserRelationshipsQuery>
{
    public GetUserRelationshipsQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync(async (id, cancellationToken) =>
                await unitOfWork.Users.AnyById(id, cancellationToken))
            .WithMessage(ValidationMessages.User.NotFound);

        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(1);
        RuleFor(request => request.Limit)
            .InclusiveBetween(1, 50);
    }
}