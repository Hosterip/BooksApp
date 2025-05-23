using BooksApp.API.Common.Constants;
using BooksApp.Application.Bookshelves;
using BooksApp.Application.Bookshelves.Commands.AddBook;
using BooksApp.Application.Bookshelves.Commands.AddBookByName;
using BooksApp.Application.Bookshelves.Commands.CreateBookshelf;
using BooksApp.Application.Bookshelves.Commands.DeleteBookshelf;
using BooksApp.Application.Bookshelves.Commands.RemoveBook;
using BooksApp.Application.Bookshelves.Commands.RemoveBookByName;
using BooksApp.Application.Bookshelves.Commands.UpdateBookshelfName;
using BooksApp.Application.Bookshelves.Queries.BookshelfById;
using BooksApp.Application.Bookshelves.Queries.BookshelfByName;
using BooksApp.Application.Bookshelves.Queries.GetBookshelfBooks;
using BooksApp.Application.Bookshelves.Queries.GetBookshelves;
using BooksApp.Application.Common.Interfaces;
using BooksApp.Contracts.Books;
using BooksApp.Contracts.Bookshelves;
using BooksApp.Contracts.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class BookshelvesController(
    ISender sender,
    IOutputCacheStore outputCacheStore,
    IMapper mapster,
    IUserService userService)
    : ApiController
{
    #region Bookshelves endpoints

    [HttpGet(ApiRoutes.Bookshelves.GetBooks)]
    [OutputCache(PolicyName = OutputCache.Bookshelves.PolicyName)]
    [ProducesResponseType(typeof(BooksResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BooksResponse>> GetBooks(
        [FromRoute] Guid bookshelfId,
        [FromQuery] GetBookshelfBooksRequest request,
        CancellationToken token)
    {
        var query = new GetBookshelfBooksQuery
        {
            BookshelfId = bookshelfId,
            Limit = request.PageSize,
            Page = request.Page
        };
        var result = await sender.Send(query, token);

        var response = mapster.Map<BooksResponse>(result);

        return Ok(response);
    }

    [HttpPost(ApiRoutes.Bookshelves.Create)]
    [Authorize]
    [ProducesResponseType(typeof(BookshelfResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookshelfResponse>> Create(
        [FromBodyOrDefault] CreateBookshelfRequest request,
        CancellationToken token)
    {
        var userId = userService.GetId()!.Value;
        var command = new CreateBookshelfCommand
        {
            Name = request.Name
        };
        var result = await sender.Send(command, token);

        await outputCacheStore.EvictByTagAsync(OutputCache.Bookshelves.Tag, token);

        var response = mapster.Map<BookshelfResponse>(result);

        return CreatedAtAction(
            nameof(GetBookshelf),
            new { idOrName = result.Id, userId },
            response);
    }

    [HttpPut(ApiRoutes.Bookshelves.Update)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookshelfResponse>> UpdateName(
        Guid bookshelfId,
        string newName,
        CancellationToken token)
    {
        var userId = userService.GetId()!.Value;
        var command = new UpdateBookshelfNameCommand
        {
            NewName = newName,
            BookshelfId = bookshelfId
        };
        await sender.Send(command, token);

        await outputCacheStore.EvictByTagAsync(OutputCache.Bookshelves.Tag, token);

        return CreatedAtAction(
            nameof(GetBookshelf),
            new { idOrName = bookshelfId, userId });
    }


    [HttpDelete(ApiRoutes.Bookshelves.Remove)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Remove(
        [FromRoute] Guid bookshelfId,
        CancellationToken token)
    {
        var command = new DeleteBookshelfCommand
        {
            BookshelfId = bookshelfId
        };

        await sender.Send(command, token);

        await outputCacheStore.EvictByTagAsync(OutputCache.Bookshelves.Tag, token);

        return NoContent();
    }

    #endregion Bookshelves endpoints

    #region Books endpoints

    [HttpPost(ApiRoutes.Books.AddBook)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddBook(
        [FromRoute] Guid bookId,
        [FromRoute] string idOrName,
        CancellationToken token)
    {
        var success = Guid.TryParse(idOrName, out var bookshelfId);
        await sender.Send(success
            ? new AddBookCommand
            {
                BookshelfId = bookshelfId,
                BookId = bookId
            }
            : new AddBookByNameCommand
            {
                BookshelfName = idOrName,
                BookId = bookId
            }, token);

        await outputCacheStore.EvictByTagAsync(OutputCache.Bookshelves.Tag, token);

        return NoContent();
    }

    [HttpDelete(ApiRoutes.Books.RemoveBook)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveBook(
        [FromRoute] Guid bookId,
        [FromRoute] string idOrName,
        CancellationToken token)
    {
        var success = Guid.TryParse(idOrName, out var bookshelfId);
        await sender.Send(success
            ? new RemoveBookCommand
            {
                BookshelfId = bookshelfId,
                BookId = bookId
            }
            : new RemoveBookByNameCommand
            {
                BookshelfName = idOrName,
                BookId = bookId
            }, token);

        await outputCacheStore.EvictByTagAsync(OutputCache.Bookshelves.Tag, token);

        return NoContent();
    }

    #endregion Books endpoints

    #region Users endpoints

    [HttpGet(ApiRoutes.Users.GetBookshelves)]
    [OutputCache(PolicyName = OutputCache.Bookshelves.PolicyName)]
    [ProducesResponseType(typeof(IEnumerable<BookshelfResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<BookshelfResponse>>> GetBookshelves(
        [FromRoute] Guid userId,
        CancellationToken token)
    {
        var query = new GetBookshelvesQuery
        {
            UserId = userId
        };
        var result = await sender.Send(query, token);

        var response = mapster.Map<IEnumerable<BookshelfResponse>>(result);

        return Ok(response);
    }

    [HttpGet(ApiRoutes.Users.GetBookshelf)]
    [OutputCache(PolicyName = OutputCache.Bookshelves.PolicyName)]
    [ProducesResponseType(typeof(BookshelfResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookshelfResponse>> GetBookshelf(
        [FromRoute] string idOrName,
        [FromRoute] Guid userId,
        CancellationToken token
    )
    {
        var success = Guid.TryParse(idOrName, out var bookshelfId);
        var result = await sender.Send(success
                ? new BookshelfByIdQuery
                {
                    BookshelfId = bookshelfId
                }
                : new BookshelfByNameQuery
                {
                    UserId = userId,
                    Name = idOrName
                },
            token
        );

        var response = mapster.Map<BookshelfResponse>(result!);

        return Ok(response);
    }

    #endregion Users endpoints
}