using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    internal class Category
    {
        private int categoryId;
        private string categoryName;
        private List<Product> products;

        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }
        public List<Product> Products
        {
            get { return products; }
            set { products = value; }
        }

        public Category()
        {
            Products = new List<Product>();
        }
        public Category(int categoryId, string categoryName)
        {
            CategoryId = categoryId;
            CategoryName = categoryName;
            products = new List<Product>();
        }
        public Category(string categoryName)
        {
            CategoryName = categoryName;
            Products = new List<Product>();
        }
    }
}
