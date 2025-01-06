namespace BooksApp.Application.Common.Errors;

[Serializable]
public class ValidationException : Exception
{
    public List<ValidationFailure> Errors { get; private set; }
    public ValidationException (List<ValidationFailure> errors) : base(ConvertToString(errors))
    {
        Errors = errors;
    }

    public static string ConvertToString(List<ValidationFailure> errors)
    {
        var arr = errors.Select(x => $"{Environment.NewLine} -- {x.PropertyName} - {x.ErrorMessage} --");
        
        return string.Format(string.Join("Validation failed: ", arr));
    }
}

public class ValidationFailure
{
    public required string PropertyName { get; init; }
    public required string ErrorMessage { get; init; }
}

