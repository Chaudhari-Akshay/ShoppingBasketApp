namespace Shopping;

public sealed record Product(string Name, int UnitPricePence, IOffer Offer)
{
    public int PriceFor(int quantity) => Offer.CalculatePence(quantity, UnitPricePence);
}
