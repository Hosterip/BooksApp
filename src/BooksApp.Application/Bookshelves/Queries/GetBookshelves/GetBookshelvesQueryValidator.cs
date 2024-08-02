using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Bookshelves.Queries.GetBookshelves;

public class GetBookshelvesQueryValidator : AbstractValidator<GetBookshelvesQuery>
{
    public GetBookshelvesQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) => 
                await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);
    }
}