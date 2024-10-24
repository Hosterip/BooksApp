using System.Reflection;
using PostsApp.Application;
using PostsApp.Common.Constants;
using PostsApp.Common.Extensions;
using PostsApp.Controllers;
using PostsApp.Infrastructure;
using PostsApp.Middlewares;
using Toycloud.AspNetCore.Mvc.ModelBinding;


// Add CORS
var corsAllow = "CorsAllow";
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCorsPolicy(corsAllow);

// Add services to the container.
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adding Dependency Injectable 
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

// Authentication || Authorization
builder.Services.AddAuth();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseExceptionHandler(ApiEndpoints.Error.ErrorHandler);
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// Swagger

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors(corsAllow);

// Authorization and authentication
app.UseAuthentication();
app.UseAuthorization();

// Mapping end points
app.Services.GetRequiredService<IEndpoint>().MapEndpoint(app);

// Registering endpoints
app.UseMiddleware<ValidateUserMiddleware>();
app.UseMiddleware<ValidationExceptionMiddleware>();

// Running the application
app.Run();