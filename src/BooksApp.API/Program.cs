using BooksApp.API;
using BooksApp.API.Common.Constants;
using BooksApp.API.Middlewares;
using BooksApp.Application;
using BooksApp.Infrastructure;


// Add CORS
var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding Dependency Injectable 
//  Api
var corsPolicy = config["CorsPolicy"];

builder.Services.AddApi(corsPolicy!);
//  Application

builder.Services.AddApplication();
//  Infra
var imagesPath = config["ImagesPath"];
var dbHost = config["Database:Host"];
var dbName = config["Database:Name"];
var dbPassword = config["Database:Password"];
builder.Services.AddInfrastructure(
    imagesPath!,
    dbHost!,
    dbName!,
    dbPassword!
);

// Building app
var app = builder.Build();
// Route for exceptions
app.UseExceptionHandler(ApiRoutes.Error.ErrorHandler);
if (!app.Environment.IsDevelopment())
    app.UseHsts();
// Swagger
app.UseSwagger();
app.UseSwaggerUI();
// Cors
app.UseCors(corsPolicy!);

// Output cache. IMPORTANT to use output cache after CORS
app.UseOutputCache();

// Authorization and authentication
app.UseAuthentication();
app.UseAuthorization();

// Mapping Controllers
app.MapControllers();

// Registering endpoints
app.UseMiddleware<ValidationExceptionMiddleware>();

// Running the application
app.Run();