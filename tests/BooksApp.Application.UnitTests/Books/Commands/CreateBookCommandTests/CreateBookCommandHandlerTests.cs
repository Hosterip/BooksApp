using System.Linq.Expressions;
using Application.UnitTest.Books.Commands.CreateBookCommandTests.TestUtils;
using Application.UnitTest.TestUtils.MockData;
using FluentAssertions;
using Moq;
using PostsApp.Application.Books.Commands.CreateBook;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Book;
using PostsApp.Domain.User;

namespace Application.UnitTest.Books.Commands.CreateBookCommandTests;

public class CreateBookCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    public CreateBookCommandHandlerTests()
    {
        _unitOfWorkMock = new();
    }
    [Fact]
    public async Task Handle_Success_ReturnBook()
    {
        // Arrange
        var command = CreateBookCommandUtils.CreateBookCommandMethod();
        var handler = new CreateBookCommandHandler(_unitOfWorkMock.Object);

        _unitOfWorkMock.Setup(x => x.Users.GetSingleWhereAsync(
            It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(MockUser.GetUser());
        _unitOfWorkMock.Setup(x => x.Books.AddAsync(
                It.IsAny<Book>()));
            
        // Act
        var result = await handler.Handle(command, default);
        
        // Assert
        result.Should().BeOfType<BookResult>();
    }
}