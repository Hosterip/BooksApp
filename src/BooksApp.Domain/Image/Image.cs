using BooksApp.Domain.Common.Models;
using BooksApp.Domain.Image.ValueObjects;

namespace BooksApp.Domain.Image;

public class Image : AggregateRoot<ImageId>
{
    private Image(ImageId id) : base(id)
    {
    }

    private Image(ImageId id, string imageName) : base(id)
    {
        ImageName = imageName;
    }

    public string ImageName { get; private set; }

    public static Image Create(string imageName)
    {
        return new Image(ImageId.CreateImageId(), imageName);
    }

    public void ChangeImageName(string imageName)
    {
        ImageName = imageName;
    }
}