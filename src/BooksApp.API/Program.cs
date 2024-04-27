using PostsApp.Application;
using PostsApp.Common.Extensions;
using PostsApp.Infrastructure;
using PostsApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);
// Add CORS
var corsAllow = "CorsAllow";
builder.Services.AddCorsPolicy(corsAllow);

// Add services to the container.
builder.Services.AddControllers();
// Adding Dependency Injectable 
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

// Authentication || Authorization

builder.Services.AddAuth();

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error");
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors(corsAllow);

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationExceptionMiddleware>();
app.MapControllers();

app.Run();