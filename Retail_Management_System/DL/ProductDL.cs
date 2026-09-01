using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using RMS.Models;

namespace RMS.DL
{
    internal class ProductDL
    {
        public static int AddProduct(Product product)
        {
            string query = "INSERT INTO products (ProductName, CategoryID, OriginalPrice, SellingPrice, StockQuantity) VALUES (@ProductName, @CategoryID, @OriginalPrice, @SellingPrice, @StockQuantity); SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = {
                new MySqlParameter("@ProductName", product.ProductName),
                new MySqlParameter("@CategoryID", product.CategoryId),
                new MySqlParameter("@OriginalPrice", product.OriginalPrice),
                new MySqlParameter("@SellingPrice", product.SellingPrice),
                new MySqlParameter("@StockQuantity", product.Quantity)
            };

            return DatabaseHelper.Instance.ExecuteScalar(query, parameters);
        }

        public static void UpdateProduct(Product product)
        {
            string query = "UPDATE products SET ProductName = @ProductName, CategoryID = @CategoryID, OriginalPrice = @OriginalPrice, SellingPrice = @SellingPrice, StockQuantity = @StockQuantity WHERE ProductID = @ProductID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@ProductName", product.ProductName),
                new MySqlParameter("@CategoryID", product.CategoryId),
                new MySqlParameter("@OriginalPrice", product.OriginalPrice),
                new MySqlParameter("@SellingPrice", product.SellingPrice),
                new MySqlParameter("@StockQuantity", product.Quantity),
                new MySqlParameter("@ProductID", product.ProductID)
            };

            DatabaseHelper.Instance.Update(query, parameters);
        }

        public static void DeleteProduct(int productId)
        {
            string query = "DELETE FROM products WHERE ProductID = @ProductID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@ProductID", productId)
            };
            DatabaseHelper.Instance.Update(query, parameters);
        }

        public static Product GetProductById(int productId)
        {
            string query = "SELECT ProductID, ProductName, CategoryID, OriginalPrice, SellingPrice, StockQuantity FROM Products WHERE ProductID = @ProductID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@ProductID", productId)
            };
            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return MapRowToProduct(dt.Rows[0]);
        }

        public static List<Product> GetProductByName(string productName)
        {
            string query = "SELECT ProductID, ProductName, CategoryID, OriginalPrice, SellingPrice, StockQuantity FROM Products WHERE ProductName LIKE @ProductName;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@ProductName", $"%{productName}%")
            };
            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            List<Product> list = new List<Product>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRowToProduct(row));
            }

            return list;
        }

        public static List<Product> LowStockProducts()
        {
            string query = "SELECT ProductID, ProductName, CategoryID, OriginalPrice, SellingPrice, StockQuantity FROM Products WHERE StockQuantity < 10;";
            DataTable dt = DatabaseHelper.Instance.GetDataTable(query);

            List<Product> list = new List<Product>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRowToProduct(row));
            }

            return list;
        }

        public static List<Product> GetAllItems()
        {
            string query = "SELECT ProductID, ProductName, CategoryID, OriginalPrice, SellingPrice, StockQuantity FROM Products;";
            DataTable dt = DatabaseHelper.Instance.GetDataTable(query);

            List<Product> list = new List<Product>();
            foreach (DataRow row in dt.Rows)
            {
                list.Add(MapRowToProduct(row));
            }
            return list;
        }

        public static bool IsProductExist(string productName, int categoryId)
        {
            string query = "SELECT COUNT(*) FROM Products WHERE ProductName = @ProductName AND CategoryID = @CategoryID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@ProductName", productName),
                new MySqlParameter("@CategoryID", categoryId)
            };

            return DatabaseHelper.Instance.ExecuteScalar(query, parameters) > 0;
        }

        public static bool ProductExistsExceptThisId(string productName, int categoryId, int productId)
        {
            string query = "SELECT COUNT(*) FROM Products WHERE ProductName = @ProductName AND CategoryID = @CategoryID AND ProductID <> @ProductID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@ProductName", productName),
                new MySqlParameter("@CategoryID", categoryId),
                new MySqlParameter("@ProductID", productId)
            };

            return DatabaseHelper.Instance.ExecuteScalar(query, parameters) > 0;
        }

        public static bool IsProductExist(int id)
        {
            string query = "SELECT COUNT(*) FROM Products WHERE ProductID = @ProductID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@ProductID", id)
            };

            return DatabaseHelper.Instance.ExecuteScalar(query, parameters) > 0;
        }

        public static bool IsProductExistAsInvoiceItems(int productId)
        {
            // Fixed table name typo from 'InvoiceItem' to 'invoiceitems' to match your database schema
            string query = "SELECT COUNT(*) FROM invoiceitems WHERE ProductID = @ProductID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@ProductID", productId)
            };

            return DatabaseHelper.Instance.ExecuteScalar(query, parameters) > 0;
        }

        private static Product MapRowToProduct(DataRow row)
        {
            return new Product
            {
                ProductID = Convert.ToInt32(row["ProductID"]),
                ProductName = row["ProductName"].ToString(),
                CategoryId = Convert.ToInt32(row["CategoryID"]),
                OriginalPrice = Convert.ToDouble(row["OriginalPrice"]),
                SellingPrice = Convert.ToDouble(row["SellingPrice"]),
                Quantity = Convert.ToInt32(row["StockQuantity"])
            };
        }
    }
}