using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using RMS.Models;

namespace RMS.DL
{
    internal class CustomerDL
    {
        public static int AddCustomer(Customer customer)
        {
            string query = "INSERT INTO Customers (CustomerName, Email, City) VALUES (@CustomerName, @Email, @City); SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CustomerName", customer.CustomerName),
                new MySqlParameter("@Email", customer.Email),
                new MySqlParameter("@City", customer.City)
            };

            return DatabaseHelper.Instance.ExecuteScalar(query, parameters);
        }

        public static void UpdateCustomer(Customer customer)
        {
            string query = "UPDATE Customers SET CustomerName = @CustomerName, Email = @Email, City = @City WHERE CustomerId = @CustomerId;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CustomerName", customer.CustomerName),
                new MySqlParameter("@Email", customer.Email),
                new MySqlParameter("@City", customer.City),
                new MySqlParameter("@CustomerId", customer.CustomerId)
            };

            DatabaseHelper.Instance.Update(query, parameters);
        }

        public static void DeleteCustomer(int Id)
        {
            string query = "DELETE FROM Customers WHERE CustomerId = @CustomerId;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CustomerId", Id)
            };

            DatabaseHelper.Instance.Update(query, parameters);
        }

        public static List<Customer> GetAllCustomers()
        {
            string query = "SELECT CustomerId, CustomerName, Email, City FROM Customers;";
            DataTable dt = DatabaseHelper.Instance.GetDataTable(query);

            List<Customer> customers = new List<Customer>();
            foreach (DataRow row in dt.Rows)
            {
                customers.Add(MapRowToCustomer(row));
            }
            return customers;
        }

        public static Customer GetCustomerById(int customerId)
        {
            string query = "SELECT CustomerId, CustomerName, Email, City FROM Customers WHERE CustomerId = @CustomerId;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CustomerId", customerId)
            };

            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return MapRowToCustomer(dt.Rows[0]);
        }

        public static List<Customer> GetCustomerByName(string customerName)
        {
            string query = "SELECT CustomerId, CustomerName, Email, City FROM Customers WHERE CustomerName LIKE @CustomerName;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CustomerName", $"%{customerName}%")
            };

            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            List<Customer> customers = new List<Customer>();
            foreach (DataRow row in dt.Rows)
            {
                customers.Add(MapRowToCustomer(row));
            }
            return customers;
        }

        public static bool IsCustomerExists(int customerId)
        {
            string query = "SELECT COUNT(*) FROM Customers WHERE CustomerId = @CustomerId;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CustomerId", customerId)
            };

            int count = DatabaseHelper.Instance.ExecuteScalar(query, parameters);
            return count > 0;
        }

        public static bool IsCustomerExists(string email)
        {
            string query = "SELECT COUNT(*) FROM Customers WHERE Email = @Email;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@Email", email)
            };

            int count = DatabaseHelper.Instance.ExecuteScalar(query, parameters);
            return count > 0;
        }

        public static bool IsCustomerHasInvoice(int customerId)
        {
            string query = "SELECT COUNT(*) FROM Invoices WHERE CustomerID = @CustomerId;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CustomerId", customerId)
            };

            return DatabaseHelper.Instance.ExecuteScalar(query, parameters) > 0;
        }

        private static Customer MapRowToCustomer(DataRow row)
        {
            return new Customer
            {
                CustomerId = Convert.ToInt32(row["CustomerId"]),
                CustomerName = row["CustomerName"].ToString(),
                Email = row["Email"].ToString(),
                City = row["City"].ToString()
            };
        }
    }
}