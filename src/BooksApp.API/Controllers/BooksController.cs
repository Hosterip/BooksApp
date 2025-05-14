using BooksApp.API.Common.Constants;
using BooksApp.Application.Books.Commands.CreateBook;
using BooksApp.Application.Books.Commands.DeleteBook;
using BooksApp.Application.Books.Commands.PrivilegedDeleteBook;
using BooksApp.Application.Books.Commands.UpdateBook;
using BooksApp.Application.Books.Queries.GetBooks;
using BooksApp.Application.Books.Queries.GetSingleBook;
using BooksApp.Contracts.Books;
using BooksApp.Contracts.Errors;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class BooksController(
    ISender sender,
    IOutputCacheStore outputCacheStore,
    IMapper mapster)
    : ApiController
{
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
        var query = new GetBooksQuery
        {
            Title = request.Title,
            Limit = request.PageSize,
            Page = request.Page,
            GenreId = request.GenreId,
            UserId = userId
        };
        var result = await sender.Send(query, cancellationToken);

        var response = mapster.Map<BooksResponse>(result);

        return Ok(response);
    }

    #endregion Users endpoints

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
        var result = await sender.Send(query, cancellationToken);

        var response = mapster.Map<BooksResponse>(result);

        return Ok(response);
    }

    [HttpGet(ApiRoutes.Books.GetSingle)]
    [OutputCache(PolicyName = OutputCache.Books.PolicyName)]
    [ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BookResponse>> GetSingle(
        [FromRoute] Guid bookId,
        CancellationToken cancellationToken)
    {
        var query = new GetSingleBookQuery { Id = bookId };
        var book = await sender.Send(query, cancellationToken);

        var response = mapster.Map<BookResponse>(book);

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
            Title = request.Title,
            Description = request.Description,
            Image = request.Cover,
            GenreIds = request.GenreIds
        };
        var book = await sender.Send(createBookCommand, cancellationToken);

        await outputCacheStore.EvictByTagAsync(OutputCache.Books.Tag, cancellationToken);

        var response = mapster.Map<BookResponse>(book);

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
            Title = request.Title,
            Description = request.Description,
            Image = request.Cover,
            GenreIds = request.GenreIds
        };

        var result = await sender.Send(updateBookCommand, cancellationToken);

        await outputCacheStore.EvictByTagAsync(OutputCache.Books.Tag, cancellationToken);

        var response = mapster.Map<BookResponse>(result);

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
        [FromRoute] Guid bookId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBookCommand { Id = bookId };

        await sender.Send(command, cancellationToken);

        await outputCacheStore.EvictByTagAsync(OutputCache.Books.Tag, cancellationToken);

        return NoContent();
    }

    [HttpDelete(ApiRoutes.Books.PrivilegedDelete)]
    [Authorize(Policies.Admin)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PrivilegedDelete(
        Guid bookId,
        CancellationToken cancellationToken)
    {
        var command = new PrivilegedDeleteBookCommand
        {
            Id = bookId
        };
        await sender.Send(command, cancellationToken);

        await outputCacheStore.EvictByTagAsync(OutputCache.Books.Tag, cancellationToken);

        return NoContent();
    }

    #endregion Delete endpoints

    #endregion Books Endpoints
}