using System;

namespace PartialMocksExample
{
    public class PhoneBillCalculator
    {
        private const int ROUNDED_DIGITS = 2;

        private const double CORPORATE_DISCOUNT_PERCENTAGE = 0.25;

        public PhoneBill GenerateBill(Customer customer)
        {
            PhoneBill generateBill = GenerateBillWithDefaultValues(customer);

            CalculateDiscount(customer, generateBill);

            CalculateTotalDueAmount(generateBill);

            return generateBill;
        }

        internal virtual void CalculateDiscount(Customer customer, PhoneBill generateBill)
        {
            switch (customer.CustomerType)
            {
                case "Normal":
                    generateBill.DiscountedAmount = 0;
                    break;
                case "Corporate":
                    generateBill.DiscountedAmount = Math.Round(
                        customer.BilledAmount * CORPORATE_DISCOUNT_PERCENTAGE, ROUNDED_DIGITS, MidpointRounding.AwayFromZero);
                    break;
            }
        }

        internal virtual void CalculateTotalDueAmount(PhoneBill generateBill)
        {
            double totalDueAmount = generateBill.BilledAmount - generateBill.DiscountedAmount;

            generateBill.TotalDueAmount = Math.Round(totalDueAmount, ROUNDED_DIGITS, MidpointRounding.AwayFromZero);
        }

        internal virtual PhoneBill GenerateBillWithDefaultValues(Customer customer)
        {
            return new PhoneBill
                {
                    CustomerType = customer.CustomerType,
                    BilledAmount = customer.BilledAmount
                };
        }
    }
}