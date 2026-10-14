# Shopping Basket

A small C# (.NET 8) program that works out the total price of a basket of shopping.

Items are passed in as a list of names, e.g. `["Apple", "Apple", "Banana"]`.

| Item   | Price | Offer                       |
|--------|-------|-----------------------------|
| Apple  | 35p   | none                        |
| Banana | 20p   | none                        |
| Melon  | 50p   | buy one get one free        |
| Lime   | 15p   | three for the price of two  |

## Running it

Needs the .NET 8 SDK.

```bash
# run the tests
dotnet test

# price a basket from the command line
dotnet run --project src/Shopping.App -- Apple Apple Banana
# Total: £0.90

# or run it with no arguments and type the items in
dotnet run --project src/Shopping.App
```

## Layout

```
src/Shopping          the library (all the logic lives here)
src/Shopping.App      tiny console front end
tests/Shopping.Tests  xUnit tests
```

## Design

The main thing I wanted was for the offers to be easy to change without touching the
basket code, since "add another promotion" is the obvious next request on a task like this.

```
ShoppingBasket ──uses──▶ ProductCatalog ──holds──▶ Product ──has──▶ IOffer
                                                                      ▲
                                                          ┌───────────┴──────────┐
                                                       NoOffer             MultiBuyOffer
```

**ShoppingBasket**
Takes the list of item names, counts how many of each product there are, then asks each
product what that quantity costs and adds it all up. It knows nothing about specific
offers or prices.

**ProductCatalog**
The list of products we sell. Looks products up by name, ignoring case and surrounding
spaces. `ProductCatalog.Default()` holds the four products from the brief. A different
catalog can be passed into the basket, which also makes it easy to test with.

**Product**
Name, unit price in pence and an offer.

**IOffer**
One method: given a quantity and a unit price, return the cost in pence. `NoOffer` is just
quantity times price. `MultiBuyOffer(groupSize, paidItems)` covers both promotions:

- Melons: `MultiBuyOffer(2, 1)`: every pair costs one melon.
- Limes: `MultiBuyOffer(3, 2)`: every three cost two limes.

Items left over after the full groups are charged at full price, so 5 limes is one group
of three (30p) plus two singles (30p) = 60p.

A new kind of promotion (say, 10% off over 5 items) is a new `IOffer` class and one line
in the catalog.

### Decisions and assumptions

- **Money is stored as whole pence (`int`)** and only converted to `decimal` pounds at the
  very end. This avoids floating point rounding problems.
- **Offers apply per product.** Two melons and a lime don't discount each other.
- **The order of the list doesn't matter.** Items are counted first, then priced, so
  `["Lime", "Melon", "Lime", "Lime"]` gets the lime offer even though they aren't next to
  each other.
- **Item names are case-insensitive** (`"apple"` works) and surrounding whitespace is ignored.
  The brief doesn't say either way; being forgiving seemed more useful than rejecting it.
- **Bad input throws rather than guessing.** A null list or null item gives
  `ArgumentNullException`; an empty/whitespace name or an unknown item gives
  `ArgumentException`. Silently pricing an unknown item at 0 would hide mistakes.
- **An empty basket costs £0.00.**
- **The input list is never modified.**
- The brief says "roughly two hours", so I kept it small and left out things like
  quantities per line, currencies, stacking offers or a database.

## Tests

`ShoppingBasketTests` covers the behaviour from the brief:

- single items, an empty basket and plain multiples
- melon and lime offers at different quantities (1 up to 10 melons, 1 up to 9 limes),
  including the leftover items that don't make a full group
- mixed baskets with both offers at once, offers not leaking between products
- order independence, repeat calls, input list not being changed
- bad input: null list, null item, unknown item, case-insensitive names

`MultiBuyOfferTests` covers the offer classes and catalog on their own, including
invalid offer definitions and a custom catalog.
