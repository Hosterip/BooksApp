using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Reviews.Commands.CreateReview;
using PostsApp.Application.Reviews.Commands.DeleteReview;
using PostsApp.Application.Reviews.Commands.UpdateReview;
using PostsApp.Application.Reviews.Queries.GetReviews;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Review;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

public static class ReviewEndpoints
{
    public static void MapReviewEndpoints(this IEndpointRouteBuilder app)
    {
        var reviews = app.MapGroup("api/reviews");
        reviews.MapGet("many/{id:guid}", GetMany);

        reviews.MapPost("", Create)
            .RequireAuthorization(Policies.Authorized);

        reviews.MapPut("", Update)
            .RequireAuthorization(Policies.Authorized);

        reviews.MapDelete("{id:guid}", Delete)
            .RequireAuthorization(Policies.Authorized);
    }
    
    public static async Task<IResult> GetMany(
        Guid id,
        int? page,
        int? pageSize,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetReviewsQuery
        {
            BookId = id,
            Page = page ?? 1,
            PageSize = pageSize ?? 10
        };
        var result = await sender.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public static async Task<IResult> Create(
        CreateReviewRequest request,
        HttpContext httpContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        CreateReviewCommand command = new CreateReviewCommand
        {
            BookId = request.BookId,
            UserId = new Guid(httpContext.GetId()!),
            Body = request.Body,
            Rating = request.Rating
        };
        var result = await sender.Send(command, cancellationToken);
        return Results.Ok(result);
    }
    
    public static async Task<IResult> Update(
        UpdateReviewRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new UpdateReviewCommand
        {
            ReviewId = request.ReviewId,
            UserId = new Guid(httpContext.GetId()!),
            Body = request.Body,
            Rating = request.Rating
        };
        var result = await sender.Send(command, cancellationToken);
        return Results.Ok(result);
    }
    
    public static async Task<IResult> Delete(
        Guid id,
        HttpContext httpContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new DeleteReviewCommand
        {
            ReviewId = id,
            UserId = new Guid(httpContext.GetId()!),
        };
        await sender.Send(command, cancellationToken);
        return Results.Ok("Review was successfully deleted");
    }
    
    
}