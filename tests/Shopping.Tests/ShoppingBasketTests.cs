using System;
using System.Collections.Generic;
using Xunit;

namespace Shopping.Tests
{
    public class ShoppingBasketTests
    {
        private readonly ShoppingBasket _basket = new ShoppingBasket();

        private decimal Total(params string[] items)
        {
            return _basket.CalculateTotal(new List<string>(items));
        }

        // basic cases

        [Fact]
        public void EmptyBasket_CostsNothing()
        {
            Assert.Equal(0m, Total());
        }

        [Fact]
        public void SingleApple_Is35p()
        {
            Assert.Equal(0.35m, Total("Apple"));
        }

        [Fact]
        public void SingleBanana_Is20p()
        {
            Assert.Equal(0.20m, Total("Banana"));
        }

        [Fact]
        public void SingleMelon_Is50p()
        {
            Assert.Equal(0.50m, Total("Melon"));
        }

        [Fact]
        public void SingleLime_Is15p()
        {
            Assert.Equal(0.15m, Total("Lime"));
        }

        [Fact]
        public void TwoApplesAndABanana()
        {
            Assert.Equal(0.90m, Total("Apple", "Apple", "Banana"));
        }

        [Fact]
        public void ManyApples_NoDiscount()
        {
            Assert.Equal(3.50m, Total("Apple", "Apple", "Apple", "Apple", "Apple",
                                      "Apple", "Apple", "Apple", "Apple", "Apple"));
        }

        // melons - buy one get one free

        [Theory]
        [InlineData(1, 0.50)]
        [InlineData(2, 0.50)]
        [InlineData(3, 1.00)]
        [InlineData(4, 1.00)]
        [InlineData(5, 1.50)]
        [InlineData(10, 2.50)]
        public void Melons_BuyOneGetOneFree(int count, double expected)
        {
            var items = new string[count];
            for (int i = 0; i < count; i++) items[i] = "Melon";

            Assert.Equal((decimal)expected, Total(items));
        }

        // limes - three for the price of two

        [Theory]
        [InlineData(1, 0.15)]
        [InlineData(2, 0.30)]
        [InlineData(3, 0.30)]
        [InlineData(4, 0.45)]
        [InlineData(5, 0.60)]
        [InlineData(6, 0.60)]
        [InlineData(7, 0.75)]
        [InlineData(9, 0.90)]
        public void Limes_ThreeForTwo(int count, double expected)
        {
            var items = new string[count];
            for (int i = 0; i < count; i++) items[i] = "Lime";

            Assert.Equal((decimal)expected, Total(items));
        }

        // mixed baskets

        [Fact]
        public void MixedBasket_AllItemTypes()
        {
            // 2 apples (70) + 1 banana (20) + 2 melons (50) + 3 limes (30) = 170
            var total = Total("Apple", "Melon", "Lime", "Banana", "Lime", "Melon", "Apple", "Lime");
            Assert.Equal(1.70m, total);
        }

        [Fact]
        public void MixedBasket_OffersAreNotAppliedAcrossDifferentItems()
        {
            // one melon and two limes, nothing should be free here
            Assert.Equal(0.80m, Total("Melon", "Lime", "Lime"));
        }

        [Fact]
        public void MixedBasket_BothOffersApplyAtOnce()
        {
            // 3 melons (100) + 4 limes (45) = 145
            Assert.Equal(1.45m, Total("Melon", "Melon", "Melon", "Lime", "Lime", "Lime", "Lime"));
        }

        [Fact]
        public void OrderOfItems_DoesNotChangeTotal()
        {
            var a = Total("Lime", "Melon", "Apple", "Lime", "Banana", "Lime", "Melon");
            var b = Total("Melon", "Melon", "Lime", "Lime", "Lime", "Banana", "Apple");

            Assert.Equal(a, b);
        }

        [Fact]
        public void SameBasketTwice_GivesSameTotal()
        {
            var items = new List<string> { "Melon", "Melon", "Melon", "Lime" };

            var first = _basket.CalculateTotal(items);
            var second = _basket.CalculateTotal(items);

            Assert.Equal(first, second);
        }

        [Fact]
        public void CalculateTotal_DoesNotModifyTheList()
        {
            var items = new List<string> { "Apple", "Melon", "Melon", "Lime" };

            _basket.CalculateTotal(items);

            Assert.Equal(4, items.Count);
            Assert.Equal("Apple", items[0]);
        }

        // bad input - adjust these to match what your implementation actually does

        [Fact]
        public void NullList_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _basket.CalculateTotal(null));
        }

        [Fact]
        public void UnknownItem_Throws()
        {
            Assert.Throws<ArgumentException>(() => Total("Apple", "Pineapple"));
        }

        [Fact]
        public void NullItemInList_Throws()
        {
            Assert.ThrowsAny<ArgumentException>(() => Total("Apple", null));
        }

        [Theory]
        [InlineData("apple")]
        [InlineData("APPLE")]
        public void ItemNames_AreCaseInsensitive(string name)
        {
            // only keep this one if you treat names as case-insensitive
            Assert.Equal(0.35m, Total(name));
        }
    }
}
