using RMS.DL;
using RMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RMS.BL
{
    internal class CatalogManager
    {
        // =========================
        // CATEGORY MANAGEMENT
        // =========================

        public void AddCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("Category cannot be null.");

            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Category name cannot be empty.");

            if (CategoryDL.CategoryExists(category.CategoryName))
                throw new InvalidOperationException("Category already exists.");

            category.CategoryId = CategoryDL.AddCategory(category);
        }
        public void UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException("Category cannot be null.");

            if (category.CategoryId <= 0)
                throw new ArgumentOutOfRangeException("Invalid Category ID.");

            if (!CategoryDL.CategoryExists(category.CategoryId))
                throw new InvalidOperationException("Category not found.");

            if (CategoryDL.CategoryNameExistsExceptThisId(category))
                throw new InvalidOperationException("Category name already exists.");

            CategoryDL.UpdateCategory(category);
        }
        public void DeleteCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentOutOfRangeException("Invalid Category ID.");

            if (!CategoryDL.CategoryExists(categoryId))
                throw new KeyNotFoundException("Category not found.");

            if (CategoryDL.IsCategoryHasProducts(categoryId))
                throw new InvalidOperationException("Cannot delete category with products.");

            CategoryDL.DeleteCategory(categoryId);
        }
        public Category GetCategoryById(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentOutOfRangeException("Invalid Category ID.");

            Category category = CategoryDL.GetCategoryById(categoryId);

            AttachProducts(category);

            return category;
        }
        public Category GetCategoryByName(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name cannot be empty.");

            Category category = CategoryDL.GetCategoryByName(categoryName);

            AttachProducts(category);

            return category;
        }
        public List<Category> GetAllCategories()
        {
            List<Category> categories = CategoryDL.GetAllCategories();

            AttachProducts(categories);

            return categories;
        }
        public int ProductsInCategory(int categoryid)
        {
            return CategoryDL.ProductsInCategory(categoryid);
        }
        // =========================
        // PRODUCT MANAGEMENT
        // =========================

        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("Product can not be null.");

            if (!CategoryDL.CategoryExists(product.CategoryId))
                throw new InvalidOperationException("Category does not exist.");

            if (ProductDL.IsProductExist(product.ProductName, product.CategoryId))
                throw new InvalidOperationException("Product already exists in this category.");

            if (product.SellingPrice < product.OriginalPrice)
                throw new Exception("Selling price must be >= original price.");

            product.ProductID = ProductDL.AddProduct(product);
        }
        public void UpdateProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("Product cannot be null.");

            if (product.ProductID <= 0)
                throw new ArgumentOutOfRangeException("Invalid Product ID.");

            if (!ProductDL.IsProductExist(product.ProductID))
                throw new InvalidOperationException("Product not found.");

            if(ProductDL.ProductExistsExceptThisId(product.ProductName,product.CategoryId,product.ProductID))    
                    throw new InvalidOperationException("Product Name already exists in this category.");

            ProductDL.UpdateProduct(product);
        }
        public void DeleteProduct(int productId)
        {
            if (productId <= 0)
                throw new ArgumentOutOfRangeException(nameof(productId), "Product ID must be greater than 0.");

            if (!ProductDL.IsProductExist(productId))
                throw new KeyNotFoundException("Product not found.");

            if (ProductDL.IsProductExistAsInvoiceItems(productId))
                throw new InvalidOperationException("Cannot delete product because it is used in invoice items.");

            ProductDL.DeleteProduct(productId);
        }
        public Product GetProductById(int id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException("Invalid Product ID.");

            return ProductDL.GetProductById(id);
        }
        public List<Product> GetProductByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.");

            return ProductDL.GetProductByName(name);
        }
        public List<Product> GetAllProducts()
        {
            return ProductDL.GetAllItems();
        }

        // =========================
        // CATEGORY + PRODUCT LINKING
        // =========================

        public List<Category> GetCategoriesWithProducts()
        {
            List<Category> categories = CategoryDL.GetAllCategories();

            AttachProducts(categories);

            return categories;
        }
        public List<Category> GetCategoriesWithLowStockProducts()
        {
            List<Category> categories = CategoryDL.GetAllCategories();

            AttachProductsForLowStock(categories);

            return categories;
        }

        private void AttachProducts(List<Category> categories)
        {
            List<Product> allProducts = ProductDL.GetAllItems();

            foreach (Category category in categories)
            {
                category.Products = allProducts.Where(p => p.CategoryId == category.CategoryId).ToList();
            }
        }
        private void AttachProducts(Category category)
        {
            if (category == null)
                return;

            List<Product> allProducts = ProductDL.GetAllItems();

            category.Products = allProducts.Where(p => p.CategoryId == category.CategoryId).ToList();
        }
        private void AttachProductsForLowStock(List<Category> categories)
        {
            List<Product> lowStock = ProductDL.LowStockProducts();

            foreach (Category category in categories)
            {
                category.Products = lowStock.Where(p => p.CategoryId == category.CategoryId).ToList();
            }
        }
    }
}