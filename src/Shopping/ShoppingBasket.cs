namespace Shopping;

public class ShoppingBasket
{
    private readonly ProductCatalog _catalog;

    public ShoppingBasket() : this(ProductCatalog.Default())
    {
    }

    public ShoppingBasket(ProductCatalog catalog)
    {
        _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
    }

    /// <summary>
    /// Total cost of the items in pounds. Offers are applied per product, so two
    /// melons and a lime never discount each other.
    /// </summary>
    public decimal CalculateTotal(IEnumerable<string> items)
    {
        if (items is null)
            throw new ArgumentNullException(nameof(items));

        var quantities = new Dictionary<Product, int>();

        foreach (var name in items)
        {
            var product = _catalog.Find(name);
            quantities[product] = quantities.GetValueOrDefault(product) + 1;
        }

        int totalPence = 0;
        foreach (var (product, quantity) in quantities)
            totalPence += product.PriceFor(quantity);

        return totalPence / 100m;
    }
}
