using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    internal class Invoice
    {
        private int invoiceId;
        private int customerId;
        private DateTime invoiceDate;
        private double totalAmount;
        private List<InvoiceItem> items = new List<InvoiceItem>();

        public int InvoiceId
        {
            get { return invoiceId; }
            set { invoiceId = value; }
        }
        public int CustomerId
        {
            get { return customerId; }
            set
            {
                if(value <= 0)
                {
                    throw new ArgumentOutOfRangeException("Invalid Customer ID.");
                }
                customerId = value;
            }
        }
        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set
            {
                if (value > DateTime.Now)
                {
                    throw new ArgumentException("Invoice date cannot be in the future.");
                }

                invoiceDate = value; 
            }
        }
        public double TotalAmount
        {
          get { return totalAmount; }
          set { totalAmount = value; }
        }
        public List<InvoiceItem> Items
        {
            get { return items; }
            set {  items = value; }
        }

        public Invoice()
        {
            items = new List<InvoiceItem>();
        }
        public Invoice(int customerId, List<InvoiceItem> items)
        {
            CustomerId = customerId;
            InvoiceDate = DateTime.Now;
            this.items = items;
        }
    }
}
