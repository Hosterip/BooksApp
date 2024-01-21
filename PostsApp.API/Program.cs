using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using PostsApp.Application;
using PostsApp.Infrastructure;
using PostsApp.Middlewares;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
// Adding Dependency Injectable 
builder.Services.AddInfrastructure();
builder.Services.AddApplication();

// Authentication || Authorization

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAuthorized", policy =>
        policy.RequireClaim(ClaimTypes.NameIdentifier));
});

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseExceptionHandler("/Error");
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ValidationExceptionMiddleware>();
app.MapControllers();

app.Run();