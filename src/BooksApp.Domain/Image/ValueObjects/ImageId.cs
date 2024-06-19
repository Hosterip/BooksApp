using PostsApp.Domain.Common.Models;

namespace PostsApp.Domain.Image.ValueObjects;

public class ImageId : ValueObject
{
    public Guid Value { get; }

    public ImageId(Guid value)
    {
        Value = value;
    }

    public static ImageId CreateImageId(Guid? value = null)
    {
        return new(value ?? Guid.NewGuid());
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}