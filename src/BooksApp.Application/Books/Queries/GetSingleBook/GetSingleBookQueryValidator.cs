using FluentValidation;
using PostsApp.Application.Common.Constants.Exceptions;
using PostsApp.Application.Common.Constants.ValidationMessages;
using PostsApp.Application.Common.Interfaces;

namespace PostsApp.Application.Books.Queries.GetSingleBook;

public class GetSingleBookQueryValidator : AbstractValidator<GetSingleBookQuery>
{
    public GetSingleBookQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(book => book.Id)
            .MustAsync(async (id, cancellationToken) => await unitOfWork.Books.AnyById(id))
            .WithMessage(BookValidationMessages.NotFound);
    }
}