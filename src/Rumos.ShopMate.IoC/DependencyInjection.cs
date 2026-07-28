using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Services.Implementations;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.IoC;

public static class DependencyInjection
{
    public static void AddShopMateServices(this IServiceCollection services,  IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IProductCatalogService, ProductCatalogService>();
        services.AddScoped<IShoppingListService, ShoppingListService>();
        
        var cs = configuration.GetConnectionString("ShopMate");
        
        services.AddDbContext<ApplicationContext>(options =>
        {
            options.UseSqlServer(cs);
        });
    }
}