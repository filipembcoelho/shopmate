using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.ConsoleApp.Application;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Model.Common;

using var context = new ApplicationContext();

context.Database.Migrate();

if (!context.Users.Any())
{
    context.SeedData();
    context.SaveChanges();
}

ShopMateConsoleApplication application = new ShopMateConsoleApplication(context);
application.Run();

