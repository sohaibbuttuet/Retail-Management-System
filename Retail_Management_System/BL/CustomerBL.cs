    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RMS.DL;
    using RMS.Models;

    namespace RMS.BL
    {        
    internal class CustomerBL
    {
        public void AddCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("Customer cannot be null.");

            if (string.IsNullOrWhiteSpace(customer.CustomerName))
                throw new ArgumentException("Name required.");

            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new ArgumentException("Email required.");

            if (CustomerDL.IsCustomerExists(customer.Email))
                throw new InvalidOperationException("Email already exists.");

            customer.CustomerId = CustomerDL.AddCustomer(customer);
        }
        public void UpdateCustomer(Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("Customer cannot be null.");

            if (customer.CustomerId <= 0)
                throw new ArgumentOutOfRangeException("Invalid Customer ID.");

            if (!CustomerDL.IsCustomerExists(customer.CustomerId))
                throw new InvalidOperationException("Customer not found.");

            CustomerDL.UpdateCustomer(customer);
        }
        public void DeleteCustomer(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentOutOfRangeException("Invalid Customer ID.");

            if (!CustomerDL.IsCustomerExists(customerId))
                throw new InvalidOperationException("Customer not found.");

            if (CustomerDL.IsCustomerHasInvoice(customerId))
                throw new InvalidOperationException("Customer cannot be deleted because it has invoices.");

            CustomerDL.DeleteCustomer(customerId);
        }
        public Customer GetCustomerByID(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentOutOfRangeException("Invalid Customer ID.");

            Customer customer = CustomerDL.GetCustomerById(customerId);

            AttachInvoices(customer);

            return customer;
        }
        public List<Customer> GetCustomerByName(string customerName)
        {
            List<Customer> customers = CustomerDL.GetCustomerByName(customerName);

            foreach (Customer c in customers)
            {
                AttachInvoices(c);
            }

            return customers;
        }
        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = CustomerDL.GetAllCustomers();

            foreach (Customer c in customers)
            {
                AttachInvoices(c);
            }

            return customers;
        }

        // For SalesUI
        public static bool IsCustomerExists(int customerId)
        {
            return CustomerDL.IsCustomerExists(customerId);
        }

        // LINKING FUNCTION
        private void AttachInvoices(Customer customer)
        {
            if (customer == null) return;

            customer.Invoices = InvoiceDL.GetInvoicesByCustomerId(customer.CustomerId) ?? new List<Invoice>();

            foreach (Invoice invoice in customer.Invoices)
            {
                invoice.Items = InvoiceItemDL.GetInvoiceItemsByInvoiceID(invoice.InvoiceId) ?? new List<InvoiceItem>();
            } 
        }
    }
}
