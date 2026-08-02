using Rumos.ShopMate.ConsoleApp.Components;
using Rumos.ShopMate.ConsoleApp.Ui;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Utils;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.ConsoleApp.Menus;

public class UserMenu(
    ConsoleUi ui,
    IShoppingListService shoppingListService,
    IProductCatalogService productCatalogService,
    ListPicker listPicker,
    ShoppingModeMenu shoppingModeMenu)
{
    public void Show(UserDto currentUser)
    {
        var logout = false;

        while (!logout)
        {
            ui.Clear();
            ui.ShowTitle("USER MENU - " + currentUser.Username);
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
                        ShowShoppingLists(currentUser);
                        break;
                    case "2":
                        CreateShoppingList(currentUser);
                        break;
                    case "3":
                        AddItemToList(currentUser);
                        break;
                    case "4":
                        CompleteItem(currentUser);
                        break;
                    case "5":
                        ShareList(currentUser);
                        break;
                    case "6":
                        ArchiveList(currentUser);
                        break;
                    case "7":
                        StartShoppingMission(currentUser);
                        break;
                    case "8":
                        ShowDashboard(currentUser);
                        break;
                    case "9":
                        ShowActivityFeed(currentUser);
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
            catch (ServiceException ex)
            {
                ui.ShowError(ex.Message);
                ui.Pause();
            }
            catch (DomainException ex)
            {
                ui.ShowDomainRule(ex.Message);
                ui.Pause();
            }
        }
    }

    private void ShowShoppingLists(UserDto currentUser)
    {
        ui.Clear();
        ui.ShowTitle("MY SHOPPING LISTS");

        var shoppingLists = listPicker.GetShoppingListsFor(currentUser.Id);

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

    private void CreateShoppingList(UserDto currentUser)
    {
        ui.Clear();
        ui.ShowTitle("CREATE SHOPPING LIST");

        var name = ui.AskText("List name");
        var shoppingList = shoppingListService.Create(new CreateShoppingListDto
        {
            Name = name,
            OwnerId = currentUser.Id
        });

        ui.ShowMessage("Created list: " + shoppingList.Name);
        ui.Pause();
    }

    private void AddItemToList(UserDto currentUser)
    {
        ui.Clear();
        ui.ShowTitle("ADD ITEM");

        var shoppingList = listPicker.ChooseShoppingList(currentUser.Id);

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

        ui.ShowMessage("Using unit: " + selectedProduct.Unit);

        var item = shoppingListService.AddItem(
            shoppingList.Id,
            selectedProduct.Name,
            quantity,
            selectedProduct.Unit,
            currentUser.Id);

        ui.ShowMessage("Added item: " + item.Name);
        ui.ShowMessage("Auto category: " + item.Category);
        ShowSuggestions(item.Name);
        ui.Pause();
    }

    private void CompleteItem(UserDto currentUser)
    {
        ui.Clear();
        ui.ShowTitle("COMPLETE ITEM");

        var shoppingList = listPicker.ChooseShoppingList(currentUser.Id);

        if (shoppingList == null)
        {
            ui.Pause();
            return;
        }

        var item = listPicker.ChooseItem(shoppingList);

        if (item == null)
        {
            ui.Pause();
            return;
        }

        var updatedShoppingList = shoppingListService.CompleteItem(
            shoppingList.Id,
            item.Id,
            currentUser.Id);

        ui.ShowMessage("Completed item: " + item.Name);
        ui.WriteProgressBar(updatedShoppingList);
        ui.Pause();
    }

    private void ShareList(UserDto currentUser)
    {
        ui.Clear();
        ui.ShowTitle("SHARE LIST");

        var shoppingList = listPicker.ChooseShoppingList(currentUser.Id);

        if (shoppingList == null)
        {
            ui.Pause();
            return;
        }

        var username = ui.AskText("Username to share with");

        Console.WriteLine();
        ui.WriteRoleOptions();
        var role = ui.GetRoleFromOption(ui.AskNumber("Role"));

        shoppingListService.Share(
            shoppingList.Id,
            username,
            role,
            currentUser.Id);

        ui.ShowMessage("Shared list with " + username + " as " + role + ".");
        ui.Pause();
    }

    private void ArchiveList(UserDto currentUser)
    {
        ui.Clear();
        ui.ShowTitle("ARCHIVE LIST");

        var shoppingList = listPicker.ChooseShoppingList(currentUser.Id);

        if (shoppingList == null)
        {
            ui.Pause();
            return;
        }

        var archivedShoppingList = shoppingListService.Archive(
            shoppingList.Id,
            currentUser.Id);

        ui.ShowMessage("Archived list: " + archivedShoppingList.Name);
        ui.Pause();
    }

    private void StartShoppingMission(UserDto currentUser)
    {
        ui.Clear();
        ui.ShowTitle("CHOOSE MISSION LIST");

        var shoppingList = listPicker.ChooseShoppingList(currentUser.Id);

        if (shoppingList == null)
        {
            ui.Pause();
            return;
        }

        shoppingModeMenu.Show(shoppingList, currentUser);
    }

    private void ShowDashboard(UserDto currentUser)
    {
        ui.Clear();
        ui.ShowTitle("PROGRESS DASHBOARD");

        var shoppingLists = listPicker.GetShoppingListsFor(currentUser.Id);

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

    private void ShowActivityFeed(UserDto currentUser)
    {
        ui.Clear();
        ui.ShowTitle("ACTIVITY FEED");

        var shoppingList = listPicker.ChooseShoppingList(currentUser.Id);

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

    private ProductDto ChooseProduct()
    {
        var searchText = ui.AskText(
            "Search product (example: arroz, leite, pao)");
        var products = productCatalogService.Search(searchText);

        if (products.Count == 0)
        {
            ui.ShowWarning("No product found.");
            return null;
        }

        Console.WriteLine();
        Console.WriteLine("Choose a product:");

        for (var i = 0; i < products.Count; i++)
        {
            Console.WriteLine(
                (i + 1) + ". " +
                products[i].Name +
                " (" + products[i].Unit + ")");
        }

        Console.WriteLine("0. Write custom product");

        var option = ui.AskNumber("Number");

        if (option == 0)
        {
            var customName = ui.AskText("Custom product name");
            Console.WriteLine();
            ui.WriteUnitOptions();
            var customUnit = ui.GetUnitFromOption(ui.AskNumber("Unit"));

            return new ProductDto
            {
                Name = customName,
                Unit = customUnit
            };
        }

        if (option < 1 || option > products.Count)
        {
            ui.ShowWarning("Invalid product.");
            return null;
        }

        return products[option - 1];
    }
}
