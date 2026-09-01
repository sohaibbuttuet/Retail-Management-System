using System;
using System.Collections.Generic;
using RMS.BL;
using RMS.Models;

namespace RMS.UI
{
    internal class InvoiceUI
    {
        private InvoiceManager invoiceManager = new InvoiceManager();

        // =========================
        // MAIN MENU
        // =========================
        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n========== INVOICE REPORTS ==========");
                Console.WriteLine("1. View All Invoices");
                Console.WriteLine("2. View Invoices By Customer ID");
                Console.WriteLine("3. View Invoices By Date Range");
                Console.WriteLine("4. View Invoice Items By Invoice ID");
                Console.WriteLine("0. Back");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewAllInvoices();
                        Pause();
                        break;
                    case "2":
                        ViewByCustomer();
                        Pause();
                        break;
                    case "3":
                        ViewByDateRange();
                        Pause();
                        break;
                    case "4":
                        ViewInvoiceItems();
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

        private void ViewAllInvoices()
        {
            List<Invoice> invoices = invoiceManager.GetAllInvoicesWithItems();

            PrintInvoices(invoices);
        }
        private void ViewByCustomer()
        {
            Console.Write("Enter Customer ID: ");

            if (!int.TryParse(Console.ReadLine(), out int customerId))
            {
                Console.WriteLine("Invalid Customer ID!");
                return;
            }

            List<Invoice> invoices = invoiceManager.GetInvoicesByCustomerID(customerId);

            PrintInvoices(invoices);
        }
        private void ViewByDateRange()
        {
            Console.Write("Enter Start Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime start))
            {
                Console.WriteLine("Invalid Start Date!");
                return;
            }

            Console.Write("Enter End Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime end))
            {
                Console.WriteLine("Invalid End Date!");
                return;
            }

            List<Invoice> invoices = invoiceManager.GetInvoicesByDateRange(start, end);

            PrintInvoices(invoices);
        }
        private void ViewInvoiceItems()
        {
            Console.Write("Enter Invoice ID: ");

            if (!int.TryParse(Console.ReadLine(), out int invoiceId))
            {
                Console.WriteLine("Invalid Invoice ID!");
                return;
            }
           
            Invoice invoice = invoiceManager.GetInvoiceByID(invoiceId);

            if (invoice == null || invoice.Items.Count == 0)
            {
                Console.WriteLine("No invoices found!");
                return;
            }

            PrintInvoice(invoice);
        }

        // =========================
        // COMMON PRINT METHOD
        // =========================
        private void PrintInvoices(List<Invoice> invoices)
        {
            if (invoices == null || invoices.Count == 0)
            {
                Console.WriteLine("No invoices found!");
                return;
            }

            Console.WriteLine("\n{0,-5} {1,-15} {2,-20} {3,-10}", "ID", "Customer", "Date", "Total");
            Console.WriteLine(new string('-', 55));

            double grandTotal = 0;

            foreach (var inv in invoices)
            {
                Console.WriteLine("{0,-5} {1,-15} {2,-20} {3,-10}",
                    inv.InvoiceId,
                    inv.CustomerId,
                    inv.InvoiceDate.ToString("yyyy-MM-dd HH:mm"),
                    inv.TotalAmount);

                grandTotal += inv.TotalAmount;
            }

            Console.WriteLine(new string('-', 55));
            Console.WriteLine("{0,-42} {1,-10}", "Grand Total:", grandTotal);
        }
        private void PrintInvoice(Invoice invoice)
        {
            Console.WriteLine("\n========================================================");
            Console.WriteLine("                      INVOICE");
            Console.WriteLine("========================================================");

            Console.WriteLine($"Invoice ID   : {invoice.InvoiceId}");
            Console.WriteLine($"Customer ID  : {invoice.CustomerId}");
            Console.WriteLine($"Date         : {invoice.InvoiceDate:yyyy-MM-dd HH:mm}");
            Console.WriteLine("--------------------------------------------------------");

            Console.WriteLine("{0,-5}{1,-8}{2,-18}{3,-8}{4,-12}{5,-12}", "No", "ID", "Product", "Qty", "Price", "Total");
            Console.WriteLine("--------------------------------------------------------");

            CatalogManager catalogManager = new CatalogManager();
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

            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine($"TOTAL AMOUNT: {invoice.TotalAmount}");
            Console.WriteLine("========================================================\n");
        }

        // =========================
        // PAUSE
        // =========================
        private void Pause()
        {
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }
}