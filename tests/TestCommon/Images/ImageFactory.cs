using BooksApp.Domain.Image;
using TestCommon.Common.Constants;

namespace TestCommon.Images;

public static class ImageFactory
{
    public static Image CreateImage(string imageName = Constants.Images.ImageName)
    {
        return Image.Create(imageName);
    }
}