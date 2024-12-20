using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Books.Commands.CreateBook;
using BooksApp.Application.Books.Commands.DeleteBook;
using BooksApp.Application.Books.Commands.PrivilegedDeleteBook;
using BooksApp.Application.Books.Commands.UpdateBook;
using BooksApp.Application.Books.Queries.GetBooks;
using BooksApp.Application.Books.Queries.GetSingleBook;
using BooksApp.Application.Books.Results;
using BooksApp.Application.Bookshelves;
using BooksApp.Application.Common.Results;
using BooksApp.Contracts.Books;
using BooksApp.Contracts.Errors;
using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class BooksController : ApiController
{
    private readonly ISender _sender;
    private readonly IMapper _mapster;
    private readonly IOutputCacheStore _outputCacheStore;

    public BooksController(ISender sender, IOutputCacheStore outputCacheStore, IMapper mapster)
    {
        _sender = sender;
        _outputCacheStore = outputCacheStore;
        _mapster = mapster;
    }

    #region Books Endpoints
    
    #region Get endpoints
    
    [HttpGet(ApiRoutes.Books.GetMany)]
    [OutputCache(PolicyName = OutputCache.Books.PolicyName)]
    [ProducesResponseType(typeof(BooksResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BooksResponse>> GetMany(
        CancellationToken cancellationToken,
        [FromQuery] GetBooksRequest request
    )
    {
        var query = new GetBooksQuery
        {
            Title = request.Title,
            Limit = request.PageSize,
            Page = request.Page,
            GenreId = request.GenreId,
            UserId = null
        };
        var result = await _sender.Send(query, cancellationToken);
        
        var response = _mapster.Map<BooksResponse>(result);
        
        return Ok(response);
    }

    [HttpGet(ApiRoutes.Books.GetSingle)]  
    [OutputCache(PolicyName = OutputCache.Books.PolicyName)]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookResponse>> GetSingle(
        Guid bookId,
        CancellationToken cancellationToken)
    {
        var query = new GetSingleBookQuery { Id = bookId };
        var book = await _sender.Send(query, cancellationToken);

        var response = _mapster.Map<BookResponse>(book);
        
        return Ok(response);
    }
    
    #endregion Get endpoints

    #region Post endpoints

    [HttpPost(ApiRoutes.Books.Create)]
    [Authorize(Policies.Author)]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookResponse>> Create(
        [FromBodyOrDefault] CreateBookRequest request,
        CancellationToken cancellationToken
    )
    {
        var createBookCommand = new CreateBookCommand
        {
            UserId = HttpContext.GetId()!.Value,
            Title = request.Title,
            Description = request.Description,
            Image = request.Cover,
            GenreIds = request.GenreIds
        };
        var book = await _sender.Send(createBookCommand, cancellationToken);

        await _outputCacheStore.EvictByTagAsync(OutputCache.Books.Tag, cancellationToken);
        
        var response = _mapster.Map<BookResponse>(book);
        
        return CreatedAtAction(
            nameof(GetSingle),
            new { bookId = book.Id },
            response
            );
    }
    
    #endregion Post endpoints

    #region Put endpoints
    
    [HttpPut(ApiRoutes.Books.Update)]
    [Authorize]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookResponse>> Update(
        [FromBodyOrDefault] UpdateBookRequest request,
        CancellationToken cancellationToken)
    {
        var updateBookCommand = new UpdateBookCommand
        {
            Id = request.Id,
            UserId = HttpContext.GetId()!.Value,
            Title = request.Title,
            Description = request.Description,
            Image = request.Cover,
            GenreIds = request.GenreIds
        };
        
        var result = await _sender.Send(updateBookCommand, cancellationToken);
        
        await _outputCacheStore.EvictByTagAsync(OutputCache.Books.Tag, cancellationToken);

        var response = _mapster.Map<BookResponse>(result);
        
        return CreatedAtAction(
            nameof(GetSingle),
            new { bookId = result.Id }, 
            response);
    }
    
    #endregion Put endpoints

    #region Delete endpoints
    
    [HttpDelete(ApiRoutes.Books.Delete)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBookCommand { Id = id, UserId = HttpContext.GetId()!.Value };
        
        await _sender.Send(command, cancellationToken);
        
        await _outputCacheStore.EvictByTagAsync(OutputCache.Books.Tag, cancellationToken);
        
        return NoContent();
    }

    [HttpDelete(ApiRoutes.Books.PrivilegedDelete)]
    [Authorize(Policies.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PrivilegedDelete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new PrivilegedDeleteBookCommand
        {
            Id = id,
            UserId = HttpContext.GetId()!.Value
        };
        await _sender.Send(command, cancellationToken);
        
        await _outputCacheStore.EvictByTagAsync(OutputCache.Books.Tag, cancellationToken);
        
        return NoContent();
    }
    
    #endregion Delete endpoints
    
    #endregion Books Endpoints

    #region Users endpoints
    
    [HttpGet(ApiRoutes.Users.GetManyBooks)]
    [OutputCache(PolicyName = OutputCache.Books.PolicyName)]
    [ProducesResponseType(typeof(BooksResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BooksResponse>> GetManyByUserId(
        CancellationToken cancellationToken,
        [FromRoute] Guid userId,
        [FromQuery] GetUserBooksRequest request
    )
    {
        var currentUserId = HttpContext.GetId();
        var query = new GetBooksQuery
        {
            Title = request.Title,
            Limit = request.PageSize,
            Page = request.Page,
            GenreId = request.GenreId,
            UserId = userId,
            CurrentUserId = currentUserId
        };
        var result = await _sender.Send(query, cancellationToken);
        
        var response = _mapster.Map<BooksResponse>(result);
        
        return Ok(response);
    }
    
    #endregion Users endpoints
}