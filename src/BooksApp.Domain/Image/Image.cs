using PostsApp.Domain.Common.Models;
using PostsApp.Domain.Image.ValueObjects;

namespace PostsApp.Domain.Image;

public class Image : AggregateRoot<ImageId>
{
    private Image(ImageId id) : base(id)
    {
    }

    private Image(ImageId id, string imageName) : base(id)
    {
        ImageName = imageName;
    }

    public string ImageName { get; set; }

    public static Image Create(string imageName)
    {
        return new Image(ImageId.CreateImageId(), imageName);
    }
}