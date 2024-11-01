using FluentValidation;

namespace BooksApp.Application.Users.Queries.GetUsers;

internal sealed class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThanOrEqualTo(1);
        RuleFor(request => request.Limit)
            .InclusiveBetween(1, 50);
    }
}