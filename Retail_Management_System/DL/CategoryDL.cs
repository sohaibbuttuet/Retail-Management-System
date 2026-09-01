using MySql.Data.MySqlClient; // Required for MySqlParameter
using System;
using System.Collections.Generic;
using System.Data;
using RMS.Models;

namespace RMS.DL
{
    internal class CategoryDL
    {
        public static int AddCategory(Category category)
        {
            string query = "INSERT INTO Categories (CategoryName) VALUES (@CategoryName); SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CategoryName", category.CategoryName)
            };

            return DatabaseHelper.Instance.ExecuteScalar(query, parameters);
        }

        public static void UpdateCategory(Category category)
        {
            string query = "UPDATE Categories SET CategoryName = @CategoryName WHERE CategoryID = @CategoryID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CategoryName", category.CategoryName),
                new MySqlParameter("@CategoryID", category.CategoryId)
            };

            DatabaseHelper.Instance.Update(query, parameters);
        }

        public static void DeleteCategory(int categoryId)
        {
            string query = "DELETE FROM Categories WHERE CategoryID = @CategoryID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CategoryID", categoryId)
            };

            DatabaseHelper.Instance.Update(query, parameters);
        }

        public static List<Category> GetAllCategories()
        {
            string query = "SELECT CategoryID, CategoryName FROM Categories;";
            DataTable dt = DatabaseHelper.Instance.GetDataTable(query);

            List<Category> categories = new List<Category>();

            foreach (DataRow row in dt.Rows)
            {
                categories.Add(MapRow(row)); // Fixed misleading method name
            }

            return categories;
        }

        public static Category GetCategoryById(int categoryId)
        {
            string query = "SELECT CategoryId, CategoryName FROM Categories WHERE CategoryId = @CategoryID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CategoryID", categoryId)
            };

            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return MapRow(dt.Rows[0]);
        }

        public static Category GetCategoryByName(string categoryName)
        {
            string query = "SELECT CategoryId, CategoryName FROM Categories WHERE CategoryName = @CategoryName;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CategoryName", categoryName)
            };

            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return MapRow(dt.Rows[0]);
        }

        public static bool CategoryExists(string categoryName)
        {
            string query = "SELECT COUNT(*) FROM Categories WHERE CategoryName = @CategoryName;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CategoryName", categoryName)
            };

            int count = DatabaseHelper.Instance.ExecuteScalar(query, parameters);
            return count > 0;
        }

        public static bool CategoryExists(int categoryId)
        {
            string query = "SELECT COUNT(*) FROM Categories WHERE CategoryID = @CategoryID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CategoryID", categoryId)
            };

            int count = DatabaseHelper.Instance.ExecuteScalar(query, parameters);
            return count > 0;
        }

        public static bool CategoryNameExistsExceptThisId(Category c)
        {
            string query = "SELECT COUNT(*) FROM Categories WHERE CategoryID <> @CategoryID AND CategoryName = @CategoryName;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CategoryID", c.CategoryId),
                new MySqlParameter("@CategoryName", c.CategoryName)
            };

            int count = DatabaseHelper.Instance.ExecuteScalar(query, parameters);
            return count > 0;
        }

        public static int ProductsInCategory(int categoryID)
        {
            string query = "SELECT COUNT(*) FROM Products WHERE CategoryID = @CategoryID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CategoryID", categoryID)
            };

            return DatabaseHelper.Instance.ExecuteScalar(query, parameters);
        }

        public static bool IsCategoryHasProducts(int categoryId)
        {
            string query = "SELECT COUNT(*) FROM Products WHERE CategoryID = @CategoryID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CategoryID", categoryId)
            };

            return DatabaseHelper.Instance.ExecuteScalar(query, parameters) > 0;
        }

        private static Category MapRow(DataRow row)
        {
            return new Category
            {
                CategoryId = Convert.ToInt32(row["CategoryID"]),
                CategoryName = row["CategoryName"].ToString()
            };
        }
    }
}