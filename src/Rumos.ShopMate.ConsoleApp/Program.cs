using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rumos.ShopMate.ConsoleApp.Application;
using Rumos.ShopMate.ConsoleApp.Components;
using Rumos.ShopMate.ConsoleApp.Menus;
using Rumos.ShopMate.ConsoleApp.Ui;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Services.Implementations;
using Rumos.ShopMate.Services.Interfaces;


var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

var cs = configuration.GetConnectionString("ShopMate");

if (string.IsNullOrEmpty(cs))
{
    throw new ArgumentException("Connection string 'ShopMate' not found in appsettings.json.");
}

var services = new ServiceCollection();

services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(cs);
});

// Services
services.AddScoped<IUserService, UserService>();
services.AddScoped<IAuthenticationService, AuthenticationService>();
services.AddScoped<IProductCatalogService, ProductCatalogService>();
services.AddScoped<IShoppingListService, ShoppingListService>();

// UI
services.AddScoped<MainMenu>();
services.AddScoped<ConsoleUi>();
services.AddScoped<ReceiptPrinter>();
services.AddScoped<UserMenu>();
services.AddScoped<ShoppingModeMenu>();
services.AddScoped<ListPicker>();
services.AddScoped<ShopMateConsoleApplication>();

var serviceProvider = services.BuildServiceProvider();
var scope = serviceProvider.CreateScope();

var context = scope.ServiceProvider.GetService<ApplicationContext>();

context.Database.Migrate();

if (!context.Users.Any())
{
    context.SeedData();
    context.SaveChanges();
}

var application = scope.ServiceProvider.GetService<ShopMateConsoleApplication>();
application.Run();
