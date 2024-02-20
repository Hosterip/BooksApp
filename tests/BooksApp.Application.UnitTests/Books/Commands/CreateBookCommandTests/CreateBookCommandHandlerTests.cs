using System.Linq.Expressions;
using Application.UnitTest.MockData;
using FluentAssertions;
using Moq;
using PostsApp.Application.Books.Commands.CreateBook;
using PostsApp.Application.Books.Results;
using PostsApp.Application.Common.Interfaces;
using PostsApp.Domain.Models;

namespace Application.UnitTest.Books.Commands.CreateBookCommandTests;

public class CreateBookCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    public CreateBookCommandHandlerTests()
    {
        _unitOfWorkMock = new();
    }
    [Fact]
    public async Task Handle_Should_ReturnBook_Success()
    {
        // Arrange
        var command = new CreateBookCommand { UserId = 1, Title = "1984", Description = "hello world" };
        var handler = new CreateBookCommandHandler(_unitOfWorkMock.Object);

        _unitOfWorkMock.Setup(x => x.Users.GetSingleWhereAsync(
            It.IsAny<Expression<Func<User, bool>>>()))
            .ReturnsAsync(MockUser.GetUser(null));
        _unitOfWorkMock.Setup(x => x.Posts.AddAsync(
                It.IsAny<Book>()));
            
        // Act
        var result = await handler.Handle(command, default);
        
        // Assert
        result.Should().BeOfType<BookResult>();
    }
}