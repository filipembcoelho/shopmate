using Rumos.ShopMate.ConsoleApp.Components;
using Rumos.ShopMate.ConsoleApp.Ui;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Domain.Model.Enums;
using Rumos.ShopMate.Domain.Utils;

namespace Rumos.ShopMate.ConsoleApp.Menus;

public class UserMenu(ConsoleUi ui, User currentUser)
{
    private readonly ListPicker _listPicker = new ListPicker(ui);

    public void Show()
    {
        var logout = false;

        while (!logout)
        {
            ui.Clear();
            ui.ShowTitle("USER MENU - " + currentUser.Account.Username);
            ui.WriteMenuOption("1", "Show my shopping lists");
            ui.WriteMenuOption("2", "Create shopping list");
            ui.WriteMenuOption("3", "Add item to list");
            ui.WriteMenuOption("4", "Complete item");
            ui.WriteMenuOption("5", "Share list");
            ui.WriteMenuOption("6", "Archive list");
            ui.WriteMenuOption("7", "Shopping mission mode");
            ui.WriteMenuOption("8", "Progress dashboard");
            ui.WriteMenuOption("9", "Activity feed");
            ui.WriteMenuOption("0", "Logout");
            Console.WriteLine();

            var option = ui.AskText("Choose an option");

            try
            {
                switch (option)
                {
                    case "1":
                        ShowShoppingLists();
                        break;
                    case "2":
                        CreateShoppingList();
                        break;
                    case "3":
                        AddItemToList();
                        break;
                    case "4":
                        CompleteItem();
                        break;
                    case "5":
                        ShareList();
                        break;
                    case "6":
                        ArchiveList();
                        break;
                    case "7":
                        StartShoppingMission();
                        break;
                    case "8":
                        ShowDashboard();
                        break;
                    case "9":
                        ShowActivityFeed();
                        break;
                    case "0":
                        logout = true;
                        break;
                    default:
                        ui.ShowWarning("Invalid option.");
                        ui.Pause();
                        break;
                }
            }
            catch (DomainException ex)
            {
                ui.ShowDomainRule(ex.Message);
                ui.Pause();
            }
        }
    }

    private void ShowShoppingLists()
    {
        ui.Clear();
        ui.ShowTitle("MY SHOPPING LISTS");

        var shoppingLists = ApplicationContext.GetShoppingListsFor(currentUser);

        if (shoppingLists.Count == 0)
        {
            ui.ShowWarning("You do not have any shopping lists yet.");
            ui.Pause();
            return;
        }

        for (var i = 0; i < shoppingLists.Count; i++)
        {
            Console.WriteLine("List " + (i + 1));
            ui.WriteListHeader(shoppingLists[i]);
            ui.WriteMembers(shoppingLists[i]);
            ui.WriteItems(shoppingLists[i]);
            Console.WriteLine();
        }

        ui.Pause();
    }

    private void CreateShoppingList()
    {
        ui.Clear();
        ui.ShowTitle("CREATE SHOPPING LIST");

        var name = ui.AskText("List name");
        var shoppingList = ApplicationContext.CreateShoppingList(name, currentUser);

        ui.ShowMessage("Created list: " + shoppingList.Name);
        ui.Pause();
    }

    private void AddItemToList()
    {
        ui.Clear();
        ui.ShowTitle("ADD ITEM");

        var shoppingList = _listPicker.ChooseShoppingList(currentUser);

        if (shoppingList == null)
        {
            ui.Pause();
            return;
        }

        var selectedProduct = ChooseProduct();

        if (selectedProduct == null)
        {
            ui.Pause();
            return;
        }

        var quantity = ui.AskNumber("Quantity");
        var unit = selectedProduct.Unit;

        ui.ShowMessage("Using unit: " + unit);

        var item = shoppingList.AddItem(selectedProduct.Name, quantity, unit, currentUser);

        ui.ShowMessage("Added item: " + item.Name);
        ui.ShowMessage("Auto category: " + item.Category.Value);
        ShowSuggestions(item.Name);
        ui.Pause();
    }

    private void CompleteItem()
    {
        ui.Clear();
        ui.ShowTitle("COMPLETE ITEM");

        var shoppingList = _listPicker.ChooseShoppingList(currentUser);

        if (shoppingList == null)
        {
            ui.Pause();
            return;
        }

        var item = _listPicker.ChooseItem(shoppingList);

        if (item == null)
        {
            ui.Pause();
            return;
        }

        shoppingList.CompleteItem(item, currentUser);

        ui.ShowMessage("Completed item: " + item.Name);
        ui.WriteProgressBar(shoppingList);
        ui.Pause();
    }

    private void ShareList()
    {
        ui.Clear();
        ui.ShowTitle("SHARE LIST");

        var shoppingList = _listPicker.ChooseShoppingList(currentUser);

        if (shoppingList == null)
        {
            ui.Pause();
            return;
        }

        var username = ui.AskText("Username to share with");
        var user = ApplicationContext.FindUserByUsername(username);

        if (user == null)
        {
            ui.ShowWarning("User was not found.");
            ui.Pause();
            return;
        }

        Console.WriteLine();
        ui.WriteRoleOptions();
        var role = ui.GetRoleFromOption(ui.AskNumber("Role"));

        shoppingList.ShareWith(user, role, currentUser);

        ui.ShowMessage("Shared list with " + user.Name + " as " + role + ".");
        ui.Pause();
    }

    private void ArchiveList()
    {
        ui.Clear();
        ui.ShowTitle("ARCHIVE LIST");

        var shoppingList = _listPicker.ChooseShoppingList(currentUser);

        if (shoppingList == null)
        {
            ui.Pause();
            return;
        }

        shoppingList.Archive(currentUser);

        ui.ShowMessage("Archived list: " + shoppingList.Name);
        ui.Pause();
    }

    private void StartShoppingMission()
    {
        ui.Clear();
        ui.ShowTitle("CHOOSE MISSION LIST");

        var shoppingList = _listPicker.ChooseShoppingList(currentUser);

        if (shoppingList == null)
        {
            ui.Pause();
            return;
        }

        var shoppingModeMenu = new ShoppingModeMenu(ui, shoppingList, currentUser);
        shoppingModeMenu.Show();
    }

    private void ShowDashboard()
    {
        ui.Clear();
        ui.ShowTitle("PROGRESS DASHBOARD");

        var shoppingLists = ApplicationContext.GetShoppingListsFor(currentUser);

        if (shoppingLists.Count == 0)
        {
            ui.ShowWarning("You do not have any shopping lists yet.");
            ui.Pause();
            return;
        }

        foreach (var shoppingList in shoppingLists)
        {
            Console.WriteLine(shoppingList.Name);
            ui.WriteProgressBar(shoppingList);
            Console.WriteLine();
        }

        ui.Pause();
    }

    private void ShowActivityFeed()
    {
        ui.Clear();
        ui.ShowTitle("ACTIVITY FEED");

        var shoppingList = _listPicker.ChooseShoppingList(currentUser);

        if (shoppingList == null)
        {
            ui.Pause();
            return;
        }

        ui.WriteActivities(shoppingList);
        ui.Pause();
    }

    private void ShowSuggestions(string itemName)
    {
        var suggestions = SuggestionUtils.GetSuggestionsFor(itemName);

        if (suggestions.Count == 0)
        {
            return;
        }

        Console.WriteLine();
        ui.ShowMessage("Smart suggestions:");

        foreach (var suggestion in suggestions)
        {
            Console.WriteLine("- " + suggestion);
        }
    }

    private ProductCatalogItem ChooseProduct()
    {
        var searchText = ui.AskText("Search product (example: arroz, leite, pao)");
        var products = ProductCatalog.Search(searchText);

        if (products.Count == 0)
        {
            ui.ShowWarning("No product found.");
            return null;
        }

        Console.WriteLine();
        Console.WriteLine("Choose a product:");

        for (var i = 0; i < products.Count; i++)
        {
            Console.WriteLine((i + 1) + ". " + products[i].Name + " (" + products[i].Unit + ")");
        }

        Console.WriteLine("0. Write custom product");

        var option = ui.AskNumber("Number");

        if (option == 0)
        {
            var customName = ui.AskText("Custom product name");
            Console.WriteLine();
            ui.WriteUnitOptions();
            var customUnit = ui.GetUnitFromOption(ui.AskNumber("Unit"));
            return new ProductCatalogItem(customName, customUnit);
        }

        if (option < 1 || option > products.Count)
        {
            ui.ShowWarning("Invalid product.");
            return null;
        }

        return products[option - 1];
    }
}
