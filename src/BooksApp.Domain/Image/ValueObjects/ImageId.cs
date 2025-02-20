using BooksApp.Domain.Common.Models;

namespace BooksApp.Domain.Image.ValueObjects;

public class ImageId : ValueObject
{
    private ImageId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static ImageId Create(Guid? value = null)
    {
        return new ImageId(value ?? Guid.NewGuid());
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}