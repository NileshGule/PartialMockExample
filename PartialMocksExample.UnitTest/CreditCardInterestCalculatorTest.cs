using Microsoft.VisualStudio.TestTools.UnitTesting;

using Rhino.Mocks;

namespace PartialMocksExample.UnitTest
{
    [TestClass]
    public class PhoneBillCalculatorTest
    {
        [TestMethod]
        public void GenerateBill_WithCustomer_GeneratesBill()
        {
            Customer customer = CreateCustomer("Normal", 170.50);

            PhoneBillCalculator billCalculator = CreatePhoneBillCalculator();

            PhoneBill expectedBill = new PhoneBill();

            billCalculator.Expect(x => x.GenerateBillWithDefaultValues(customer)).Return(expectedBill);

            billCalculator.Expect(x => x.CalculateDiscount(customer, expectedBill));

            billCalculator.Expect(x => x.CalculateTotalDueAmount(expectedBill));

            PhoneBill phoneBill = billCalculator.GenerateBill(customer);

            billCalculator.VerifyAllExpectations();

            Assert.IsNotNull(phoneBill);
        }

        [TestMethod]
        public void GenerateBill_WithCustomerTypeAsNormal_ApplyNoDiscountOnTotalBilledAmount()
        {
            Customer customer = CreateCustomer("Normal", 170.50);

            PhoneBillCalculator billCalculator = CreatePhoneBillCalculator();

            PhoneBill phoneBill = billCalculator.GenerateBill(customer);

            Assert.IsNotNull(phoneBill);
            Assert.AreEqual("Normal", phoneBill.CustomerType);
            Assert.AreEqual(170.50, phoneBill.BilledAmount);
            Assert.AreEqual(0, phoneBill.DiscountedAmount);
            Assert.AreEqual(170.50, phoneBill.TotalDueAmount);
        }

        [TestMethod]
        public void GenerateBill_WithCustomerTypeAsCorporate_ApplyCorporateDiscountOnTotalBilledAmount()
        {
            Customer customer = CreateCustomer("Corporate", 170.50);

            PhoneBillCalculator billCalculator = CreatePhoneBillCalculator();

            PhoneBill phoneBill = billCalculator.GenerateBill(customer);

            Assert.IsNotNull(phoneBill);
            Assert.AreEqual("Corporate", phoneBill.CustomerType);
            Assert.AreEqual(170.50, phoneBill.BilledAmount);
            Assert.AreEqual(42.63, phoneBill.DiscountedAmount);
            Assert.AreEqual(127.87, phoneBill.TotalDueAmount);
        }

        private static Customer CreateCustomer(string customerType, double billedAmount)
        {
            return new Customer
            {
                CustomerType = customerType,
                BilledAmount = billedAmount
            };
        }

        private PhoneBillCalculator CreatePhoneBillCalculator()
        {
            return MockRepository.GeneratePartialMock<PhoneBillCalculator>();
        }
    }
}