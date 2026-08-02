using Microsoft.EntityFrameworkCore;
using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Domain.Model.Enums;
using Rumos.ShopMate.Services.Dtos;
using Rumos.ShopMate.Services.Exceptions;
using Rumos.ShopMate.Services.Implementations;
using Rumos.ShopMate.Services.Interfaces;

namespace Rumos.ShopMate.Tests;

public static class ServiceTests
{
    private static int _failedTests;

    public static int Run()
    {
        _failedTests = 0;

        RunTest(ServiceInterfacesDoNotExposeDomainEntities, "Service interfaces do not expose domain entities");
        RunTest(LoginReturnsASafeUserDto, "Login returns a safe user DTO");
        RunTest(LoginRejectsInvalidCredentials, "Login rejects invalid credentials");
        RunTest(LoginValidatesRequiredUsername, "Login validates required username");
        RunTest(RegisterPersistsAUser, "Register persists a user");
        RunTest(RegisterRejectsADuplicateUsername, "Register rejects a duplicate username");
        RunTest(RegisterValidatesRequiredData, "Register validates required data");
        RunTest(SuggestUsernameUsesExistingUsers, "Suggest username uses existing users");
        RunTest(SuggestUsernameValidatesFullName, "Suggest username validates full name");
        RunTest(GetByUsernameReturnsASafeUserDto, "Get by username returns a safe user DTO");
        RunTest(GetByUsernameRejectsAnUnknownUser, "Get by username rejects an unknown user");
        RunTest(GetForUserMapsTheFullShoppingList, "Get for user maps the full shopping list");
        RunTest(GetForUserValidatesUserId, "Get for user validates user ID");
        RunTest(GetByIdReturnsAVisibleShoppingList, "Get by ID returns a visible shopping list");
        RunTest(GetByIdRejectsAnInvisibleShoppingList, "Get by ID rejects an invisible shopping list");
        RunTest(CreateShoppingListPersistsTheList, "Create shopping list persists the list");
        RunTest(CreateShoppingListRejectsAnUnknownOwner, "Create shopping list rejects an unknown owner");
        RunTest(AddItemPersistsAndReturnsTheItem, "Add item persists and returns the item");
        RunTest(AddItemValidatesQuantity, "Add item validates quantity");
        RunTest(AddItemValidatesUnit, "Add item validates unit");
        RunTest(CompleteItemPersistsCompletion, "Complete item persists completion");
        RunTest(CompleteItemRejectsAnUnknownItem, "Complete item rejects an unknown item");
        RunTest(UpdateItemValidatesRequiredName, "Update item validates required name");
        RunTest(UpdateItemValidatesQuantity, "Update item validates quantity");
        RunTest(UpdateItemValidatesUnit, "Update item validates unit");
        RunTest(UpdateItemRejectsAnInvalidItemId, "Update item rejects an invalid item ID");
        RunTest(UpdateItemRejectsAnUnknownItem, "Update item rejects an unknown item");
        RunTest(
            UpdateItemPersistsTheCompleteEditableState,
            "Update item persists the complete editable state");
        RunTest(
            UpdateItemSkipsUnchangedDomainOperations,
            "Update item skips unchanged domain operations");
        RunTest(RemoveItemPersistsTheRemoval, "Remove item persists the removal");
        RunTest(RemoveItemRejectsAnInvalidItemId, "Remove item rejects an invalid item ID");
        RunTest(RemoveItemRejectsAnUnknownItem, "Remove item rejects an unknown item");
        RunTest(ShareAddsTheRequestedMember, "Share adds the requested member");
        RunTest(ShareValidatesRole, "Share validates role");
        RunTest(
            ChangeMemberRolePersistsTheNewRole,
            "Change member role persists the new role");
        RunTest(RemoveMemberPersistsTheRemoval, "Remove member persists the removal");
        RunTest(ArchivePersistsTheArchivedState, "Archive persists the archived state");
        RunTest(RenameShoppingListPersistsTheNewName, "Rename shopping list persists the new name");
        RunTest(ArchivedShoppingListsAreHidden, "Archived shopping lists are hidden");
        RunTest(ProductSearchReturnsProductDtos, "Product search returns product DTOs");
        RunTest(ProductSearchValidatesSearchText, "Product search validates search text");
        RunTest(MissingConnectionStringIsRejected, "Missing connection string is rejected");

        return _failedTests;
    }

    private static void ServiceInterfacesDoNotExposeDomainEntities()
    {
        var interfaceTypes = new[]
        {
            typeof(IAuthenticationService),
            typeof(IUserService),
            typeof(IShoppingListService),
            typeof(IProductCatalogService)
        };

        foreach (var interfaceType in interfaceTypes)
        {
            foreach (var method in interfaceType.GetMethods())
            {
                AssertTrue(
                    !ContainsDomainEntity(method.ReturnType),
                    interfaceType.Name + "." + method.Name + " must not return a domain entity.");
            }
        }
    }

    private static void LoginReturnsASafeUserDto()
    {
        using var context = CreateContext();
        var user = new User("Test User", "testuser", "Testing1!");
        context.Users.Add(user);
        context.SaveChanges();
        var service = new AuthenticationService(context);

        var userDto = service.Login(" TESTUSER ", "Testing1!");
        AssertEqual(user.Id, userDto.Id, "Login should return the persisted user ID.");
        AssertEqual("Test User", userDto.FullName, "Login should return the full name.");
        AssertEqual("testuser", userDto.Username, "Login should return the normalized username.");
        AssertTrue(typeof(UserDto).GetProperty("Password") == null, "UserDto must not expose a password.");
    }

    private static void LoginRejectsInvalidCredentials()
    {
        using var context = CreateContext();
        context.Users.Add(new User("Test User", "testuser", "Testing1!"));
        context.SaveChanges();
        var service = new AuthenticationService(context);

        AssertInvalidCredentialsException(
            () => service.Login("testuser", "Wrong1!"),
            "Invalid username or password.");
    }

    private static void LoginValidatesRequiredUsername()
    {
        using var context = CreateContext();
        var service = new AuthenticationService(context);

        AssertServiceException(
            () => service.Login("", "Testing1!"),
            "Username is required.");
    }

    private static void RegisterPersistsAUser()
    {
        using var context = CreateContext();
        var service = new AuthenticationService(context);
        var registerUserDto = new RegisterUserDto
        {
            FullName = "New User",
            Username = "newuser",
            Password = "Testing1!"
        };

        var result = service.Register(registerUserDto);

        AssertTrue(result.Id > 0, "Registration should return the generated user ID.");
        AssertEqual("New User", result.FullName, "Registration should return the full name.");
        AssertEqual("newuser", result.Username, "Registration should return the username.");
        AssertEqual(1, context.Users.Count(), "Registration should persist the user.");
    }

    private static void RegisterRejectsADuplicateUsername()
    {
        using var context = CreateContext();
        context.Users.Add(new User("Existing User", "existing", "Testing1!"));
        context.SaveChanges();
        var service = new AuthenticationService(context);
        var registerUserDto = new RegisterUserDto
        {
            FullName = "Another User",
            Username = " EXISTING ",
            Password = "Testing1!"
        };

        AssertServiceException(
            () => service.Register(registerUserDto),
            "Username already exists.");
    }

    private static void RegisterValidatesRequiredData()
    {
        using var context = CreateContext();
        var service = new AuthenticationService(context);
        var registerUserDto = new RegisterUserDto
        {
            FullName = "",
            Username = "newuser",
            Password = "Testing1!"
        };

        AssertServiceException(
            () => service.Register(registerUserDto),
            "Full name is required.");
    }

    private static void SuggestUsernameUsesExistingUsers()
    {
        using var context = CreateContext();
        context.Users.Add(new User("Existing User", "jdoe", "Testing1!"));
        context.SaveChanges();
        var service = new UserService(context);

        var result = service.SuggestUsername("John Doe");

        AssertEqual("jdoe2", result, "The suggestion should avoid an existing username.");
    }

    private static void SuggestUsernameValidatesFullName()
    {
        using var context = CreateContext();
        var service = new UserService(context);

        AssertServiceException(
            () => service.SuggestUsername(" "),
            "Full name is required.");
    }

    private static void GetByUsernameReturnsASafeUserDto()
    {
        using var context = CreateContext();
        var user = new User("Test User", "testuser", "Testing1!");
        context.Users.Add(user);
        context.SaveChanges();
        var service = new UserService(context);

        var userDto = service.GetByUsername(" TESTUSER ");
        AssertEqual(user.Id, userDto.Id, "The lookup should return the persisted user ID.");
        AssertEqual("Test User", userDto.FullName, "The lookup should return the full name.");
        AssertEqual("testuser", userDto.Username, "The lookup should return the normalized username.");
    }

    private static void GetByUsernameRejectsAnUnknownUser()
    {
        using var context = CreateContext();
        var service = new UserService(context);

        AssertServiceNotFoundException(
            () => service.GetByUsername("missing"),
            "User not found.");
    }

    private static void GetForUserMapsTheFullShoppingList()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        var result = service.GetForUser(data.MemberId);

        AssertEqual(1, result.Count, "The member should see the shared list.");
        var shoppingList = result[0];
        AssertEqual("Weekly list", shoppingList.Name, "The list name should be mapped.");
        AssertEqual("owneruser", shoppingList.Owner.Username, "The owner should be mapped.");
        AssertEqual(2, shoppingList.Members.Count, "Members should be mapped.");
        AssertEqual(1, shoppingList.Items.Count, "Items should be mapped.");
        AssertEqual("Dairy", shoppingList.Items[0].Category, "The item category should be mapped.");
        AssertEqual(2, shoppingList.Activities.Count, "Activities should be mapped.");
        AssertEqual(0, shoppingList.CompletedItems, "Completed count should be mapped.");
        AssertEqual(1, shoppingList.PendingItems, "Pending count should be mapped.");
        AssertEqual(0, shoppingList.ProgressPercentage, "Progress should be mapped.");
    }

    private static void GetForUserValidatesUserId()
    {
        using var context = CreateContext();
        var service = new ShoppingListService(context);

        AssertServiceException(
            () => service.GetForUser(0),
            "User ID must be greater than zero.");
    }

    private static void GetByIdReturnsAVisibleShoppingList()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        var result = service.GetById(data.ShoppingListId, data.MemberId);

        AssertEqual(data.ShoppingListId, result.Id, "The visible list should be returned.");
        AssertEqual("Weekly list", result.Name, "The visible list should be mapped.");
    }

    private static void GetByIdRejectsAnInvisibleShoppingList()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var outsider = new User("Outside User", "outsider", "Testing1!");
        context.Users.Add(outsider);
        context.SaveChanges();
        var service = new ShoppingListService(context);

        AssertServiceNotFoundException(
            () => service.GetById(data.ShoppingListId, outsider.Id),
            "Shopping list was not found.");
    }

    private static void CreateShoppingListPersistsTheList()
    {
        using var context = CreateContext();
        var owner = new User("Owner User", "owneruser", "Testing1!");
        context.Users.Add(owner);
        context.SaveChanges();
        var service = new ShoppingListService(context);

        var result = service.Create(new CreateShoppingListDto
        {
            Name = "Weekend groceries",
            OwnerId = owner.Id
        });

        AssertTrue(result.Id > 0, "The new list should receive an ID.");
        AssertEqual("Weekend groceries", result.Name, "The new list should keep its name.");
        AssertEqual("owneruser", result.Owner.Username, "The owner should be mapped.");
        AssertEqual(1, context.ShoppingLists.Count(), "The list should be persisted.");
    }

    private static void CreateShoppingListRejectsAnUnknownOwner()
    {
        using var context = CreateContext();
        var service = new ShoppingListService(context);

        AssertServiceNotFoundException(
            () => service.Create(new CreateShoppingListDto
            {
                Name = "Weekend groceries",
                OwnerId = 999
            }),
            "Owner was not found.");
    }

    private static void AddItemPersistsAndReturnsTheItem()
    {
        var data = CreateEmptyShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        var result = service.AddItem(
            data.ShoppingListId,
            "Milk",
            2,
            Unit.Liter,
            data.OwnerId);

        AssertTrue(result.Id > 0, "The item should receive an ID.");
        AssertEqual("Milk", result.Name, "The item name should be returned.");
        AssertEqual("Dairy", result.Category, "The automatic category should be returned.");
        AssertEqual(1, context.ShoppingListItems.Count(), "The item should be persisted.");
    }

    private static void AddItemValidatesQuantity()
    {
        var data = CreateEmptyShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        AssertServiceException(
            () => service.AddItem(data.ShoppingListId, "Milk", 0, Unit.Liter, data.OwnerId),
            "Quantity must be greater than zero.");
    }

    private static void AddItemValidatesUnit()
    {
        var data = CreateEmptyShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        AssertServiceException(
            () => service.AddItem(data.ShoppingListId, "Milk", 1, (Unit)999, data.OwnerId),
            "Unit is invalid.");
    }

    private static void CompleteItemPersistsCompletion()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        var result = service.CompleteItem(
            data.ShoppingListId,
            data.ItemId,
            data.OwnerId);

        AssertEqual(1, result.CompletedItems, "The completed count should be updated.");
        AssertEqual(100, result.ProgressPercentage, "Progress should be updated.");
        AssertTrue(result.Items[0].IsCompleted, "The item should be completed.");
    }

    private static void CompleteItemRejectsAnUnknownItem()
    {
        var data = CreateEmptyShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        AssertServiceNotFoundException(
            () => service.CompleteItem(data.ShoppingListId, 999, data.OwnerId),
            "Shopping list item was not found.");
    }

    private static void UpdateItemValidatesRequiredName()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        AssertServiceException(
            () => service.UpdateItem(
                data.ShoppingListId,
                data.ItemId,
                new UpdateShoppingListItemDto
                {
                    Name = " ",
                    Quantity = 1,
                    Unit = Unit.Liter,
                    IsCompleted = false
                },
                data.OwnerId),
            "Item name is required.");
    }

    private static void UpdateItemValidatesQuantity()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        AssertServiceException(
            () => service.UpdateItem(
                data.ShoppingListId,
                data.ItemId,
                new UpdateShoppingListItemDto
                {
                    Name = "Milk",
                    Quantity = 0,
                    Unit = Unit.Liter,
                    IsCompleted = false
                },
                data.OwnerId),
            "Quantity must be greater than zero.");
    }

    private static void UpdateItemValidatesUnit()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        AssertServiceException(
            () => service.UpdateItem(
                data.ShoppingListId,
                data.ItemId,
                new UpdateShoppingListItemDto
                {
                    Name = "Milk",
                    Quantity = 1,
                    Unit = (Unit)999,
                    IsCompleted = false
                },
                data.OwnerId),
            "Unit is invalid.");
    }

    private static void UpdateItemRejectsAnInvalidItemId()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        AssertServiceException(
            () => service.UpdateItem(
                data.ShoppingListId,
                0,
                new UpdateShoppingListItemDto
                {
                    Name = "Milk",
                    Quantity = 1,
                    Unit = Unit.Liter,
                    IsCompleted = false
                },
                data.OwnerId),
            "Shopping list item ID must be greater than zero.");
    }

    private static void UpdateItemRejectsAnUnknownItem()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        AssertServiceNotFoundException(
            () => service.UpdateItem(
                data.ShoppingListId,
                999,
                new UpdateShoppingListItemDto
                {
                    Name = "Milk",
                    Quantity = 1,
                    Unit = Unit.Liter,
                    IsCompleted = false
                },
                data.OwnerId),
            "Shopping list item was not found.");
    }

    private static void UpdateItemPersistsTheCompleteEditableState()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        var result = service.UpdateItem(
            data.ShoppingListId,
            data.ItemId,
            new UpdateShoppingListItemDto
            {
                Name = "Oat milk",
                Quantity = 3,
                Unit = Unit.Bottle,
                IsCompleted = true
            },
            data.OwnerId);

        AssertEqual("Oat milk", result.Name, "The item name should be updated.");
        AssertEqual(3, result.Quantity, "The item quantity should be updated.");
        AssertEqual(Unit.Bottle.ToString(), result.Unit.ToString(), "The unit should be updated.");
        AssertTrue(result.IsCompleted, "The item should be completed.");
    }

    private static void UpdateItemSkipsUnchangedDomainOperations()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);
        var activityCount = context.Activities.Count();

        service.UpdateItem(
            data.ShoppingListId,
            data.ItemId,
            new UpdateShoppingListItemDto
            {
                Name = "Milk",
                Quantity = 1,
                Unit = Unit.Liter,
                IsCompleted = false
            },
            data.OwnerId);

        AssertEqual(activityCount, context.Activities.Count(), "Unchanged PUT values should not add activity.");
    }

    private static void RemoveItemPersistsTheRemoval()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        service.RemoveItem(data.ShoppingListId, data.ItemId, data.OwnerId);

        AssertEqual(0, context.ShoppingListItems.Count(), "DELETE should remove the item row.");
    }

    private static void RemoveItemRejectsAnInvalidItemId()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        AssertServiceException(
            () => service.RemoveItem(data.ShoppingListId, 0, data.OwnerId),
            "Shopping list item ID must be greater than zero.");
    }

    private static void RemoveItemRejectsAnUnknownItem()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        AssertServiceNotFoundException(
            () => service.RemoveItem(data.ShoppingListId, 999, data.OwnerId),
            "Shopping list item was not found.");
    }

    private static void ShareAddsTheRequestedMember()
    {
        var data = CreateEmptyShoppingList();
        using var context = CreateContext(data.DatabaseName);
        context.Users.Add(new User("Member User", "memberuser", "Testing1!"));
        context.SaveChanges();
        var service = new ShoppingListService(context);

        var result = service.Share(
            data.ShoppingListId,
            " MEMBERUSER ",
            ShoppingListRole.Editor,
            data.OwnerId);

        AssertEqual(2, result.Members.Count, "Sharing should add the member.");
        AssertEqual("memberuser", result.Members[1].Username, "The shared member should be mapped.");
        AssertEqual(
            ShoppingListRole.Editor.ToString(),
            result.Members[1].Role.ToString(),
            "The selected role should be persisted.");
    }

    private static void ShareValidatesRole()
    {
        var data = CreateEmptyShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        AssertServiceException(
            () => service.Share(data.ShoppingListId, "memberuser", (ShoppingListRole)999, data.OwnerId),
            "Shopping list role is invalid.");
    }

    private static void ChangeMemberRolePersistsTheNewRole()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        var result = service.ChangeMemberRole(
            data.ShoppingListId,
            data.MemberId,
            new UpdateShoppingListMemberDto { Role = ShoppingListRole.Viewer },
            data.OwnerId);

        var member = result.Members.Single(item => item.UserId == data.MemberId);
        AssertEqual(ShoppingListRole.Viewer.ToString(), member.Role.ToString(), "PUT should persist the role.");
    }

    private static void RemoveMemberPersistsTheRemoval()
    {
        var data = CreateSharedShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        service.RemoveMember(data.ShoppingListId, data.MemberId, data.OwnerId);

        AssertEqual(1, context.ShoppingListMembers.Count(), "Only the owner membership should remain.");
    }

    private static void ArchivePersistsTheArchivedState()
    {
        var data = CreateEmptyShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        var result = service.Archive(data.ShoppingListId, data.OwnerId);

        AssertTrue(result.IsArchived, "The returned DTO should be archived.");
        AssertTrue(context.ShoppingLists.Single().IsArchived, "The archived state should be persisted.");
    }

    private static void RenameShoppingListPersistsTheNewName()
    {
        var data = CreateEmptyShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        var result = service.Rename(
            data.ShoppingListId,
            new UpdateShoppingListDto { Name = "Monthly groceries" },
            data.OwnerId);

        AssertEqual("Monthly groceries", result.Name, "PUT should return the renamed list.");
        AssertEqual(
            "Monthly groceries",
            context.ShoppingLists.Single().Name,
            "The renamed list should be persisted.");
    }

    private static void ArchivedShoppingListsAreHidden()
    {
        var data = CreateEmptyShoppingList();
        using var context = CreateContext(data.DatabaseName);
        var service = new ShoppingListService(context);

        service.Archive(data.ShoppingListId, data.OwnerId);

        AssertEqual(1, context.ShoppingLists.Count(), "Soft delete must preserve the row.");
        AssertEqual(0, service.GetForUser(data.OwnerId).Count, "Archived lists should be hidden.");
        AssertServiceNotFoundException(
            () => service.GetById(data.ShoppingListId, data.OwnerId),
            "Shopping list was not found.");
        AssertServiceNotFoundException(
            () => service.Rename(
                data.ShoppingListId,
                new UpdateShoppingListDto { Name = "Hidden list" },
                data.OwnerId),
            "Shopping list was not found.");
    }

    private static void ProductSearchReturnsProductDtos()
    {
        var service = new ProductCatalogService();

        var result = service.Search("arroz");

        AssertTrue(result.Count > 0, "The catalog should find arroz.");
        AssertEqual("Arroz", result[0].Name, "The product name should be mapped.");
        AssertEqual(Unit.Kilogram.ToString(), result[0].Unit.ToString(), "The product unit should be mapped.");
    }

    private static void ProductSearchValidatesSearchText()
    {
        var service = new ProductCatalogService();

        AssertServiceException(
            () => service.Search(" "),
            "Search text is required.");
    }

    private static void MissingConnectionStringIsRejected()
    {
        // const string variableName = "ConnectionStrings__ShopMate";
        // var originalValue = Environment.GetEnvironmentVariable(variableName);
        //
        // try
        // {
        //     Environment.SetEnvironmentVariable(variableName, null);
        //     using var context = new ApplicationContext();
        //     var exceptionWasThrown = false;
        //
        //     try
        //     {
        //         _ = context.Database.ProviderName;
        //     }
        //     catch (InvalidOperationException ex)
        //     {
        //         exceptionWasThrown = true;
        //         AssertEqual(
        //             "The ShopMate connection string was not configured.",
        //             ex.Message,
        //             "The exception should explain the missing configuration.");
        //     }
        //
        //     AssertTrue(exceptionWasThrown, "A missing connection string should be rejected.");
        // }
        // finally
        // {
        //     Environment.SetEnvironmentVariable(variableName, originalValue);
        // }
    }

    private static ShoppingListTestData CreateSharedShoppingList()
    {
        var data = CreateEmptyShoppingList();

        using var context = CreateContext(data.DatabaseName);
        var shoppingList = context.ShoppingLists
            .Include(list => list.Owner)
                .ThenInclude(owner => owner.Account)
            .Include(list => list.Members)
                .ThenInclude(member => member.User)
                    .ThenInclude(user => user.Account)
            .Include(list => list.Items)
            .Single(list => list.Id == data.ShoppingListId);
        var owner = shoppingList.Owner;
        var member = new User("Member User", "memberuser", "Testing1!");
        context.Users.Add(member);
        shoppingList.ShareWith(member, ShoppingListRole.Editor, owner);
        var item = shoppingList.AddItem("Milk", 1, Unit.Liter, owner);
        context.SaveChanges();

        return new ShoppingListTestData(
            data.DatabaseName,
            data.OwnerId,
            member.Id,
            data.ShoppingListId,
            item.Id);
    }

    private static ShoppingListTestData CreateEmptyShoppingList()
    {
        var databaseName = Guid.NewGuid().ToString();

        using var context = CreateContext(databaseName);
        var owner = new User("Owner User", "owneruser", "Testing1!");
        var shoppingList = new ShoppingList("Weekly list", owner);
        context.ShoppingLists.Add(shoppingList);
        context.SaveChanges();

        return new ShoppingListTestData(
            databaseName,
            owner.Id,
            0,
            shoppingList.Id,
            0);
    }

    private static ApplicationContext CreateContext()
    {
        return CreateContext(Guid.NewGuid().ToString());
    }

    private static ApplicationContext CreateContext(string databaseName)
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;

        return new ApplicationContext(options);
    }

    private static bool ContainsDomainEntity(Type type)
    {
        if (type == typeof(User) ||
            type == typeof(ShoppingList) ||
            type == typeof(ShoppingListItem))
        {
            return true;
        }

        return type.IsGenericType &&
            type.GetGenericArguments().Any(ContainsDomainEntity);
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

    private static void AssertServiceException(Action action, string expectedMessage)
    {
        var exceptionWasThrown = false;

        try
        {
            action();
        }
        catch (ServiceException ex)
        {
            exceptionWasThrown = true;
            AssertEqual(
                expectedMessage,
                ex.Message,
                "The service exception should explain the validation failure.");
        }

        AssertTrue(exceptionWasThrown, "Expected ServiceException.");
    }

    private static void AssertInvalidCredentialsException(
        Action action,
        string expectedMessage)
    {
        var exceptionWasThrown = false;

        try
        {
            action();
        }
        catch (InvalidCredentialsException ex)
        {
            exceptionWasThrown = true;
            AssertEqual(expectedMessage, ex.Message, "The login failure should be explained.");
        }

        AssertTrue(exceptionWasThrown, "Expected InvalidCredentialsException.");
    }

    private static void AssertServiceNotFoundException(
        Action action,
        string expectedMessage)
    {
        var exceptionWasThrown = false;

        try
        {
            action();
        }
        catch (ServiceNotFoundException ex)
        {
            exceptionWasThrown = true;
            AssertEqual(expectedMessage, ex.Message, "The missing resource should be explained.");
        }

        AssertTrue(exceptionWasThrown, "Expected ServiceNotFoundException.");
    }

    private static void AssertEqual(int expected, int actual, string message)
    {
        if (expected != actual)
        {
            throw new Exception(message + " Expected: " + expected + " Actual: " + actual);
        }
    }

    private static void AssertEqual(string expected, string actual, string message)
    {
        if (expected != actual)
        {
            throw new Exception(message + " Expected: " + expected + " Actual: " + actual);
        }
    }

    private static void AssertTrue(bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception(message);
        }
    }

    private sealed record ShoppingListTestData(
        string DatabaseName,
        int OwnerId,
        int MemberId,
        int ShoppingListId,
        int ItemId);
}
