using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.IoC;
using Rumos.ShopMate.Services.Implementations;
using Rumos.ShopMate.Services.Interfaces;
using Rumos.ShopMate.WebAPI.Controllers;
using AuthenticationService = Microsoft.AspNetCore.Authentication.AuthenticationService;
using IAuthenticationService = Microsoft.AspNetCore.Authentication.IAuthenticationService;

var builder = WebApplication.CreateBuilder(args); // server

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddShopMateServices(builder.Configuration);


// DI Lifetime
builder.Services.AddSingleton<SingletonExample>();
builder.Services.AddScoped<ScopedExample>();
builder.Services.AddTransient<TransientExample>();
builder.Services.AddScoped<HelloService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();