using MediatR;
using Microsoft.AspNetCore.Mvc;
using PostsApp.Application.Books.Commands.CreateBook;
using PostsApp.Application.Books.Commands.DeleteBook;
using PostsApp.Application.Books.Commands.UpdateBook;
using PostsApp.Application.Books.Queries.GetBooks;
using PostsApp.Application.Books.Queries.GetSingleBook;
using PostsApp.Application.Bookshelves.Commands.AddBook;
using PostsApp.Application.Bookshelves.Commands.AddBookToDefaultBookshelf;
using PostsApp.Application.Bookshelves.Commands.RemoveBook;
using PostsApp.Application.Bookshelves.Commands.RemoveBookFromDefaultBookshelf;
using PostsApp.Application.Images.Commands.CreateImage;
using PostsApp.Application.Images.Commands.DeleteImage;
using PostsApp.Common.Constants;
using PostsApp.Common.Contracts.Requests.Book;
using PostsApp.Common.Contracts.Requests.Bookshelf;
using PostsApp.Common.Extensions;

namespace PostsApp.Controllers;

public static class BookEndpoints
{
    public static void MapBookEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet(ApiEndpoints.Books.GetMany, GetMany);
        app.MapGet(ApiEndpoints.Books.GetSingle, GetSingle);
        
        app.MapPost(ApiEndpoints.Books.Create, Create)
            .RequireAuthorization(Policies.Author);
        
        app.MapPut(ApiEndpoints.Books.Update, Update)
            .RequireAuthorization(Policies.Authorized);

        app.MapDelete(ApiEndpoints.Books.Delete, Delete)
            .RequireAuthorization(Policies.Authorized);
        
        // Bookshelves logic
        app.MapPost(ApiEndpoints.Books.AddBook, AddBook)
            .RequireAuthorization(Policies.Authorized);
        app.MapPost(ApiEndpoints.Books.AddBookToDefault, AddBookToDefault)
            .RequireAuthorization(Policies.Authorized);

        app.MapDelete(ApiEndpoints.Books.RemoveBook, RemoveBook)
            .RequireAuthorization(Policies.Authorized);
        app.MapDelete(ApiEndpoints.Books.RemoveBookFromDefault, RemoveBookFromDefault)
            .RequireAuthorization(Policies.Authorized);
    } 
    
    public static async Task<IResult> Create(
        BookRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken
        )
    {
        var imageCommand = new CreateImageCommand
        {
            Image = request.Cover
        };
        var fileName = await sender.Send(imageCommand, cancellationToken);
        
        var createBookCommand = new CreateBookCommand
        {
            UserId = new Guid(httpContext.GetId()!), 
            Title = request.Title,
            Description = request.Description,
            ImageName = fileName,
            GenreIds = request.GenreIds
        };
        var book = await sender.Send(createBookCommand, cancellationToken);

        return Results.Ok(book);
    }

    public static async Task<IResult> Update(
        UpdateBookRequest request,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        string? fileName = null;
        if (request.Cover is not null)
        {
            var bookQuery = new GetSingleBookQuery
            {
                Id = request.Id
            };
            var book = await sender.Send(bookQuery, cancellationToken);
            var deleteImageCommand = new DeleteImageCommand
            {
                ImageName = book.CoverName
            };
            await sender.Send(deleteImageCommand, cancellationToken);
            var imageCommand = new CreateImageCommand
            {
                Image = request.Cover
            };
            fileName = await sender.Send(imageCommand, cancellationToken);
        }
        
        var updateBookCommand = new UpdateBookCommand
        {
            Id = request.Id, 
            UserId = new Guid(httpContext.GetId()!),
            Title = request.Title, 
            Description = request.Description,
            ImageName = fileName ?? null,
            GenreIds = request.GenreIds
        };
        var result = await sender.Send(updateBookCommand, cancellationToken);
        return Results.Ok(result);
    }

    public static async Task<IResult> Delete(
        Guid id,
        ISender sender,
        HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        var command = new DeleteBookCommand { Id = id, UserId = new Guid(httpContext.GetId()!) };
        await sender.Send(command, cancellationToken);
        return Results.Ok();
    }

    public static async Task<IResult> GetMany(
        CancellationToken cancellationToken,
        ISender sender,
        [FromQuery] int? page,
        [FromQuery] int? limit,
        [FromQuery] string? q,
        [FromQuery] Guid? userId,
        [FromQuery] Guid? genreId
        )
    {
        var query = new GetBooksQuery { Query = q, Limit = limit, Page = page, UserId = userId, GenreId = genreId};
        var result = await sender.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    public static async Task<IResult> GetSingle(
        Guid id,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new GetSingleBookQuery { Id = id };
        var post = await sender.Send(query, cancellationToken);
        return Results.Ok(post);
    }
    
    // Bookshelves logic
    
    public static async Task<IResult> AddBook(
        AddBookToBookshelfRequest request,
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
        AddBookToDefaultBookshelfRequest request,
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
        RemoveBookFromBookshelfRequest request,
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
        RemoveBookFromDefaultBookshelfRequest request,
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
}