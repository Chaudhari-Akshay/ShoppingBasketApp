namespace Shopping;

/// <summary>
/// Works out what a given quantity of one product costs, in pence.
/// New promotions are added by implementing this, nothing else needs to change.
/// </summary>
public interface IOffer
{
    int CalculatePence(int quantity, int unitPricePence);
}
