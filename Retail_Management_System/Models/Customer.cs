using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    internal class Customer
    {
        private int customerId;
        private string customerName;
        private string email;
        private string city;
        private List<Invoice> invoices;

        public int CustomerId
        {
            get { return customerId; }
            set { customerId = value; }
        }
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public List<Invoice> Invoices
        {
            get { return invoices; }
            set { invoices = value; }
        }

        public Customer()
        {
            invoices = new List<Invoice>();
        }
        public Customer(int customerId, string name, string email, string city)
        {
            CustomerId = customerId;
            CustomerName = name;
            Email = email;
            City = city;
            invoices = new List<Invoice>();
        }
        public Customer(string name, string email, string city)
        {
            CustomerName = name;
            Email = email;
            City = city;
            invoices = new List<Invoice>();
        }
    }
}
