using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using RMS.Models;

namespace RMS.DL
{
    internal class InvoiceItemDL
    {
        public static int AddInvoiceItem(InvoiceItem item)
        {
            string query = "INSERT INTO InvoiceItems (InvoiceID, ProductID, Quantity, UnitPrice) VALUES (@InvoiceID, @ProductID, @Quantity, @UnitPrice); SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = {
                new MySqlParameter("@InvoiceID", item.InvoiceID),
                new MySqlParameter("@ProductID", item.ProductID),
                new MySqlParameter("@Quantity", item.Quantity),
                new MySqlParameter("@UnitPrice", item.UnitPrice)
            };

            return DatabaseHelper.Instance.ExecuteScalar(query, parameters);
        }

        public static void UpdateInvoiceItem(InvoiceItem item)
        {
            string query = "UPDATE InvoiceItems SET Quantity = @Quantity, UnitPrice = @UnitPrice WHERE InvoiceItemID = @InvoiceItemID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@Quantity", item.Quantity),
                new MySqlParameter("@UnitPrice", item.UnitPrice),
                new MySqlParameter("@InvoiceItemID", item.InvoiceItemID)
            };
            DatabaseHelper.Instance.Update(query, parameters);
        }

        public static void DeleteInvoiceItemByID(int invoiceItemID)
        {
            string query = "DELETE FROM InvoiceItems WHERE InvoiceItemID = @InvoiceItemID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@InvoiceItemID", invoiceItemID)
            };
            DatabaseHelper.Instance.Update(query, parameters);
        }

        public static List<InvoiceItem> GetInvoiceItemsByInvoiceID(int invoiceID)
        {
            string query = "SELECT InvoiceItemID, InvoiceID, ProductID, Quantity, UnitPrice FROM InvoiceItems WHERE InvoiceID = @InvoiceID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@InvoiceID", invoiceID)
            };
            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            List<InvoiceItem> invoiceItems = new List<InvoiceItem>();
            foreach (DataRow row in dt.Rows)
            {
                invoiceItems.Add(MapRowToInvoiceItem(row));
            }
            return invoiceItems;
        }

        public static InvoiceItem GetInvoiceItem(int invoiceId, int productId)
        {
            string query = "SELECT InvoiceItemID, InvoiceID, ProductID, Quantity, UnitPrice FROM InvoiceItems WHERE InvoiceID = @InvoiceID AND ProductID = @ProductID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@InvoiceID", invoiceId),
                new MySqlParameter("@ProductID", productId)
            };

            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return MapRowToInvoiceItem(dt.Rows[0]);
        }

        public static bool InvoiceItemExists(int invoiceItemID)
        {
            string query = "SELECT COUNT(*) FROM InvoiceItems WHERE InvoiceItemID = @InvoiceItemID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@InvoiceItemID", invoiceItemID)
            };

            int count = DatabaseHelper.Instance.ExecuteScalar(query, parameters);
            return count > 0;
        }

        private static InvoiceItem MapRowToInvoiceItem(DataRow row)
        {
            return new InvoiceItem
            {
                InvoiceItemID = Convert.ToInt32(row["InvoiceItemID"]),
                InvoiceID = Convert.ToInt32(row["InvoiceID"]),
                ProductID = Convert.ToInt32(row["ProductID"]),
                Quantity = Convert.ToInt32(row["Quantity"]),
                UnitPrice = Convert.ToDouble(row["UnitPrice"]),
            };
        }
    }
}