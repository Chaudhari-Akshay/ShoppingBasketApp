using System;
using Shopping;
using Xunit;

namespace Shopping.Tests
{
    public class MultiBuyOfferTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 50)]
        [InlineData(2, 50)]
        [InlineData(3, 100)]
        [InlineData(4, 100)]
        public void TwoForOne_ChargesCorrectly(int quantity, int expectedPence)
        {
            var offer = new MultiBuyOffer(2, 1);

            Assert.Equal(expectedPence, offer.CalculatePence(quantity, 50));
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(2, 30)]
        [InlineData(3, 30)]
        [InlineData(5, 60)]
        [InlineData(6, 60)]
        public void ThreeForTwo_ChargesCorrectly(int quantity, int expectedPence)
        {
            var offer = new MultiBuyOffer(3, 2);

            Assert.Equal(expectedPence, offer.CalculatePence(quantity, 15));
        }

        [Fact]
        public void NoOffer_IsJustQuantityTimesPrice()
        {
            Assert.Equal(105, NoOffer.Instance.CalculatePence(3, 35));
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(3, 0)]
        [InlineData(3, 3)]
        [InlineData(3, 4)]
        public void InvalidOffer_IsRejected(int groupSize, int paidItems)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new MultiBuyOffer(groupSize, paidItems));
        }

        [Fact]
        public void Catalog_RejectsDuplicateProducts()
        {
            var products = new[]
            {
                new Product("Apple", 35, NoOffer.Instance),
                new Product("apple", 40, NoOffer.Instance),
            };

            Assert.Throws<ArgumentException>(() => new ProductCatalog(products));
        }

        [Fact]
        public void Basket_UsesACustomCatalog()
        {
            var catalog = new ProductCatalog(new[]
            {
                new Product("Pear", 40, new MultiBuyOffer(2, 1)),
            });
            var basket = new ShoppingBasket(catalog);

            Assert.Equal(0.40m, basket.CalculateTotal(new[] { "Pear", "Pear" }));
        }
    }
}
