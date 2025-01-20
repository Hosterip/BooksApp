using System.Text;
using BooksApp.Domain.Image;
using Microsoft.AspNetCore.Http;
using TestCommon.Common.Constants;

namespace TestCommon.Images;

public static class ImageFactory
{
    public static Image CreateImage(string imageName = Constants.Images.ImageName)
    {
        return Image.Create(imageName);
    }

    public static IFormFile CreateFormFileImage()
    {
        var bytes = Encoding.UTF8.GetBytes("foo");

        return new FormFile(new MemoryStream(bytes), 0, bytes.Length, "Data", "foo.png");
    }
}