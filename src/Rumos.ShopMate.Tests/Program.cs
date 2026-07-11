using Rumos.ShopMate.Data;
using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Model;
using Rumos.ShopMate.Domain.Model.Enums;
using Rumos.ShopMate.Domain.Utils;

var failedTests = 0;

StrongPasswordCreatesAccount();
PasswordWithoutCapitalLetterIsRejected();
PasswordWithoutSpecialCharacterIsRejected();
MemorySeedCreatesUsersWithValidPasswords();
CategoryUtilsDetectsDairyItems();
CategoryUtilsUsesOtherForUnknownItems();
CategoryUtilsDetectsArrozAsPantry();
ShoppingListRecordsActivityWhenItemIsAdded();
UsernameUtilsSuggestsFirstInitialAndLastName();
ProductCatalogFindsArroz();
ProductCatalogUsesRealisticUnits();
AccountRejectsUsernameWithSpaces();
NameRejectsFirstNameWithOneCharacter();
CategoryRejectsNamesThatAreTooLong();
CategoryRuleRejectsEmptyWords();
ActivityRejectsEmptyDescription();
ShoppingListRejectsInvalidRole();
ShoppingListItemRejectsInvalidUnit();
ShoppingListItemRejectsNullCategory();

if (failedTests > 0)
{
    Console.WriteLine();
    Console.WriteLine(failedTests + " test(s) failed.");
    Environment.Exit(1);
}

Console.WriteLine();
Console.WriteLine("All tests passed.");

void StrongPasswordCreatesAccount()
{
    var testName = "Strong password creates account";

    try
    {
        var account = new Account("student", "ShopMate1!");

        AssertEqual("student", account.Username, "Username should be normalized.");
        AssertEqual("ShopMate1!", account.Password, "Password should be stored.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void PasswordWithoutCapitalLetterIsRejected()
{
    var testName = "Password without capital letter is rejected";

    try
    {
        var exceptionWasThrown = false;

        try
        {
            new Account("student", "shopmate1!");
        }
        catch (InvalidAccountException ex)
        {
            exceptionWasThrown = true;
            AssertEqual("Password must contain at least one capital letter.", ex.Message, "Exception message should explain the rule.");
        }

        AssertTrue(exceptionWasThrown, "Expected InvalidAccountException.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void PasswordWithoutSpecialCharacterIsRejected()
{
    var testName = "Password without special character is rejected";

    try
    {
        var exceptionWasThrown = false;

        try
        {
            new Account("student", "ShopMate1");
        }
        catch (InvalidAccountException ex)
        {
            exceptionWasThrown = true;
            AssertEqual("Password must contain at least one special character.", ex.Message, "Exception message should explain the rule.");
        }

        AssertTrue(exceptionWasThrown, "Expected InvalidAccountException.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void MemorySeedCreatesUsersWithValidPasswords()
{
    var testName = "Memory seed creates users with valid passwords";

    try
    {
        var seedData = MemoryDomainSeeder.CreateSeedData();

        AssertTrue(seedData.Users.Count >= 5, "Seed should create at least five users.");

        foreach (var user in seedData.Users)
        {
            AssertTrue(user.Account.Password.Length >= 8, "Seed password should have at least 8 characters.");
        }

        AssertTrue(seedData.ShoppingLists.Count >= 2, "Seed should create at least two shopping lists.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void CategoryUtilsDetectsDairyItems()
{
    var testName = "Category utils detects dairy items";

    try
    {
        var category = CategoryUtils.GuessCategory("Milk");

        AssertEqual("Dairy", category.Value, "Milk should be categorized as dairy.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void CategoryUtilsUsesOtherForUnknownItems()
{
    var testName = "Category utils uses other for unknown items";

    try
    {
        var category = CategoryUtils.GuessCategory("Notebook");

        AssertEqual("Other", category.Value, "Unknown items should use Other.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void CategoryUtilsDetectsArrozAsPantry()
{
    var testName = "Category utils detects arroz as pantry";

    try
    {
        var category = CategoryUtils.GuessCategory("Arroz");

        AssertEqual("Pantry", category.Value, "Arroz should be categorized as pantry.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void ShoppingListRecordsActivityWhenItemIsAdded()
{
    var testName = "Shopping list records activity when item is added";

    try
    {
        var user = new User("Test User", "testuser", "Testing1!");
        var shoppingList = new ShoppingList("Test list", user);

        shoppingList.AddItem("Milk", 1, Unit.Liter, user);

        AssertTrue(shoppingList.Activities.Count > 0, "Adding an item should record activity.");
        AssertEqual("Test User added Milk.", shoppingList.Activities[0].Description, "Activity should describe the action.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void UsernameUtilsSuggestsFirstInitialAndLastName()
{
    var testName = "Username utils suggests first initial and last name";

    try
    {
        var users = new List<User>();
        var username = UsernameUtils.SuggestUsername("John Doe", users);

        AssertEqual("jdoe", username, "Username should use first initial and last name.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void ProductCatalogFindsArroz()
{
    var testName = "Product catalog finds arroz";

    try
    {
        var items = ProductCatalog.Search("arroz");

        AssertTrue(items.Count > 0, "Product catalog should find arroz.");
        AssertEqual("Arroz", items[0].Name, "First arroz match should be Arroz.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void ProductCatalogUsesRealisticUnits()
{
    var testName = "Product catalog uses realistic units";

    try
    {
        AssertProductUnit("Arroz", Unit.Kilogram);
        AssertProductUnit("Leite", Unit.Liter);
        AssertProductUnit("Atum", Unit.Can);
        AssertProductUnit("Papel higienico", Unit.Roll);
        AssertProductUnit("Ovos", Unit.Dozen);

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void AccountRejectsUsernameWithSpaces()
{
    var testName = "Account rejects username with spaces";

    try
    {
        var exceptionWasThrown = false;

        try
        {
            new Account("bad username", "Testing1!");
        }
        catch (InvalidAccountException ex)
        {
            exceptionWasThrown = true;
            AssertEqual("Username cannot contain spaces.", ex.Message, "Exception message should explain the rule.");
        }

        AssertTrue(exceptionWasThrown, "Expected InvalidAccountException.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void NameRejectsFirstNameWithOneCharacter()
{
    var testName = "Name rejects first name with one character";

    try
    {
        var exceptionWasThrown = false;

        try
        {
            new Name("J", "Doe");
        }
        catch (DomainException ex)
        {
            exceptionWasThrown = true;
            AssertEqual("First name must be at least 2 characters long.", ex.Message, "Exception message should explain the rule.");
        }

        AssertTrue(exceptionWasThrown, "Expected DomainException.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void CategoryRejectsNamesThatAreTooLong()
{
    var testName = "Category rejects names that are too long";

    try
    {
        var exceptionWasThrown = false;

        try
        {
            new Category("This category name is intentionally very very long");
        }
        catch (DomainException ex)
        {
            exceptionWasThrown = true;
            AssertEqual("Category name must be at most 30 characters long.", ex.Message, "Exception message should explain the rule.");
        }

        AssertTrue(exceptionWasThrown, "Expected DomainException.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void CategoryRuleRejectsEmptyWords()
{
    var testName = "Category rule rejects empty words";

    try
    {
        var exceptionWasThrown = false;

        try
        {
            var rule = new CategoryRule("Pantry");
            rule.AddWord("");
        }
        catch (DomainException ex)
        {
            exceptionWasThrown = true;
            AssertEqual("Category rule word is required.", ex.Message, "Exception message should explain the rule.");
        }

        AssertTrue(exceptionWasThrown, "Expected DomainException.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void ActivityRejectsEmptyDescription()
{
    var testName = "Activity rejects empty description";

    try
    {
        var exceptionWasThrown = false;

        try
        {
            new Activity("");
        }
        catch (DomainException ex)
        {
            exceptionWasThrown = true;
            AssertEqual("Activity description is required.", ex.Message, "Exception message should explain the rule.");
        }

        AssertTrue(exceptionWasThrown, "Expected DomainException.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void ShoppingListRejectsInvalidRole()
{
    var testName = "Shopping list rejects invalid role";

    try
    {
        var exceptionWasThrown = false;

        try
        {
            var owner = new User("Owner User", "owneruser", "Testing1!");
            var member = new User("Member User", "memberuser", "Testing1!");
            var shoppingList = new ShoppingList("Test list", owner);

            shoppingList.ShareWith(member, (ShoppingListRole)99, owner);
        }
        catch (DomainException ex)
        {
            exceptionWasThrown = true;
            AssertEqual("Invalid shopping list role.", ex.Message, "Exception message should explain the rule.");
        }

        AssertTrue(exceptionWasThrown, "Expected DomainException.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void ShoppingListItemRejectsInvalidUnit()
{
    var testName = "Shopping list item rejects invalid unit";

    try
    {
        var exceptionWasThrown = false;

        try
        {
            var owner = new User("Owner User", "owneruser", "Testing1!");
            var shoppingList = new ShoppingList("Test list", owner);

            shoppingList.AddItem("Milk", 1, (Unit)99, owner);
        }
        catch (DomainException ex)
        {
            exceptionWasThrown = true;
            AssertEqual("Invalid unit.", ex.Message, "Exception message should explain the rule.");
        }

        AssertTrue(exceptionWasThrown, "Expected DomainException.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void ShoppingListItemRejectsNullCategory()
{
    var testName = "Shopping list item rejects null category";

    try
    {
        var exceptionWasThrown = false;

        try
        {
            var owner = new User("Owner User", "owneruser", "Testing1!");
            var shoppingList = new ShoppingList("Test list", owner);

            shoppingList.AddItem("Milk", 1, Unit.Liter, null, owner);
        }
        catch (DomainException ex)
        {
            exceptionWasThrown = true;
            AssertEqual("Category is required.", ex.Message, "Exception message should explain the rule.");
        }

        AssertTrue(exceptionWasThrown, "Expected DomainException.");

        PassTest(testName);
    }
    catch (Exception ex)
    {
        FailTest(testName, ex);
    }
}

void AssertProductUnit(string productName, Unit expectedUnit)
{
    var products = ProductCatalog.Search(productName);

    AssertTrue(products.Count > 0, "Product should exist: " + productName);
    AssertEqual(expectedUnit.ToString(), products[0].Unit.ToString(), "Product unit should be realistic.");
}

void AssertEqual(string expected, string actual, string message)
{
    if (expected != actual)
    {
        throw new Exception(message + " Expected: " + expected + " Actual: " + actual);
    }
}

void AssertTrue(bool condition, string message)
{
    if (!condition)
    {
        throw new Exception(message);
    }
}

void PassTest(string testName)
{
    Console.WriteLine("[PASS] " + testName);
}

void FailTest(string testName, Exception ex)
{
    failedTests++;
    Console.WriteLine("[FAIL] " + testName);
    Console.WriteLine("       " + ex.Message);
}
