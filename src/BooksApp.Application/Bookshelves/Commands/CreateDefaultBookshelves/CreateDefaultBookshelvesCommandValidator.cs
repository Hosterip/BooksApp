using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Bookshelves.Commands.CreateDefaultBookshelves;

public class CreateDefaultBookshelvesCommandValidator : AbstractValidator<CreateDefaultBookshelvesCommand>
{
    public CreateDefaultBookshelvesCommandValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(request => request.UserId)
            .MustAsync(async (userId, cancellationToken) => 
                await unitOfWork.Users.AnyById(userId))
            .WithMessage(UserValidationMessages.NotFound);
    }
}