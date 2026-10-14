namespace Shopping;

/// <summary>
/// "Take groupSize, pay for paidItems". Covers both buy-one-get-one-free (2 for 1)
/// and three-for-the-price-of-two (3 for 2). Leftover items that don't fill a group
/// are charged at full price.
/// </summary>
public sealed class MultiBuyOffer : IOffer
{
    private readonly int _groupSize;
    private readonly int _paidItems;

    public MultiBuyOffer(int groupSize, int paidItems)
    {
        if (groupSize < 2)
            throw new ArgumentOutOfRangeException(nameof(groupSize), "A group needs at least two items.");
        if (paidItems < 1 || paidItems >= groupSize)
            throw new ArgumentOutOfRangeException(nameof(paidItems), "Paid items must be at least 1 and less than the group size.");

        _groupSize = groupSize;
        _paidItems = paidItems;
    }

    public int CalculatePence(int quantity, int unitPricePence)
    {
        int fullGroups = quantity / _groupSize;
        int leftovers = quantity % _groupSize;
        int chargedItems = fullGroups * _paidItems + leftovers;

        return chargedItems * unitPricePence;
    }
}
