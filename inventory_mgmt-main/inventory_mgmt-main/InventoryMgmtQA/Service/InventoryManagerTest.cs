using InventoryMgmt.Model;
using InventoryMgmt.Service;
using InventoryMgmt.Interface;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json.Bson;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;

// guide: https://www.aligrant.com/web/blog/2020-07-20_capturing_console_outputs_with_microsoft_test_framework

namespace InventoryMgmtQA.Service
{
    [TestClass]
    public class InventoryManagerTest
    {
        private IInventoryManager _inventoryManager = new InventoryManager();

        [TestMethod]
        public void TestAddProduct()
        {
            using (StringWriter sw = new StringWriter())
            {
                // capture console output
                Console.SetOut(sw);

                // create a new product with valid attribute values
                _inventoryManager.AddNewProduct(
                    "TestProduct",
                    1,
                    1.23M
                );

                // console output should contain 'success'
                Assert.IsTrue(sw.ToString().Contains("success"));
            }
        }

        [TestMethod] 
        public void TestAddProductPriceNegative()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                _inventoryManager.AddNewProduct(
                    "TestProduct",
                    1,
                    -1.0M
                );
                Assert.IsFalse(sw.ToString().Contains("success"));
            }
        }

        [TestMethod]
        public void TestGetTotalValue()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                _inventoryManager.AddNewProduct(
                    "TestProduct",
                    1,
                    2.56M
                );
                _inventoryManager.GetTotalValue();
                Assert.IsTrue(sw.ToString().Contains("2.56"));
            }
        }

        // Added test cases ->

        [TestMethod]
        public void TestRemoveProduct()
        {

            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);  // Add a product to ensure it can be removed
                _inventoryManager.AddNewProduct("Test Product", 1, 3.5M);

                _inventoryManager.RemoveProduct(1);  // Remove the product with a valid ProductID (assuming ID 1)

                Assert.IsTrue(sw.ToString().Contains("Product removed successfully."));

                sw.GetStringBuilder().Clear();  // Attempt to remove a non-existent product (ID 99)
                _inventoryManager.RemoveProduct(99);

                Assert.IsTrue(sw.ToString().Contains("Product not found, please try again."));
            }
        }

        [TestMethod]
        public void TestAddProductQuantity()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw); // Add a product to ensure there is a product to update
                _inventoryManager.AddNewProduct("Test Product", 1, 5M);

                sw.GetStringBuilder().Clear(); // Test case for updating with a negative quantity
                _inventoryManager.UpdateProduct(1, -3);
                Assert.IsTrue(sw.ToString().Contains("Quantity should not be less than 0"));

                sw.GetStringBuilder().Clear(); // Test case for updating with alphabetic characters in quantity (invalid input)
                try
                {
                    _inventoryManager.UpdateProduct(1, Convert.ToInt32("AAA"));
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
                Assert.IsTrue(sw.ToString().Contains("Invalid input, please try again."));

                sw.GetStringBuilder().Clear(); // Test case for updating with special characters in quantity (invalid input)
                try
                {
                    _inventoryManager.UpdateProduct(1, Convert.ToInt32("@@@"));
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
                Assert.IsTrue(sw.ToString().Contains("Invalid input, please try again."));
            }
        }

        [TestMethod]
        public void TestGetTotalValueEmptyInventory()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Attempt to get the total value when no products are in inventory
                _inventoryManager.GetTotalValue();

                // Ensure the system returns a total value of 0 or indicates an empty inventory
                Assert.IsTrue(sw.ToString().Contains("Total value of inventory: 0"));
            }
        }

        [TestMethod]
        public void TestUpdateProductNonExistentID()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Attempt to update a product that doesn't exist
                _inventoryManager.UpdateProduct(999, 10);

                // Console output should indicate that the product is not found
                Assert.IsTrue(sw.ToString().Contains("Product not found, please try again."));
            }
        }

        [TestMethod]
        public void CalculateTotalValueMultipleProducts()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Add multiple products
                _inventoryManager.AddNewProduct("Product1", 1, 10.0M); // 20.0 total value
                _inventoryManager.AddNewProduct("Product2", 2, 5.0M);  // 15.0 total value
                _inventoryManager.AddNewProduct("Product3", 3, 25.0M); // 25.0 total value

                // Calculate total value of inventory
                _inventoryManager.GetTotalValue();

                // Expect the total value to be 95.0 (10.0 + 10.0 + 75.0)
                Assert.IsTrue(sw.ToString().Contains("95.0"));
            }
        }
    }
            
}