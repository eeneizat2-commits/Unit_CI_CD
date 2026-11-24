namespace Domain
{
    public class DiscountCalculator
    {
        public decimal Calculate(decimal price, decimal discountPercent)
        {
            if (price < 0)
                throw new ArgumentOutOfRangeException(nameof(price));

            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentOutOfRangeException(nameof(discountPercent));

            return price - (price * discountPercent / 100m);
        }
    }
}