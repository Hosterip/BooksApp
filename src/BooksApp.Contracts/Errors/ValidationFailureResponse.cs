namespace BooksApp.Contracts.Errors;

public class ValidationFailureResponse
{
    public required IEnumerable<ValidationError> Errors { get; set; } = [];
}

public class ValidationError
{
    public required string PropertyName { get; init; }
    public required string ErrorMessage { get; init; }
}