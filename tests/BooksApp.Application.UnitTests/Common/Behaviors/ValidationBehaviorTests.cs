using BooksApp.Application.Books.Commands.CreateBook;
using BooksApp.Application.Books.Results;
using BooksApp.Application.Common.Behaviors;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using NSubstitute;
using TestCommon.Books;
using ValidationException = BooksApp.Application.Common.Errors.ValidationException;

namespace BooksApp.Application.UnitTests.Common.Behaviors;

public class ValidationBehaviorTests
{
    private readonly RequestHandlerDelegate<BookResult> _next;
    private readonly ValidationBehavior<CreateBookCommand, BookResult> _validationBehavior;
    private readonly IValidator<CreateBookCommand> _validator;

    public ValidationBehaviorTests()
    {
        _validator = Substitute.For<IValidator<CreateBookCommand>>();
        _next = Substitute.For<RequestHandlerDelegate<BookResult>>();
        _validationBehavior = new ValidationBehavior<CreateBookCommand, BookResult>([_validator]);
    }

    [Fact]
    public async Task InvokeHandle_WhenValidatorReturnsValid_ShouldInvokeNext()
    {
        // Arrange
        //  Create a command
        var command = BookCommandFactory.CreateCreateBookCommand();

        // Make next return something
        var book = BookResultFactory.CreateBook();
        _next.Invoke().Returns(book);

        // Make validator ValidateAsync method return something
        _validator.ValidateAsync(command).Returns(new ValidationResult());

        // Act
        var result = await _validationBehavior.Handle(command, _next, default);

        // Assert
        result.Should().BeEquivalentTo(book);
    }

    [Fact]
    public async Task InvokeHandle_WhenValidatorReturnsError_ShouldThrowAnError()
    {
        // Arrange
        //  Create a command
        var command = BookCommandFactory.CreateCreateBookCommand();

        // Make validator ValidateAsync method return something
        _validator.ValidateAsync(command).Returns(
            new ValidationResult(
                [new ValidationFailure(string.Empty, string.Empty)]));

        // Act
        Func<Task> act = () => _validationBehavior.Handle(command, _next, default);

        // Assert
        await Assert.ThrowsAsync<ValidationException>(act);
    }
}