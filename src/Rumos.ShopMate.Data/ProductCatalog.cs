using Rumos.ShopMate.Domain.Model.Enums;

namespace Rumos.ShopMate.Data;

public static class ProductCatalog
{
    public static List<ProductCatalogItem> GetAll()
    {
        var items = new List<ProductCatalogItem>();

        Add(items, "Arroz", Unit.Kilogram);
        Add(items, "Massa", Unit.Package);
        Add(items, "Esparguete", Unit.Package);
        Add(items, "Feijao", Unit.Can);
        Add(items, "Grao", Unit.Can);
        Add(items, "Farinha", Unit.Kilogram);
        Add(items, "Acucar", Unit.Kilogram);
        Add(items, "Sal", Unit.Kilogram);
        Add(items, "Azeite", Unit.Bottle);
        Add(items, "Oleo", Unit.Bottle);
        Add(items, "Cafe", Unit.Package);
        Add(items, "Leite", Unit.Liter);
        Add(items, "Queijo", Unit.Package);
        Add(items, "Iogurte", Unit.Package);
        Add(items, "Manteiga", Unit.Package);
        Add(items, "Ovos", Unit.Dozen);
        Add(items, "Pao", Unit.Each);
        Add(items, "Croissant", Unit.Each);
        Add(items, "Bolo", Unit.Each);
        Add(items, "Maca", Unit.Each);
        Add(items, "Banana", Unit.Each);
        Add(items, "Laranja", Unit.Each);
        Add(items, "Pera", Unit.Each);
        Add(items, "Uvas", Unit.Kilogram);
        Add(items, "Morangos", Unit.Box);
        Add(items, "Batata", Unit.Kilogram);
        Add(items, "Cenoura", Unit.Kilogram);
        Add(items, "Alface", Unit.Each);
        Add(items, "Tomate", Unit.Kilogram);
        Add(items, "Cebola", Unit.Kilogram);
        Add(items, "Alho", Unit.Each);
        Add(items, "Frango", Unit.Kilogram);
        Add(items, "Vaca", Unit.Kilogram);
        Add(items, "Porco", Unit.Kilogram);
        Add(items, "Peixe", Unit.Kilogram);
        Add(items, "Salmao", Unit.Kilogram);
        Add(items, "Atum", Unit.Can);
        Add(items, "Fiambre", Unit.Package);
        Add(items, "Agua", Unit.Bottle);
        Add(items, "Sumo", Unit.Bottle);
        Add(items, "Coca Cola", Unit.Bottle);
        Add(items, "Cha", Unit.Box);
        Add(items, "Bolachas", Unit.Package);
        Add(items, "Cereais", Unit.Box);
        Add(items, "Chocolate", Unit.Each);
        Add(items, "Gelado", Unit.Package);
        Add(items, "Detergente", Unit.Bottle);
        Add(items, "Sabao", Unit.Package);
        Add(items, "Esponja", Unit.Package);
        Add(items, "Papel higienico", Unit.Roll);
        Add(items, "Guardanapos", Unit.Package);
        Add(items, "Shampoo", Unit.Bottle);
        Add(items, "Pasta de dentes", Unit.Each);

        return items;
    }

    public static List<ProductCatalogItem> Search(string searchText)
    {
        var results = new List<ProductCatalogItem>();
        var items = GetAll();

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return items;
        }

        var normalizedSearchText = Normalize(searchText);

        foreach (var item in items)
        {
            if (Normalize(item.Name).Contains(normalizedSearchText))
            {
                results.Add(item);
            }
        }

        return results;
    }

    private static void Add(List<ProductCatalogItem> items, string name, Unit unit)
    {
        items.Add(new ProductCatalogItem(name, unit));
    }

    private static string Normalize(string text)
    {
        return text.Trim().ToLower();
    }
}
