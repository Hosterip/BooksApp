using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace PostsApp.Middlewares;

public class ValidationExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ValidationExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException exception)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "ValidationFailure",
                Title = "Validation error",
                Detail = "One or more validation errors has occurred"
            };

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            if (exception.Errors is not null) problemDetails.Extensions["errors"] = exception.Errors;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}