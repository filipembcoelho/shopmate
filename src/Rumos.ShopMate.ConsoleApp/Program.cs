using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.ConsoleApp.Application;
using Rumos.ShopMate.ConsoleApp.Components;
using Rumos.ShopMate.ConsoleApp.Menus;
using Rumos.ShopMate.ConsoleApp.Ui;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Services.Implementations;
using Rumos.ShopMate.Services.Interfaces;

using var context = new ApplicationContext();

context.Database.Migrate();

if (!context.Users.Any())
{
    context.SeedData();
    context.SaveChanges();
}

IAuthenticationService authenticationService =
    new AuthenticationService(context);
IUserService userService =
    new UserService(context);
IShoppingListService shoppingListService =
    new ShoppingListService(context);
IProductCatalogService productCatalogService =
    new ProductCatalogService();

var ui = new ConsoleUi();
var receiptPrinter = new ReceiptPrinter(ui);
var shoppingModeMenu = new ShoppingModeMenu(
    ui,
    shoppingListService,
    receiptPrinter);
var listPicker = new ListPicker(ui, shoppingListService);
var userMenu = new UserMenu(
    ui,
    shoppingListService,
    productCatalogService,
    listPicker,
    shoppingModeMenu);
var mainMenu = new MainMenu(
    ui,
    authenticationService,
    userService,
    userMenu);
var application = new ShopMateConsoleApplication(mainMenu);

application.Run();
