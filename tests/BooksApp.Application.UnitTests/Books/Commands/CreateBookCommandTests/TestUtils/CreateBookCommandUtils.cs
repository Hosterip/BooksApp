using PostsApp.Application.Books.Commands.CreateBook;

namespace Application.UnitTest.Books.Commands.CreateBookCommandTests.TestUtils;

public static class CreateBookCommandUtils
{
    public static CreateBookCommand CreateBookCommandMethod() =>
        new CreateBookCommand
        {
            UserId = new Guid("1"), Title = "1", Description = "1", ImageName = "1", GenreIds = [Guid.NewGuid(), Guid.NewGuid()]
        };
}