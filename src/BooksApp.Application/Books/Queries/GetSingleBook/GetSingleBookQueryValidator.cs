using BooksApp.Application.Common.Constants.ValidationMessages;
using BooksApp.Application.Common.Interfaces;
using FluentValidation;

namespace BooksApp.Application.Books.Queries.GetSingleBook;

public sealed class GetSingleBookQueryValidator : AbstractValidator<GetSingleBookQuery>
{
    public GetSingleBookQueryValidator(IUnitOfWork unitOfWork)
    {
        RuleFor(book => book.Id)
            .MustAsync(async (id, cancellationToken) => 
                await unitOfWork.Books.AnyById(id, cancellationToken))
            .WithMessage(BookValidationMessages.NotFound);
    }
}