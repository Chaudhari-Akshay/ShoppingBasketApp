namespace Shopping;

public sealed class ProductCatalog
{
    private readonly Dictionary<string, Product> _products = new(StringComparer.OrdinalIgnoreCase);

    public ProductCatalog(IEnumerable<Product> products)
    {
        ArgumentNullException.ThrowIfNull(products);

        foreach (var product in products)
        {
            if (!_products.TryAdd(product.Name, product))
                throw new ArgumentException($"Duplicate product '{product.Name}' in catalog.", nameof(products));
        }
    }

    /// <summary>
    /// The stock list from the brief.
    /// </summary>
    public static ProductCatalog Default() => new(new[]
    {
        new Product("Apple", 35, NoOffer.Instance),
        new Product("Banana", 20, NoOffer.Instance),
        new Product("Melon", 50, new MultiBuyOffer(groupSize: 2, paidItems: 1)),   // buy one get one free
        new Product("Lime", 15, new MultiBuyOffer(groupSize: 3, paidItems: 2)),    // three for the price of two
    });

    public Product Find(string name)
    {
        if (name is null)
            throw new ArgumentNullException(nameof(name), "Basket contains a null item.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Basket contains an empty item name.", nameof(name));

        if (!_products.TryGetValue(name.Trim(), out var product))
            throw new ArgumentException($"Unknown item '{name}'.", nameof(name));

        return product;
    }
}
