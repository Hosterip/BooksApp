namespace BooksApp.Application.Common.Constants.ValidationMessages;

public partial class ValidationMessages
{
    public static class Image
    {
        public static readonly string WrongFileName =
            $"Wrong file extension. Allowed extensions: {AppConstants.AllowedExtensions}";

        public const string NotFound = "Image was not found";
    }
}