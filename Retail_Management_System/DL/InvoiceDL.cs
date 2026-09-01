using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using RMS.Models;

namespace RMS.DL
{
    internal class InvoiceDL
    {
        public static int AddInvoice(Invoice invoice)
        {
            string query = "INSERT INTO Invoices (CustomerId, InvoiceDate, TotalAmount) VALUES (@CustomerId, @InvoiceDate, @TotalAmount); SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CustomerId", invoice.CustomerId),
                new MySqlParameter("@InvoiceDate", invoice.InvoiceDate),
                new MySqlParameter("@TotalAmount", invoice.TotalAmount)
            };
            return DatabaseHelper.Instance.ExecuteScalar(query, parameters);
        }

        public static void UpdateInvoiceTotal(int invoiceId, double totalAmount)
        {
            string query = "UPDATE Invoices SET TotalAmount = @TotalAmount WHERE InvoiceId = @InvoiceId;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@TotalAmount", totalAmount),
                new MySqlParameter("@InvoiceId", invoiceId)
            };
            DatabaseHelper.Instance.Update(query, parameters);
        }

        public static void DeleteInvoice(int id)
        {
            string query = "DELETE FROM Invoices WHERE InvoiceID = @InvoiceID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@InvoiceID", id)
            };
            DatabaseHelper.Instance.Update(query, parameters);
        }

        public static Invoice GetInvoiceById(int invoiceId)
        {
            string query = "SELECT InvoiceID, CustomerId, InvoiceDate, TotalAmount FROM Invoices WHERE InvoiceId = @InvoiceId;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@InvoiceId", invoiceId)
            };
            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return MapRowToInvoice(dt.Rows[0]);
        }

        public static List<Invoice> GetAllInvoices()
        {
            string query = "SELECT InvoiceID, CustomerId, InvoiceDate, TotalAmount FROM Invoices;";
            DataTable dt = DatabaseHelper.Instance.GetDataTable(query);

            List<Invoice> invoices = new List<Invoice>();
            foreach (DataRow row in dt.Rows)
            {
                invoices.Add(MapRowToInvoice(row));
            }

            return invoices;
        }

        public static List<Invoice> GetInvoicesByCustomerId(int customerId)
        {
            string query = "SELECT InvoiceID, CustomerId, InvoiceDate, TotalAmount FROM Invoices WHERE CustomerId = @CustomerId;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CustomerId", customerId)
            };
            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            List<Invoice> invoices = new List<Invoice>();
            foreach (DataRow row in dt.Rows)
            {
                invoices.Add(MapRowToInvoice(row));
            }

            return invoices;
        }

        public static List<Invoice> GetInvoicesByDateRange(DateTime startDate, DateTime endDate)
        {
            string query = "SELECT InvoiceID, CustomerId, InvoiceDate, TotalAmount FROM Invoices WHERE InvoiceDate >= @StartDate AND InvoiceDate < @EndDate;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@StartDate", startDate),
                new MySqlParameter("@EndDate", endDate)
            };
            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            List<Invoice> invoices = new List<Invoice>();
            foreach (DataRow row in dt.Rows)
            {
                invoices.Add(MapRowToInvoice(row));
            }

            return invoices;
        }

        private static Invoice MapRowToInvoice(DataRow row)
        {
            return new Invoice
            {
                InvoiceId = Convert.ToInt32(row["InvoiceID"]),
                CustomerId = Convert.ToInt32(row["CustomerId"]),
                InvoiceDate = Convert.ToDateTime(row["InvoiceDate"]),
                TotalAmount = Convert.ToDouble(row["TotalAmount"])
            };
        }
    }
}