using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    internal class InvoiceItem
    {
        private int invoiceItemID;
        private int invoiceID;
        private int productID;
        private int quantity;
        private double unitPrice;

        public int InvoiceItemID
        {
            get { return invoiceItemID; }
            set { invoiceItemID = value; }
        }
        public int InvoiceID
        {
            get { return invoiceID; }
            set 
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Invalid Invoice ID.");
                }
                invoiceID = value; 
            }
        }
        public int ProductID
        {
            get { return productID; }
            set
            { 
                if(value <= 0)
                {
                    throw new ArgumentException("Invalid Product ID.");
                }
                productID = value;
            }
        }
        public int Quantity
        {
            get { return quantity; }
            set
            { 
                if(value <= 0)
                {
                    throw new ArgumentException("Quantity must be positive.");
                }
                quantity = value; 
            }
        }
        public double UnitPrice
        {
            get { return unitPrice; }
            set
            {
                if(value <= 0)
                {
                    throw new ArgumentException("Unit price must be positive.");
                }
                unitPrice = value; 
            }
        }
        public double TotalPrice
        {
            get { return Quantity * UnitPrice; }
        }

        public InvoiceItem()
        {

        }
        public InvoiceItem(int productID, int quantity, double unitPrice)
        {
            ProductID = productID;
            Quantity= quantity;
            UnitPrice= unitPrice;
        }
    }
}
