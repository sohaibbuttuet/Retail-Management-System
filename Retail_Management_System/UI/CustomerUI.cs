using RMS.BL;
using RMS.DL;
using RMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.UI
{
    internal class CustomerUI
    {
        CustomerBL customerBL = new CustomerBL();
        public void AddCustomerUI()
        {
            Console.Write("\nEnter Customer Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Customer Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Customer City: ");
            string city = Console.ReadLine();

            Customer customer = new Customer(name, email, city);

            try
            {
                customerBL.AddCustomer(customer);
                Console.WriteLine("Customer Added Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public void UpdateCustomerUI()
        {
            int id = GetCustomerByID();

            if (id == -1)
                return;

            Console.Write("\nEnter New Customer Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter New Customer Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter New Customer City: ");
            string city = Console.ReadLine();

            Customer customer = new Customer(id, name, email, city);

            try
            {
                customerBL.UpdateCustomer(customer);
                Console.WriteLine("\nCustomer Updated Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public void DeleteCustomerUI()
        {
            int id = GetCustomerByID();

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
                customerBL.DeleteCustomer(id);
                Console.WriteLine("\nCustomer Deleted Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }
        public int GetCustomerByID()
        {
            Console.Write("\nEnter Customer ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nInvalid ID format!");
                return -1;
            }

            try
            {
                Customer customer = customerBL.GetCustomerByID(id);

                if (customer != null)
                {
                    Console.WriteLine($"\nCustomer ID: {customer.CustomerId}");
                    Console.WriteLine($"Customer Name: {customer.CustomerName}");
                    Console.WriteLine($"Customer Email: {customer.Email}");
                    Console.WriteLine($"Customer City: {customer.City}\n");
                    return id;
                }
                else
                {
                    Console.WriteLine("\nCustomer not found.");
                    return -1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError: " + e.Message);
                return -1;
            }
        }
        public void GetCustomerByName()
        {
            Console.Write("\nEnter Customer Name to View: ");
            string name = Console.ReadLine();

            try
            {
                PrintCustomer(customerBL.GetCustomerByName(name));
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError: " + e.Message);
            }
        }
        public void ViewAllCustomers()
        {
            try
            {
                PrintCustomer(customerBL.GetAllCustomers());
            }
            catch (Exception e)
            {
                Console.WriteLine("\nError: " + e.Message);
            }
        }
        private void PrintCustomer(List<Customer> customers)
        {
            if (customers == null || customers.Count == 0)
            {
                Console.WriteLine("\nNo Customer Exists!");
                return;
            }

            Console.WriteLine();
            Console.WriteLine($"{"ID",-5} {"Name",-20} {"Email",-25} {"City",-15}");
            Console.WriteLine(new string('-', 70));

            foreach (Customer customer in customers)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-25} {3,-15}",
                    customer.CustomerId,
                    customer.CustomerName,
                    customer.Email,
                    customer.City);
            }
        }

        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n===== CUSTOMER MANAGEMENT =====");
                Console.WriteLine("1. Add Customer");
                Console.WriteLine("2. Update Customer");
                Console.WriteLine("3. Delete Customer");
                Console.WriteLine("4. View Customer by ID");
                Console.WriteLine("5. View Customer by Name");
                Console.WriteLine("6. View All Customers");
                Console.WriteLine("0. Back");
                Console.Write("Select Option: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCustomerUI();
                        Pause();
                        break;
                    case "2":
                        UpdateCustomerUI();
                        Pause();
                        break;
                    case "3":
                        DeleteCustomerUI();
                        Pause();
                        break;
                    case "4":
                        GetCustomerByID();
                        Pause();
                        break;
                    case "5":
                        GetCustomerByName();
                        Pause();
                        break;
                    case "6":
                        ViewAllCustomers();
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
        private void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
