using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.ConsoleApp.Application;
using Rumos.ShopMate.ConsoleApp.Components;
using Rumos.ShopMate.ConsoleApp.Menus;
using Rumos.ShopMate.ConsoleApp.Ui;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Services.Implementations;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.Tests;

public static class ConsoleDependencyTests
{
    private static int _failedTests;

    public static int Run()
    {
        _failedTests = 0;

        RunTest(
            ConsoleClassesDoNotDependOnApplicationContext,
            "Console classes do not depend on ApplicationContext");
        RunTest(
            ConsoleClassesDependOnServiceInterfaces,
            "Console classes depend on service interfaces");
        RunTest(
            ConsoleApplicationStartsAndExits,
            "Console application starts and exits");

        return _failedTests;
    }

    private static void ConsoleClassesDoNotDependOnApplicationContext()
    {
        foreach (var type in GetConsoleTypes())
        {
            var dependsOnContext = type
                .GetConstructors()
                .SelectMany(constructor => constructor.GetParameters())
                .Any(parameter => parameter.ParameterType == typeof(ApplicationContext));

            AssertTrue(
                !dependsOnContext,
                type.Name + " must not receive ApplicationContext.");
        }
    }

    private static void ConsoleClassesDependOnServiceInterfaces()
    {
        AssertConstructorContains<MainMenu, IAuthenticationService>();
        AssertConstructorContains<MainMenu, IUserService>();
        AssertConstructorContains<UserMenu, IShoppingListService>();
        AssertConstructorContains<UserMenu, IProductCatalogService>();
        AssertConstructorContains<ListPicker, IShoppingListService>();
        AssertConstructorContains<ShoppingModeMenu, IShoppingListService>();
    }

    private static void ConsoleApplicationStartsAndExits()
    {
        var originalInput = Console.In;
        var originalOutput = Console.Out;
        var output = new StringWriter();

        try
        {
            Console.SetIn(new StringReader("0" + Environment.NewLine));
            Console.SetOut(output);

            var options =
                new DbContextOptionsBuilder<ApplicationContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

            using var context = new ApplicationContext(options);

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

            AssertTrue(
                output.ToString().Contains("GOODBYE"),
                "The Console application should reach the exit screen.");
        }
        finally
        {
            Console.SetIn(originalInput);
            Console.SetOut(originalOutput);
        }
    }

    private static Type[] GetConsoleTypes()
    {
        return
        [
            typeof(ShopMateConsoleApplication),
            typeof(ListPicker),
            typeof(MainMenu),
            typeof(ShoppingModeMenu),
            typeof(UserMenu),
            typeof(ConsoleUi),
            typeof(ReceiptPrinter)
        ];
    }

    private static void AssertConstructorContains<TConsumer, TDependency>()
    {
        var containsDependency = typeof(TConsumer)
            .GetConstructors()
            .SelectMany(constructor => constructor.GetParameters())
            .Any(parameter => parameter.ParameterType == typeof(TDependency));

        AssertTrue(
            containsDependency,
            typeof(TConsumer).Name + " must receive " + typeof(TDependency).Name + ".");
    }

    private static void RunTest(Action test, string testName)
    {
        try
        {
            test();
            Console.WriteLine("[PASS] " + testName);
        }
        catch (Exception ex)
        {
            _failedTests++;
            Console.WriteLine("[FAIL] " + testName);
            Console.WriteLine("       " + ex.Message);
        }
    }

    private static void AssertTrue(bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception(message);
        }
    }
}
