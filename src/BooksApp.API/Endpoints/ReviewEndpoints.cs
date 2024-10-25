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

namespace PostsApp.Controllers;

public class ReviewEndpoints : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ApiEndpoints.Reviews.Create, Create)
            .RequireAuthorization(Policies.Authorized);

        app.MapPut(ApiEndpoints.Reviews.Update, Update)
            .RequireAuthorization(Policies.Authorized);

        app.MapDelete(ApiEndpoints.Reviews.Delete, Delete)
            .RequireAuthorization(Policies.Authorized);
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