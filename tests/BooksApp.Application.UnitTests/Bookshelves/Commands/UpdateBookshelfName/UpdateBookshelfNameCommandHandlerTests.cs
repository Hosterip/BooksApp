using BooksApp.Application.Bookshelves.Commands.UpdateBookshelfName;
using BooksApp.Application.Common.Interfaces;
using FluentAssertions;
using NSubstitute;
using TestCommon.Bookshelves;

namespace BooksApp.Application.UnitTests.Bookshelves.Commands.UpdateBookshelfName;

public class UpdateBookshelfNameCommandHandlerTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly UpdateBookshelfNameCommandHandler _handler;

    public UpdateBookshelfNameCommandHandlerTests()
    {
        _handler = new UpdateBookshelfNameCommandHandler(_unitOfWork);
    }

    [Fact]
    public void Handle_WhenEverythingIsOkay_ShouldReturnSucceededTask()
    {
        // Arrange
        var bookshelf = BookshelfFactory.CreateBookshelf();
        var command = BookshelfCommandFactory.CreateUpdateBookshelfNameCommand();
        
        _unitOfWork.Bookshelves.GetSingleById(Guid.Empty, default).ReturnsForAnyArgs(bookshelf);
        
        // Act
        var result = _handler.Handle(command, default);

        // Assert
        result.IsCompletedSuccessfully.Should().BeTrue();
    }
}