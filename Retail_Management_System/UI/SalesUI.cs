using RMS.BL;
using RMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RMS.UI
{
    internal class SalesUI
    {
        private CatalogManager catalogManager = new CatalogManager();
        private SalesManager salesManager = new SalesManager();
        private InvoiceManager invoiceManager = new InvoiceManager();

        // =========================
        // BUY PRODUCT UI
        // =========================
        private void BuyProductUI()
        {
            try
            {
                Console.WriteLine("\nAvailable Products:");
                List<Product> productsList = catalogManager.GetAllProducts().Where(p => p.Quantity > 0).ToList();

                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine($"{"ID",-5} | {"Name",-30} | {"Stock",-6} | {"Price",10}");
                Console.WriteLine("---------------------------------------------------------------");

                foreach (Product p in productsList)
                {
                    Console.WriteLine($"{p.ProductID,-5} | {p.ProductName,-30} | {p.Quantity,-6} | {p.SellingPrice,10}");
                }
                Console.WriteLine("---------------------------------------------------------------");

                Console.Write("\nEnter Customer ID: ");
                if (!int.TryParse(Console.ReadLine(), out int customerId))
                {
                    Console.WriteLine("Invalid Customer ID!");
                    return;
                }

                if (!CustomerBL.IsCustomerExists(customerId))
                {
                    Console.WriteLine("Error: Customer does not exist!");
                    return;
                }

                List<InvoiceItem> items = new List<InvoiceItem>();

                while (true)
                {
                    Console.Write("\nEnter Product ID (0 to finish): ");
                    if (!int.TryParse(Console.ReadLine(), out int productId))
                    {
                        Console.WriteLine("\nInvalid Product ID!");
                        continue;
                    }

                    if (productId < 0)
                    {
                        Console.WriteLine("Invalid product ID");
                        continue;
                    }

                    if (productId == 0)
                        break;

                    try
                    {
                        Product product = catalogManager.GetProductById(productId);
                        if (product == null)
                        {
                            Console.WriteLine("\nProduct not found!");
                            continue;
                        }

                        Console.Write("Enter Quantity: ");
                        if (!int.TryParse(Console.ReadLine(), out int qty))
                        {
                            Console.WriteLine("\nInvalid Quantity!");
                            continue;
                        }

                        if (qty <= 0)
                        {
                            Console.WriteLine("\nQuantity must be greater than 0!");
                            continue;
                        }

                        if (qty > product.Quantity)
                        {
                            Console.WriteLine($"\nOnly {product.Quantity} available!");
                            continue;
                        }

                        // Merge same product
                        InvoiceItem existing = items.Find(i => i.ProductID == productId);

                        if (existing != null)
                        {
                            existing.Quantity += qty;
                            existing.UnitPrice = product.SellingPrice;
                        }
                        else
                        {
                            items.Add(new InvoiceItem(productId, qty, product.SellingPrice));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error: " + e.Message);
                    }                   
                }

                if (items.Count == 0)
                {
                    Console.WriteLine("\nNo items added!");
                    return;
                }

                Invoice invoice = new Invoice(customerId, items);

                try
                {
                    salesManager.CreateInvoice(invoice);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }                

                PrintInvoice(invoice);

                Console.WriteLine("\nPurchase Successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }

        // =========================
        // RETURN PRODUCT UI
        // =========================
        private void ReturnProductUI()
        {
            Console.Write("\nEnter Invoice ID: ");
            if (!int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                Console.WriteLine("\nInvalid Invoice ID!");
                return;
            }

            try
            {               
                Invoice invoice = invoiceManager.GetInvoiceByID(invoiceId);
                List<InvoiceItem> items = invoice.Items;                

                if (items == null || items.Count == 0)
                {
                    Console.WriteLine("Invoice not found or empty!");
                    return;
                }

                Console.WriteLine("\n==================== INVOICE ITEMS ====================");
                Console.WriteLine($"{"ID",-6} {"Product",-25} {"Qty",-8} {"Price",-12}");
                Console.WriteLine("-------------------------------------------------------");

                foreach (InvoiceItem item in items)
                {
                    Product product = catalogManager.GetProductById(item.ProductID);

                    Console.WriteLine($"{product.ProductID,-6} {product.ProductName,-25} {item.Quantity,-8} {item.TotalPrice,-12}");
                }

                Console.WriteLine("-------------------------------------------------------");

                while (true)
                {
                    Console.Write("\nEnter Product ID to return (0 to finish): ");
                    if (!int.TryParse(Console.ReadLine(), out int productId))
                    {
                        Console.WriteLine("Invalid Product ID!");
                        continue;
                    }

                    if (productId == 0)
                        break;

                    Console.Write("Enter Quantity: ");
                    if (!int.TryParse(Console.ReadLine(), out int qty))
                    {
                        Console.WriteLine("Invalid Quantity!");
                        continue;
                    }

                    if (qty <= 0)
                    {
                        Console.WriteLine("\nQuantity must be greater than 0!");
                        continue;
                    }

                    try
                    {
                        salesManager.ReturnProduct(invoiceId, productId, qty);
                        Console.WriteLine("\nItem returned successfully!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nError: " + ex.Message);
                    }
                }
                Console.WriteLine("\nReturn process completed!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        private void ReturnFullInvoiceUI()
        {
            Console.Write("\nEnter Invoice ID: ");
            if (!int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                Console.WriteLine("\nInvalid Invoice ID!");
                return;
            }

            try
            {               
                Invoice invoice = invoiceManager.GetInvoiceByID(invoiceId);
                List<InvoiceItem> items = invoice.Items;

                if (items == null || items.Count == 0)
                {
                    Console.WriteLine("Invoice not found or has no items!");
                    return;
                }

                Console.WriteLine("\n==================== INVOICE ITEMS ====================");
                Console.WriteLine($"{"ID",-6} {"Product",-25} {"Qty",-8} {"Price",-12}");
                Console.WriteLine("-------------------------------------------------------");

                foreach (InvoiceItem item in items)
                {
                    Product product = catalogManager.GetProductById(item.ProductID);

                    Console.WriteLine($"{product.ProductID,-6} {product.ProductName,-25} {item.Quantity,-8} {item.TotalPrice,-12}");
                }

                Console.WriteLine("-------------------------------------------------------");

                Console.Write("\nAre you sure you want to return FULL invoice? (Y/N): ");
                string confirm = Console.ReadLine().ToLower();

                if (confirm != "y")
                {
                    Console.WriteLine("\nReturn cancelled.");
                    return;
                }

                salesManager.ReturnFullInvoice(invoiceId);

                Console.WriteLine("\nFull Invoice Returned Successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }

        // =========================
        // INVOICE UI
        // =========================
        private void PrintInvoice(Invoice invoice)
        {
            Console.WriteLine("\n=========================================================");
            Console.WriteLine("                 SALES INVOICE");
            Console.WriteLine("=========================================================");

            Console.WriteLine($"Invoice ID   : {invoice.InvoiceId}");
            Console.WriteLine($"Customer ID  : {invoice.CustomerId}");
            Console.WriteLine($"Date         : {invoice.InvoiceDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine("---------------------------------------------------------");

            Console.WriteLine("{0,-5}{1,-8}{2,-18}{3,-8}{4,-12}{5,-12}", "No", "ID", "Product", "Qty", "Price", "Total");
            Console.WriteLine("---------------------------------------------------------");

            int i = 1;
            foreach (var item in invoice.Items)
            {
                Product p = catalogManager.GetProductById(item.ProductID);
                string name = p != null ? p.ProductName : "Unknown";

                if (name.Length > 16)
                    name = name.Substring(0, 16);

                Console.WriteLine("{0,-5}{1,-8}{2,-18}{3,-8}{4,-12}{5,-12}",
                    i++,
                    item.ProductID,
                    name,
                    item.Quantity,
                    item.UnitPrice,
                    item.TotalPrice);
            }

            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine($"TOTAL AMOUNT: {invoice.TotalAmount}");
            Console.WriteLine("=========================================================\n");
        }

        // =========================
        // MENU
        // =========================
        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n========== SALES MENU ==========");
                Console.WriteLine("1. Buy Product");
                Console.WriteLine("2. Return Product");
                Console.WriteLine("3. Return Full Invoice");
                Console.WriteLine("0. Back");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        BuyProductUI();
                        Pause();
                        break;
                    case "2":
                        ReturnProductUI();
                        Pause();
                        break;
                    case "3":
                        ReturnFullInvoiceUI();
                        Pause();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice! Try again.");
                        Pause();
                        break;
                }
            }
        }
        private void Pause()
        {
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }
}