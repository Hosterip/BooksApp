using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.Application.Common.Results;
using BooksApp.Application.Reviews.Commands.CreateReview;
using BooksApp.Application.Reviews.Commands.DeleteReview;
using BooksApp.Application.Reviews.Commands.PrivilegedDeleteReview;
using BooksApp.Application.Reviews.Commands.UpdateReview;
using BooksApp.Application.Reviews.Queries.GetReviews;
using BooksApp.Application.Reviews.Results;
using BooksApp.Contracts.Errors;
using BooksApp.Contracts.Reviews;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace BooksApp.API.Controllers;

public class ReviewsController(
    ISender sender,
    IOutputCacheStore outputCacheStore,
    IMapper mapster)
    : ApiController
{
    #region Reviews endpoints
    
    [HttpPost(ApiRoutes.Reviews.Create)]
    [Authorize]
    [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewResponse>> Create(
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
        
        var result = await sender.Send(command, cancellationToken);

        await outputCacheStore.EvictByTagAsync(OutputCache.Reviews.Tag, cancellationToken);

        var response = mapster.Map<ReviewResponse>(result);
        
        return Ok(response);
    }

    [HttpPut(ApiRoutes.Reviews.Update)]
    [Authorize]
    [ProducesResponseType(typeof(ReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewResponse>> Update(
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
        
        var result = await sender.Send(command, cancellationToken);
        
        await outputCacheStore.EvictByTagAsync(OutputCache.Reviews.Tag, cancellationToken);
        
        var response = mapster.Map<ReviewResponse>(result);
        
        return Ok(response);
    }

    #region Delete endpoints
    
    [HttpDelete(ApiRoutes.Reviews.Delete)]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        
        await sender.Send(command, cancellationToken);
        
        await outputCacheStore.EvictByTagAsync(OutputCache.Reviews.Tag, cancellationToken);
        
        return NoContent();
    }

    [HttpDelete(ApiRoutes.Reviews.PrivilegedDelete)]
    [Authorize(Policies.Moderator)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        
        await sender.Send(command, cancellationToken);
        
        await outputCacheStore.EvictByTagAsync(OutputCache.Reviews.Tag, cancellationToken);
        
        return NoContent();
    }
    
    #endregion Delete endpoints

    #endregion Reviews endpoints
    
    #region Books Endpoints

    [HttpGet(ApiRoutes.Books.GetReviews)]
    [OutputCache(PolicyName = OutputCache.Reviews.PolicyName)]
    [ProducesResponseType(typeof(ReviewsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationFailureResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReviewsResponse>> GetReviews(
        [FromRoute] Guid bookId,
        [FromQuery] GetReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetId();
        var query = new GetReviewsQuery
        {
            CurrentUserId = userId,
            BookId = bookId,
            Page = request.Page,
            Limit = request.Limit
        };
        var result = await sender.Send(query, cancellationToken);
        
        var response = mapster.Map<ReviewsResponse>(result);
        
        return Ok(response);
    }
    
    #endregion Books Endpoints
}