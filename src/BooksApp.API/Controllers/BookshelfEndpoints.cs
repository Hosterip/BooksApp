using MediatR;
using PostsApp.Application.Bookshelves.Commands.AddBook;
using PostsApp.Application.Bookshelves.Commands.AddBookToDefaultBookshelf;
using PostsApp.Application.Bookshelves.Commands.CreateBookshelf;
using PostsApp.Application.Bookshelves.Commands.DeleteBookshelf;
using PostsApp.Application.Bookshelves.Commands.RemoveBook;
using PostsApp.Application.Bookshelves.Commands.RemoveBookFromDefaultBookshelf;
using PostsApp.Application.Bookshelves.Queries.GetBookshelfBooks;
using PostsApp.Application.Bookshelves.Queries.GetBookshelves;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Bookshelf;
using PostsApp.Common.Extensions;
using Toycloud.AspNetCore.Mvc.ModelBinding;

namespace PostsApp.Controllers;

public static class BookshelfEndpoints
{
    public static void MapBookshelfEndpoints(this IEndpointRouteBuilder app)
    {
        var bookshelves = app.MapGroup("api/bookshelves");

        bookshelves.MapGet("{userId:guid}", GetBookshelves);
        bookshelves.MapGet("books/{bookshelfId:guid}", GetBooks);

        bookshelves.MapPost("{name}", Create)
            .RequireAuthorization(Policies.Authorized);
        bookshelves.MapPost("addBook", AddBook)
            .RequireAuthorization(Policies.Authorized);
        bookshelves.MapPost("addBookToDefault", AddBookToDefault)
            .RequireAuthorization(Policies.Authorized);

        bookshelves.MapDelete("removeBook", RemoveBook)
            .RequireAuthorization(Policies.Authorized);
        bookshelves.MapDelete("removeBookFromDefault", RemoveBookFromDefault)
            .RequireAuthorization(Policies.Authorized);
        bookshelves.MapDelete("{bookshelfId:guid}", Remove)
            .RequireAuthorization(Policies.Authorized);
    }
    
    public static async Task<IResult> GetBookshelves(
        Guid userId,
        ISender sender)
    {
        var query = new GetBookshelvesQuery
        {
            UserId = userId
        };
        var result = await sender.Send(query);

        return Results.Ok(result);
    }
    
    public static async Task<IResult> GetBooks(
        Guid bookshelfId,
        int? limit,
        int? page,
        ISender sender)
    {
        var query = new GetBookshelfBooksQuery
        {
            BookshelfId = bookshelfId,
            Limit = limit,
            Page = page
        };
        var result = await sender.Send(query);

        return Results.Ok(result);
    }
    
    public static async Task<IResult> Create(
        string name,
        ISender sender,
        HttpContext httpContext)
    {
        var command = new CreateBookshelfCommand
        {
            Name = name,
            UserId = Guid.Parse(httpContext.GetId()!)
        };
        var result = await sender.Send(command);

        return Results.Ok(result);
    }
    
    public static async Task<IResult> AddBook(
        AddRemoveBookBookshelfRequest request,
        ISender sender,
        HttpContext httpContext)
    {
        var command = new AddBookCommand
        {
            BookshelfId = request.BookshelfId,
            BookId = request.BookId,
            UserId = Guid.Parse(httpContext.GetId()!)
        };
        await sender.Send(command);

        return Results.Ok("Book was added successfully!");
    }
    
    public static async Task<IResult> AddBookToDefault(
        AddRemoveBookToDefaultBookshelfRequest request,
        ISender sender,
        HttpContext httpContext)
    {
        var command = new AddBookToDefaultBookshelfCommand
        {
            BookshelfName = request.BookshelfName,
            BookId = request.BookId,
            UserId = Guid.Parse(httpContext.GetId()!)
        };
        await sender.Send(command);

        return Results.Ok("Book was added successfully!");
    }
    
    public static async Task<IResult> RemoveBook(
        AddRemoveBookBookshelfRequest request,
        ISender sender,
        HttpContext httpContext)
    {
        var command = new RemoveBookCommand
        {
            BookshelfId = request.BookshelfId,
            BookId = request.BookId,
            UserId = Guid.Parse(httpContext.GetId()!)
        };
        await sender.Send(command);

        return Results.Ok("Book was deleted successfully!");
    }
    
    public static async Task<IResult> RemoveBookFromDefault(
        AddRemoveBookToDefaultBookshelfRequest request,
        ISender sender,
        HttpContext httpContext)
    {
        var command = new RemoveBookFromDefaultBookshelfCommand
        {
            BookshelfName = request.BookshelfName,
            BookId = request.BookId,
            UserId = Guid.Parse(httpContext.GetId()!)
        };
        await sender.Send(command);

        return Results.Ok("Book was deleted successfully!");
    }

    public static async Task<IResult> Remove(
        Guid bookshelfId,
        ISender sender)
    {
        var command = new DeleteBookshelfCommand
        {
            BookshelfId = bookshelfId
        };

        await sender.Send(command);
        
        return Results.Ok("Bookshelf deleted");
    }
}