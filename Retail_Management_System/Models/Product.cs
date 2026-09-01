using System;
using System.Collections.Generic;

namespace RMS.Models
{
    internal class Product
    {
        private int productID;
        private string productName;
        private int categoryId;
        private double originalPrice;
        private double sellingPrice;
        private int quantity;

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }
        public string ProductName
        {
            get { return productName; }
            set 
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new Exception("Item name cannot be empty.");
                }
                    
                productName = value.ToLower();
            }
        }
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }
        public double OriginalPrice
        {
            get { return originalPrice; }
            set
            {
                if (value > 0)
                    originalPrice = value;
                else
                    throw new Exception("Invalid Orignal Price! Must be greater than 0.");
            }
        }
        public double SellingPrice
        {
            get { return sellingPrice; }
            set
            {
                if (value > 0)
                    sellingPrice = value;
                else
                    throw new Exception("Invalid Selling Price!");
            }
        }
        public int Quantity
        {
            get { return quantity; }
            set {
                if (value < 0)
                    throw new Exception("Quantity cannot be negative");
                else
                    quantity = value;
            }
        }

        public Product()
        {
            
        }
        public Product(int productID, string productName, int categoryID, double originalPrice, double itemPrice, int quantity)
        {
            ProductID = productID;
            ProductName = productName;
            CategoryId = categoryID;
            OriginalPrice = originalPrice;
            SellingPrice = itemPrice;
            Quantity = quantity;
        }
        public Product(string productName, int categoryID, double originalPrice, double itemPrice, int quantity)
        {
            ProductName = productName;
            CategoryId = categoryID;
            OriginalPrice = originalPrice;
            SellingPrice= itemPrice;
            Quantity= quantity;
        }
    }
}