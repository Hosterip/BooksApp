using PostsApp.Domain.Common.Models;
using PostsApp.Domain.Image.ValueObjects;

namespace PostsApp.Domain.Image;

public class Image : AggregateRoot<ImageId>
{
    public string ImageName { get; set; }

    private Image(ImageId id) : base(id) { }

    private Image(ImageId id, string imageName) : base(id)
    {
        ImageName = imageName;
    }

    public static Image Create(string imageName)
    {
        return new(ImageId.CreateImageId(), imageName);
    }
}