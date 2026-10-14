using Rumos.ShopMate.IoC;

var builder = WebApplication.CreateBuilder(args); // server

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddShopMateServices(builder.Configuration);

const string angularDevelopment = "AngularDevelopment";

builder.Services.AddCors(options =>
{
    options.AddPolicy(angularDevelopment, policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors(angularDevelopment);
}

app.MapControllers();

app.Run();
