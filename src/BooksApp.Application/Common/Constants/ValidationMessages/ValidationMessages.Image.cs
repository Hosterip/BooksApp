namespace BooksApp.Application.Common.Constants.ValidationMessages;

public partial class ValidationMessages
{
    public static class Image
    {
        public const string NotFound = "Image was not found";

        public static readonly string InvalidFileName =
            $"Wrong file extension. Allowed extensions: {AppConstants.AllowedExtensions}";
    }
}