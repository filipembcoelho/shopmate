using Rumos.ShopMate.Domain.Model;

namespace Rumos.ShopMate.Domain.Utils;

public static class CategoryUtils
{
    public static Category GuessCategory(string itemName)
    {
        var rules = CreateRules();

        if (string.IsNullOrWhiteSpace(itemName))
        {
            return new Category("Other");
        }

        var normalizedItemName = itemName.Trim().ToLower();

        foreach (var rule in rules)
        {
            foreach (var word in rule.Words)
            {
                if (normalizedItemName.Contains(word))
                {
                    return new Category(rule.CategoryName);
                }
            }
        }

        return new Category("Other");
    }

    private static List<CategoryRule> CreateRules()
    {
        var rules = new List<CategoryRule>();

        AddRule(rules, "Dairy", new List<string> { "milk", "leite", "cheese", "queijo", "yogurt", "iogurte", "butter", "manteiga" });
        AddRule(rules, "Fruit", new List<string> { "apple", "maca", "banana", "orange", "laranja", "pear", "pera", "grape", "uva", "morangos" });
        AddRule(rules, "Vegetables", new List<string> { "carrot", "cenoura", "lettuce", "alface", "tomato", "tomate", "potato", "batata", "onion", "cebola", "alho" });
        AddRule(rules, "Bakery", new List<string> { "bread", "pao", "croissant", "cake", "bolo" });
        AddRule(rules, "Pantry", new List<string> { "rice", "arroz", "pasta", "massa", "esparguete", "flour", "farinha", "sugar", "acucar", "coffee", "cafe", "feijao", "grao", "sal", "azeite", "oleo" });
        AddRule(rules, "Meat and Fish", new List<string> { "chicken", "frango", "beef", "vaca", "pork", "porco", "fish", "peixe", "salmon", "salmao", "atum", "fiambre" });
        AddRule(rules, "Drinks", new List<string> { "agua", "water", "sumo", "juice", "coca", "cha", "tea" });
        AddRule(rules, "Snacks", new List<string> { "bolacha", "cereal", "chocolate", "gelado", "ice cream" });
        AddRule(rules, "Cleaning", new List<string> { "soap", "sabao", "detergent", "detergente", "sponge", "esponja", "bleach" });
        AddRule(rules, "Hygiene", new List<string> { "shampoo", "pasta de dentes", "papel higienico", "guardanapos" });

        return rules;
    }

    private static void AddRule(List<CategoryRule> rules, string categoryName, List<string> words)
    {
        var rule = new CategoryRule(categoryName);

        foreach (var word in words)
        {
            rule.AddWord(word);
        }

        rules.Add(rule);
    }
}
