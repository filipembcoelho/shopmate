namespace Rumos.ShopMate.Domain.Utils;

public static class SuggestionUtils
{
    public static List<string> GetSuggestionsFor(string itemName)
    {
        var suggestions = new List<string>();

        if (string.IsNullOrWhiteSpace(itemName))
        {
            return suggestions;
        }

        var normalizedItemName = itemName.Trim().ToLower();

        if (normalizedItemName.Contains("milk"))
        {
            suggestions.Add("Cereal");
            suggestions.Add("Cookies");
        }
        else if (normalizedItemName.Contains("leite"))
        {
            suggestions.Add("Cereais");
            suggestions.Add("Bolachas");
        }
        else if (normalizedItemName.Contains("pasta"))
        {
            suggestions.Add("Tomato sauce");
            suggestions.Add("Cheese");
        }
        else if (normalizedItemName.Contains("massa") || normalizedItemName.Contains("esparguete"))
        {
            suggestions.Add("Molho de tomate");
            suggestions.Add("Queijo");
        }
        else if (normalizedItemName.Contains("arroz"))
        {
            suggestions.Add("Feijao");
            suggestions.Add("Atum");
        }
        else if (normalizedItemName.Contains("bread"))
        {
            suggestions.Add("Butter");
            suggestions.Add("Cheese");
        }
        else if (normalizedItemName.Contains("pao"))
        {
            suggestions.Add("Manteiga");
            suggestions.Add("Queijo");
        }
        else if (normalizedItemName.Contains("coffee"))
        {
            suggestions.Add("Sugar");
            suggestions.Add("Milk");
        }
        else if (normalizedItemName.Contains("cafe"))
        {
            suggestions.Add("Acucar");
            suggestions.Add("Leite");
        }

        return suggestions;
    }
}
