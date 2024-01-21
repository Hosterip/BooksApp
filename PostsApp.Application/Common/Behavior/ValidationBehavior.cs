using FluentValidation;
using FluentValidation.Results;
using MediatR;
using PostsApp.Domain.Exceptions;

namespace PostsApp.Application.Common.Behavior;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull

{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var validationResults = await Task.WhenAll(_validators
                .Select(validator => validator.ValidateAsync(request, cancellationToken)));

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .Select(r => 
                    new ValidationFailure{ErrorMessage = r.ErrorMessage, PropertyName = r.PropertyName})
                .ToList();
            if (failures.Any())
            {
                throw new ValidationException(failures);
            }
        }


        return await next();
    }
}