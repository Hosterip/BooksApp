using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Books.Results;
using BooksApp.Application.Bookshelves;
using BooksApp.Application.Bookshelves.Commands.AddBook;
using BooksApp.Application.Bookshelves.Commands.AddBookByName;
using BooksApp.Application.Bookshelves.Commands.CreateBookshelf;
using BooksApp.Application.Bookshelves.Commands.DeleteBookshelf;
using BooksApp.Application.Bookshelves.Commands.RemoveBook;
using BooksApp.Application.Bookshelves.Commands.RemoveBookByName;
using BooksApp.Application.Bookshelves.Queries.BookshelfById;
using BooksApp.Application.Bookshelves.Queries.BookshelfByName;
using BooksApp.Application.Bookshelves.Queries.GetBookshelfBooks;
using BooksApp.Application.Bookshelves.Queries.GetBookshelves;
using BooksApp.Application.Common.Results;
using BooksApp.Contracts.Bookshelves;
using BooksApp.Contracts.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class BookshelvesController : ApiController
{
    private readonly ISender _sender;
    private readonly IOutputCacheStore _outputCacheStore;
    private readonly IMapper _mapster;
    
    public BookshelvesController(ISender sender, IOutputCacheStore outputCacheStore, IMapper mapster)
    {
        _sender = sender;
        _outputCacheStore = outputCacheStore;
        _mapster = mapster;
    }

    #region Bookshelves endpoints
    
    [HttpGet(ApiRoutes.Bookshelves.GetBooks)]
    [OutputCache(PolicyName = OutputCache.Bookshelves.PolicyName)]
    [ProducesResponseType(typeof(PaginatedArray<BookResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedArray<BookResult>>> GetBooks(
        [FromRoute] Guid bookshelfId,
        [FromQuery] GetBookshelfBooksRequest request,
        CancellationToken token)
    {
        var userId = HttpContext.GetId();
        var query = new GetBookshelfBooksQuery
        {
            CurrentUserId = userId,
            BookshelfId = bookshelfId,
            Limit = request.PageSize,
            Page = request.Page
        };
        var result = await _sender.Send(query, token);

        return Ok(result);
    }

    [HttpPost(ApiRoutes.Bookshelves.Create)]
    [Authorize]
    [ProducesResponseType(typeof(BookshelfResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookshelfResult>> Create(
        [FromBodyOrDefault] CreateBookshelfRequest request,
        CancellationToken token)
    {
        var userId = HttpContext.GetId()!.Value;
        var command = new CreateBookshelfCommand
        {
            Name = request.Name,
            UserId = userId
        };
        var result = await _sender.Send(command, token);

        await _outputCacheStore.EvictByTagAsync(OutputCache.Bookshelves.Tag, token);
        
        return CreatedAtAction(
            nameof(GetBookshelf),
            new { nameOrGuid = result.Id, userId },
            result);
    }
    
    [HttpDelete(ApiRoutes.Bookshelves.Remove)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Remove(
        [FromRoute] Guid bookshelfId,
        CancellationToken token)
    {
        var userId = HttpContext.GetId();
        var command = new DeleteBookshelfCommand
        {
            BookshelfId = bookshelfId,
            UserId = userId!.Value
        };

        await _sender.Send(command, token);

        await _outputCacheStore.EvictByTagAsync(OutputCache.Bookshelves.Tag, token);
        
        return Ok();
    }
    
    #endregion Bookshelves endpoints

    #region Books endpoints

    [HttpPost(ApiRoutes.Books.AddBook)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddBook(
        [FromRoute] Guid bookId,
        [FromRoute] string idOrName,
        CancellationToken token)
    {
        var userId = HttpContext.GetId()!.Value;
        var success = Guid.TryParse(idOrName, out var bookshelfId);
        await _sender.Send(success
            ? new AddBookCommand
            {
                BookshelfId = bookshelfId,
                BookId = bookId,
                UserId = userId
            }
            : new AddBookByNameCommand
            {
                BookshelfName = idOrName,
                BookId = bookId,
                UserId = userId
            }, token);

        await _outputCacheStore.EvictByTagAsync(OutputCache.Bookshelves.Tag, token);

        return Ok();
    }

    [HttpDelete(ApiRoutes.Books.RemoveBook)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveBook(
        [FromRoute] Guid bookId,
        [FromRoute] string idOrName,
        CancellationToken token)
    {
        var userId = HttpContext.GetId()!.Value;
        var success = Guid.TryParse(idOrName, out var bookshelfId);
        await _sender.Send(success
            ? new RemoveBookCommand
            {
                BookshelfId = bookshelfId,
                BookId = bookId,
                UserId = userId
            }
            : new RemoveBookByNameCommand
            {
                BookshelfName = idOrName,
                BookId = bookId,
                UserId = userId
            }, token);
        
        await _outputCacheStore.EvictByTagAsync(OutputCache.Bookshelves.Tag, token);

        return Ok();
    }
    
    #endregion Books endpoints

    #region Users endpoints 

    [HttpGet(ApiRoutes.Users.GetBookshelves)]
    [OutputCache(PolicyName = OutputCache.Bookshelves.PolicyName)]
    [ProducesResponseType(typeof(IEnumerable<BookshelfResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<BookshelfResult>>> GetBookshelves(
        Guid userId,
        CancellationToken token)
    {
        var query = new GetBookshelvesQuery
        {
            UserId = userId
        };
        var result = await _sender.Send(query, token);

        var response = _mapster.Map<BookshelfResponse>(result);
        
        return Ok(response);
    }
    
    [HttpGet(ApiRoutes.Users.GetBookshelf)]
    [OutputCache(PolicyName = OutputCache.Bookshelves.PolicyName)]
    [ProducesResponseType(typeof(BookshelfResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookshelfResult>> GetBookshelf(
        [FromRoute] string idOrName,
        [FromRoute] Guid userId,
        CancellationToken token
    )
    {
        var success = Guid.TryParse(idOrName, out var bookshelfId);
        var result = await _sender.Send(success
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
        return Ok(result);
    }
    
    #endregion Users endpoints 
}