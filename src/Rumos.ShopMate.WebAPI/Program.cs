using Rumos.ShopMate.IoC;

var builder = WebApplication.CreateBuilder(args); // server

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddShopMateServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// TODO: Configure CORS when the Angular application
// starts calling this API from a different origin.
app.MapControllers();

app.Run();
