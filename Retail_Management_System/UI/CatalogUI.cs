using RMS.BL;
using RMS.DL;
using RMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RMS.UI
{
    internal class CatalogUI
    {
        private CatalogManager catalogManager = new CatalogManager();

        // =========================
        // CATEGORY
        // =========================

        private void AddCategoryUI()
        {
            Console.Write("Enter Category Name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty or whitespace.");
                return;
            }

            Category category = new Category(name);

            try
            {
                catalogManager.AddCategory(category);
                Console.WriteLine("\nCategory added successfully!");
            }
            catch(Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }            
        }
        private void UpdateCategoryUI()
        {
            int id = GetCategoryByID();
            if (id == -1)
                return;

            Console.Write("Enter new Category Name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Category name cannot be empty!");
                return;
            }

            try
            {
                catalogManager.UpdateCategory(new Category(id, name));
                Console.WriteLine("\nCategory updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }
        private void DeleteCategoryUI()
        {
            int id = GetCategoryByID();
            if (id == -1)
                return;

            Console.Write("Are you sure you want to delete (Y/N): ");
            string c = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(c))
            {
                Console.WriteLine("Invalid input!");
                return;
            }

            if (c == "n")
            {
                Console.WriteLine("Cancel Delete");
                return;
            }
            else if (c != "y")
            {
                Console.WriteLine("Invalid input! Use Y or N.");
                return;
            }

            try
            {
                catalogManager.DeleteCategory(id);
                Console.WriteLine("\nCategory Deleted Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }
        private int GetCategoryByID()
        {
            int id = ReadInt("Enter Category ID: ");

            try
            {
                Category category = catalogManager.GetCategoryById(id);

                if (category != null)
                {
                    Console.WriteLine();
                    Console.WriteLine("Category ID: " + category.CategoryId);
                    Console.WriteLine("Category Name: " + category.CategoryName);
                    Console.WriteLine("Total Products: " + catalogManager.ProductsInCategory(category.CategoryId));
                }
                else
                {
                    Console.WriteLine("\nCategory not found!");
                    return -1;
                }
                   
            } 
            catch (Exception ex)
            { 
                Console.WriteLine("\nError: " + ex.Message);
                return -1;
            } 
            return id; 
        }
        private void GetCategoryByName()
        {
            Console.WriteLine("Enter Category Name: ");
            string categoryName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                Console.WriteLine("Name cannot be empty or whitespace.");
                return;
            }

            try
            {
                Category category = catalogManager.GetCategoryByName(categoryName);

                if (category != null)
                {
                    Console.WriteLine();
                    Console.WriteLine("Category ID: " + category.CategoryId);
                    Console.WriteLine("Category Name: " + category.CategoryName);
                    Console.WriteLine("Total Products: " + catalogManager.ProductsInCategory(category.CategoryId));
                }
                else
                    Console.WriteLine("\nCategory not found!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }
        private void ViewCategoriesUI()
        {
            List<Category> list = catalogManager.GetAllCategories();

            Console.WriteLine("\n         --- CATEGORY LIST ---              ");
            Console.WriteLine("{0,-5} {1,-30} {2,-10}", "ID", "Name", "Products");
            Console.WriteLine("----------------------------------------------");

            foreach (Category c in list)
            {
                Console.WriteLine("{0,-5} {1,-30} {2,-10}",
                    c.CategoryId,
                    c.CategoryName,
                    catalogManager.ProductsInCategory(c.CategoryId));
            }
        }

        // =========================
        // PRODUCT
        // =========================

        private void AddProductUI()
        {
            Console.WriteLine();
            Console.WriteLine("     --- Available Categories ---          ");
           
            ViewCategoriesUI();
            Console.WriteLine();

            int categoryId = ReadInt("Enter Category ID: ");

            Console.Write("Enter Product Name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty or whitespace.");
                return;
            }

            double originalPrice = ReadDouble("Enter Original Price: ");
            double sellingPrice = ReadDouble("Enter Selling Price: ");
            int quantity = ReadInt("Enter Quantity: ");

            Product product = new Product(name, categoryId, originalPrice, sellingPrice, quantity);

            try
            {
                catalogManager.AddProduct(product);
                Console.WriteLine("\nProduct added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }
        private void UpdateProductUI()
        {
            int id = GetProductByID();

            if (id == -1)
                return;

            Console.Write("\nEnter New Product Name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty or whitespace.");
                return;
            }

            int categoryId = ReadInt("Enter New Category ID: ");
            double originalPrice = ReadDouble("Enter New Original Price: ");
            double sellingPrice = ReadDouble("Enter New Selling Price: ");
            int quantity = ReadInt("Enter New Quantity: ");

            Product product = new Product(id, name, categoryId, originalPrice, sellingPrice, quantity);

            try
            {
                catalogManager.UpdateProduct(product);
                Console.WriteLine("\nProduct updated successfully!");
            } 
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }
        private void DeleteProductUI()
        {
            int id = GetProductByID();

            if (id == -1)
                return;

            Console.Write("Are you sure you want to delete (Y/N): ");
            string c = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(c))
            {
                Console.WriteLine("Invalid input!");
                return;
            }

            if (c == "n")
            {
                Console.WriteLine("Cancel Delete");
                return;
            }
            else if (c != "y")
            {
                Console.WriteLine("Invalid input! Use Y or N.");
                return;
            }

            try
            {
                catalogManager.DeleteProduct(id);
                Console.WriteLine("\nProduct deleted successfully!");
            }
            catch(Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }           
        }
        private int GetProductByID()
        {
            int id = ReadInt("Enter Product ID: ");

            try
            {
                Product product = catalogManager.GetProductById(id);

                if (product != null)
                {
                    Console.WriteLine("\n----- PRODUCT DETAILS -----");
                    Console.WriteLine($"Name: {product.ProductName}");
                    Console.WriteLine($"Category ID: {product.CategoryId}");
                    Console.WriteLine($"Original Price: {product.OriginalPrice}");
                    Console.WriteLine($"Selling Price: {product.SellingPrice}");
                    Console.WriteLine($"Quantity: {product.Quantity}\n");
                    return id;
                }
                else
                {
                    Console.WriteLine("\nProduct not found!");
                    return -1;
                }
            } 
            catch(Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
                return -1;
            }            
        }
        private void GetProductByName()
        {
            Console.Write("Enter Product Name: ");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name cannot be empty or whitespace.");
                return;
            }

            try
            {
                PrintProducts(catalogManager.GetProductByName(name));               
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }
        private void GetProductsByCategory()
        {
            int categoryID = ReadInt("Enter Category ID: ");

            try
            {
                Category category = catalogManager.GetCategoryById(categoryID);

                if (category == null)
                {
                    Console.WriteLine("\nCategory not found!");
                    return;
                }            

                Console.WriteLine($"\nCategory ID: {category.CategoryId}  |  Category Name: {category.CategoryName}\n");
                PrintProducts(category.Products);
            }
            catch(Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }
        private void PrintProducts(List<Product> products)
        {
            if (products == null || products.Count == 0)
            {
                Console.WriteLine("No Product Found!\n");
                return;
            }

            Console.WriteLine("{0,-15} {1,-25} {2,-15} {3,-15} {4,-10}", "Product ID", "Product Name", "Unit Price", "Selling Price", "Quantity");

            Console.WriteLine(new string('-', 85));

            foreach (Product product in products)
            {
                Console.WriteLine("{0,-15} {1,-25} {2,-15} {3,-15} {4,-10}",
                    product.ProductID,
                    product.ProductName,
                    product.OriginalPrice,
                    product.SellingPrice,
                    product.Quantity);
            }
        }

        // =========================
        // CATEGORY + PRODUCT
        // =========================

        private void GetCategoriesWithProducts()
        {
            List<Category> categories = catalogManager.GetCategoriesWithProducts();

            foreach (Category category in categories)
            {
                Console.WriteLine($"\nCategory: {category.CategoryName} (ID: {category.CategoryId})");
                Console.WriteLine(new string('-', 50)); // make a line of 50 '-'

                if (category.Products.Count == 0)
                {
                    Console.WriteLine("  No products in this category.");
                }
                else
                {
                    Console.WriteLine("  {0,-25} {1,-15} {2,-10}", "Product Name", "Price", "Qty");
                    Console.WriteLine("  " + new string('-', 45)); // make a line of 45 '-'

                    foreach (var product in category.Products)
                    {
                        Console.WriteLine("  {0,-25} {1,-15} {2,-10}", product.ProductName, product.SellingPrice, product.Quantity);
                    }
                }

                Console.WriteLine();
            }
        }
        private void GetCategoriesWithLowStockProducts()
        {
            List<Category> categories = catalogManager.GetCategoriesWithLowStockProducts();

            Console.WriteLine("\nLow Stock Products: Quantity less than 10");
            foreach (Category category in categories)
            {
                Console.WriteLine($"\nCategory: {category.CategoryName} (ID: {category.CategoryId})");
                Console.WriteLine(new string('-', 50));

                if (category.Products == null || category.Products.Count == 0)
                {
                    Console.WriteLine("  No products in this category.");
                }
                else
                {
                    Console.WriteLine("  {0,-25} {1,-15} {2,-10}", "Product Name", "Price", "Qty");
                    Console.WriteLine("  " + new string('-', 45));

                    foreach (var product in category.Products)
                    {
                        Console.WriteLine("  {0,-25} {1,-15} {2,-10}", product.ProductName, product.SellingPrice, product.Quantity);
                    }
                }

                Console.WriteLine();
            }
        }


        // =========================
        // MENU
        // =========================

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n===== CATALOG MANAGEMENT =====");
                Console.WriteLine("1. Category Management");
                Console.WriteLine("2. Product Management");
                Console.WriteLine("3. View Categories With Products");
                Console.WriteLine("4. Low Stock Products");
                Console.WriteLine("0. Exit");
                Console.Write("Select Option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CategoryMenu();
                        break;
                    case "2":
                        ProductMenu();
                        break;
                    case "3":
                        GetCategoriesWithProducts();
                        Pause();
                        break;
                    case "4":
                        GetCategoriesWithLowStockProducts();
                        Pause();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice!");
                        Pause();
                        break;
                }
            }
        }
        private void CategoryMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== CATEGORY MENU =====");
                Console.WriteLine("1. Add Category");
                Console.WriteLine("2. Update Category");
                Console.WriteLine("3. Delete Category");
                Console.WriteLine("4. View All Categories");
                Console.WriteLine("5. View Category By ID");
                Console.WriteLine("6. View Category By Name");
                Console.WriteLine("0. Back");
                Console.Write("Select Option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCategoryUI();
                        Pause();
                        break;
                    case "2":
                        UpdateCategoryUI();
                        Pause();
                        break;
                    case "3":
                        DeleteCategoryUI();
                        Pause();
                        break;
                    case "4":
                        ViewCategoriesUI();
                        Pause();
                        break;
                    case "5":
                        GetCategoryByID();
                        Pause();
                        break;
                    case "6":
                        GetCategoryByName();
                        Pause();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice!");
                        Pause();
                        break;
                }                
            }
        }
        private void ProductMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== PRODUCT MENU =====");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. Update Product");
                Console.WriteLine("3. Delete Product");
                Console.WriteLine("4. View Product By ID");
                Console.WriteLine("5. View Product By Name");
                Console.WriteLine("6. View Products By Category");
                Console.WriteLine("0. Back");
                Console.Write("Select Option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddProductUI();
                        Pause();
                        break;
                    case "2":
                        UpdateProductUI();
                        Pause();
                        break;
                    case "3":
                        DeleteProductUI();
                        Pause();
                        break;
                    case "4":
                        GetProductByID();
                        Pause();
                        break;
                    case "5":
                        GetProductByName();
                        Pause();
                        break;
                    case "6":
                        GetProductsByCategory();
                        Pause();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice!");
                        Pause();
                        break;
                }
            }
        }

        private int ReadInt(string message)
        {
            Console.Write(message);

            int value;
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Invalid input! Enter a valid number: ");
            }
            return value;
        }

        private double ReadDouble(string message)
        {
            Console.Write(message);

            double value;
            while (!double.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Invalid input! Enter a number: ");
            }
            return value;
        }
        private void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}