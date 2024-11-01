using BooksApp.API.Common.Constants;
using BooksApp.API.Common.Extensions;
using BooksApp.API.Middlewares;
using BooksApp.Application;
using BooksApp.Infrastructure;
using Toycloud.AspNetCore.Mvc.ModelBinding;


// Add CORS
var corsAllow = "CorsAllow";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCorsPolicy(corsAllow);

// Controllers
builder.Services.AddControllers(options => { options.ModelBinderProviders.InsertBodyOrDefaultBinding(); });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding Dependency Injectable 
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

// Authentication || Authorization
builder.Services.AddAuth();

// Building app
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseExceptionHandler(ApiRoutes.Error.ErrorHandler);
if (!app.Environment.IsDevelopment())
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
// Swagger
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(corsAllow);

// Authorization and authentication
app.UseAuthentication();
app.UseAuthorization();

// Mapping Controllers
app.MapControllers();

// Registering endpoints
app.UseMiddleware<ValidateUserMiddleware>();
app.UseMiddleware<ValidationExceptionMiddleware>();

// Running the application
app.Run();