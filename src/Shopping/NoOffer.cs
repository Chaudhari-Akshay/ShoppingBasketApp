namespace Shopping;

public sealed class NoOffer : IOffer
{
    public static readonly NoOffer Instance = new();

    private NoOffer()
    {
    }

    public int CalculatePence(int quantity, int unitPricePence) => quantity * unitPricePence;
}
