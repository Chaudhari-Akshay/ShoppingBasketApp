using Shopping;

// Usage:
//   dotnet run --project src/Shopping.App -- Apple Apple Banana
//   dotnet run --project src/Shopping.App            (then type items separated by commas)

var items = args.Length > 0
    ? args.ToList()
    : ReadItemsFromConsole();

try
{
    var total = new ShoppingBasket().CalculateTotal(items);
    Console.WriteLine($"Total: £{total:0.00}");
    return 0;
}
catch (ArgumentException ex)
{
    Console.Error.WriteLine(ex.Message);
    return 1;
}

static List<string> ReadItemsFromConsole()
{
    Console.Write("Items (comma separated, e.g. Apple, Apple, Banana): ");
    var line = Console.ReadLine() ?? string.Empty;

    return line
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .ToList();
}
