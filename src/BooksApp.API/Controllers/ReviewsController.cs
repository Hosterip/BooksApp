using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Reviews.Commands.CreateReview;
using BooksApp.Application.Reviews.Commands.DeleteReview;
using BooksApp.Application.Reviews.Commands.PrivilegedDeleteReview;
using BooksApp.Application.Reviews.Commands.UpdateReview;
using BooksApp.Application.Reviews.Queries.GetReviews;
using BooksApp.Application.Reviews.Results;
using BooksApp.Contracts.Requests.Reviews;
using BooksApp.Contracts.Responses.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class ReviewsController : ApiController
{
    private readonly ISender _sender;

    public ReviewsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost(ApiRoutes.Reviews.Create)]
    [Authorize]
    [ProducesResponseType(typeof(ReviewResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewResult>> Create(
        [FromBodyOrDefault] CreateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateReviewCommand
        {
            BookId = request.BookId,
            UserId = HttpContext.GetId()!.Value,
            Body = request.Body,
            Rating = request.Rating
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut(ApiRoutes.Reviews.Update)]
    [Authorize]
    [ProducesResponseType(typeof(ReviewResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewResult>> Update(
        [FromBodyOrDefault] UpdateReviewRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateReviewCommand
        {
            ReviewId = request.ReviewId,
            UserId = HttpContext.GetId()!.Value,
            Body = request.Body,
            Rating = request.Rating
        };
        var result = await _sender.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete(ApiRoutes.Reviews.Delete)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteReviewCommand
        {
            ReviewId = id,
            UserId = HttpContext.GetId()!.Value
        };
        await _sender.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete(ApiRoutes.Reviews.PrivilegedDelete)]
    [Authorize(Policies.Moderator)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PrivilegedDelete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var command = new PrivilegedDeleteReviewCommand
        {
            ReviewId = id,
            UserId = HttpContext.GetId()!.Value
        };
        await _sender.Send(command, cancellationToken);
        return Ok();
    }

    // Books

    [HttpGet(ApiRoutes.Books.GetReviews)]
    [ProducesResponseType(typeof(PaginatedArray<ReviewResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedArray<ReviewResult>>> GetReviews(
        Guid id,
        [FromQuery] GetReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetId();
        var query = new GetReviewsQuery
        {
            CurrentUserId = userId,
            BookId = id,
            Page = request.Page,
            Limit = request.Limit
        };
        var result = await _sender.Send(query, cancellationToken);
        return Ok(result);
    }
}