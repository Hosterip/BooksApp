using BooksApp.Domain.Common.Models;

namespace BooksApp.Domain.Image.ValueObjects;

public class ImageId : ValueObject
{
    public ImageId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ImageId CreateImageId(Guid? value = null)
    {
        return new ImageId(value ?? Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}