using InventoryMgmt.Model;
using System.ComponentModel.DataAnnotations;

// guide: https://learn.microsoft.com/en-us/visualstudio/test/walkthrough-creating-and-running-unit-tests-for-managed-code?view=vs-2022

namespace InventoryMgmtQA.Model
{
    [TestClass]
    public class ProductTest
    {
        [TestMethod]
        public void TestAddProduct()
        {
            // create a new product with compliant attribute values
            Product product = new()
            {
                Name = "TestProduct",
                QuantityInStock = 1,
                Price = 1.0M
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            bool isProductValid = Validator.TryValidateObject(product, context, results, true);

            // the product must be valid since all attributes values validated correctly
            Assert.IsTrue(isProductValid);
        }
        [TestMethod]
        public void TestAddProductPriceNegative()
        {
            Product product = new()
            {
                Name = "TestProduct",
                QuantityInStock = 1,
                Price = -1.0M // test for negative price
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            bool isProductValid = Validator.TryValidateObject(product, context, results, true);

            // the product must NOT be valid since the Price attribute has invalid value
            Assert.IsFalse(isProductValid);
        }

        // add more test cases here
        
        // Added test cases ->

        [TestMethod]
        public void TestAddProductEmptyName()
        {
            // Test with an empty Name value
            Product product = new()
            {
                // Name should not be empty but in this scenario, it is empty
                Name = "", QuantityInStock = 1, Price = 1.0M
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            bool isProductValid = Validator.TryValidateObject(product, context, results, true);

            // The product must NOT be valid since the Name attribute is empty
            Assert.IsFalse(isProductValid);
        }
        
        [TestMethod]
        public void TestAddProductZeroQuantity()
        {
            // Test with QuantityInStock set to zero (boundary value)
            Product product = new()
            {
                Name = "TestProduct",
                QuantityInStock = 0, // Quantity should be an integer - zero or more
                Price = 1.0M
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            bool isProductValid = Validator.TryValidateObject(product, context, results, true);

            // The product should be valid as zero is an acceptable quantity based on the business rules
            Assert.IsTrue(isProductValid);
        }
        
        [TestMethod]
        public void TestAddProductMaxQuantity()
        {
            // Test with QuantityInStock set to maximum integer value
            Product product = new()
            {
                Name = "TestProduct",
                QuantityInStock = int.MaxValue, // Maximum possible quantity
                Price = 1.0M
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            bool isProductValid = Validator.TryValidateObject(product, context, results, true);

            // The product should be valid as max integer is an acceptable quantity
            Assert.IsTrue(isProductValid);
        }

        [TestMethod]
        public void TestAddProductHighPrice()
        {
            // Test with an extremely high price (near the upper limit for decimals)
            Product product = new()
            {
                Name = "ExpensiveProduct",
                QuantityInStock = 1,
                Price = 79228162514264337593543950335M // Max limit for decimal
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            bool isProductValid = Validator.TryValidateObject(product, context, results, true);

            // The product should be valid as this is within the allowed price range
            Assert.IsTrue(isProductValid);
        }

        [TestMethod]
        public void TestAddProductWhitespaceName()
        {
            // Test with a whitespace-only Name
            Product product = new()
            {
                Name = "   ", // Name should not be empty or whitespace only
                QuantityInStock = 1,
                Price = 1.0M
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            bool isProductValid = Validator.TryValidateObject(product, context, results, true);

            // The product must NOT be valid since the Name is just whitespace
            Assert.IsFalse(isProductValid);
        }

        [TestMethod]
        public void TestAddProductBoundaryQuantityAndPrice()
        {
            // Test with boundary values for QuantityInStock and Price (both at minimum valid values)
            Product product = new()
            {
                Name = "BoundaryProduct",
                QuantityInStock = 0, // Minimum quantity allowed
                Price = 0.0M // Minimum price allowed
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(product, null, null);
            bool isProductValid = Validator.TryValidateObject(product, context, results, true);

            // The product should be valid as both quantity and price meet minimum requirements
            Assert.IsTrue(isProductValid);
        }
    }
}