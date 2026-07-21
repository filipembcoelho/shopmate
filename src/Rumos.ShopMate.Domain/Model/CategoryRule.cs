using Rumos.ShopMate.Domain.Exceptions;
using Rumos.ShopMate.Domain.Model.Common;

namespace Rumos.ShopMate.Domain.Model;

public class CategoryRule(string categoryName) : AuditableEntity
{
    public string CategoryName { get; set; } = ValidateCategoryName(categoryName);
    public List<string> Words { get; set; } = new List<string>();

    public void AddWord(string word)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            throw new InvalidShoppingListItemException("Category rule word is required.");
        }

        Words.Add(word.Trim().ToLower());
    }

    private static string ValidateCategoryName(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
        {
            throw new InvalidShoppingListItemException("Category rule name is required.");
        }

        return categoryName.Trim();
    }
}
