using Mysqlx.Crud;
using RMS.DL;
using RMS.Models;
using System;
using System.Collections.Generic;

namespace RMS.BL
{
    internal class SalesManager
    {
        private CatalogManager catalogManager = new CatalogManager();
        private InvoiceManager invoiceManager = new InvoiceManager();

        // =========================
        // BUY PRODUCT
        // =========================
        public void CreateInvoice(Invoice invoice)
        {
            if (invoice == null)
                throw new InvalidOperationException("Invoice cannot be null.");

            if (invoice.Items == null || invoice.Items.Count == 0)
                throw new Exception("Invoice must contain at least one item.");

            // 1. VALIDATE ITEM DATA FIRST
            foreach (InvoiceItem item in invoice.Items)
            {
                if (item.Quantity <= 0)
                    throw new Exception("Invalid quantity detected.");

                if (item.ProductID <= 0)
                    throw new Exception("Invalid product ID.");
            }

            // 2. VALIDATE STOCK
            foreach (InvoiceItem item in invoice.Items)
            {
                Product product = catalogManager.GetProductById(item.ProductID);

                if (product == null)
                    throw new InvalidOperationException("Product not found: " + item.ProductID);

                if (product.Quantity < item.Quantity)
                    throw new Exception($"Not enough stock for {product.ProductName}");
            }

            try
            {
                // 2. CREATE INVOICE
                invoice.InvoiceId = invoiceManager.AddInvoice(invoice);

                double total = 0;

                // 3. PROCESS ITEMS
                foreach (InvoiceItem item in invoice.Items)
                {
                    Product product = catalogManager.GetProductById(item.ProductID);

                    item.InvoiceID = invoice.InvoiceId;

                    // update stock
                    product.Quantity -= item.Quantity;
                    catalogManager.UpdateProduct(product);

                    // save item
                    item.InvoiceItemID = invoiceManager.AddInvoiceItem(item);

                    // calculate total
                    total += item.UnitPrice * item.Quantity;
                }

                // 4. UPDATE TOTAL IN DB
                invoice.TotalAmount = total;
                invoiceManager.UpdateInvoice(invoice.InvoiceId, total);
            }
            catch (Exception e)
            {
                throw new Exception("Error: " + e.Message);
            }
        }

        // =========================
        // RETURN PRODUCT
        // =========================
        public void ReturnProduct(int invoiceId, int productId, int returnQty) 
        {
            if (invoiceId <= 0 || productId <= 0 || returnQty <= 0)
                throw new Exception("Invalid return data.");

            InvoiceItem item = invoiceManager.GetInvoiceItem(invoiceId, productId);

            if (item == null)
                throw new Exception("Invoice item not found.");

            if (returnQty > item.Quantity)
                throw new Exception("Return quantity exceeds available quantity.");

            Product product = catalogManager.GetProductById(productId);

            if (product == null)
                throw new Exception("Product not found.");

            try
            {
                // 1. RESTORE STOCK
                product.Quantity += returnQty;
                catalogManager.UpdateProduct(product);

                // 2. UPDATE ITEM
                item.Quantity -= returnQty;

                if (item.Quantity == 0)
                    invoiceManager.DeleteInvoiceItem(item.InvoiceItemID);
                else
                    invoiceManager.UpdateInvoiceItem(item);

                // 3. RECALCULATE TOTAL
                Invoice invoice = invoiceManager.GetInvoiceByID(invoiceId);

                if (invoice.Items == null || invoice.Items.Count == 0)
                {
                    invoiceManager.DeleteInvoice(invoiceId);
                    return;
                }

                double newTotal = 0;

                foreach (InvoiceItem i in invoice.Items)
                {
                    Product p = catalogManager.GetProductById(i.ProductID);
                    newTotal += p.SellingPrice * i.Quantity;
                }

                invoiceManager.UpdateInvoice(invoiceId, newTotal);
            }
            catch (Exception ex)
            {
                throw new Exception("Return failed: " + ex.Message);
            }
        }
        public void ReturnFullInvoice(int invoiceId)
        {
            try
            {
                Invoice invoice = invoiceManager.GetInvoiceByID(invoiceId);

                if (invoice == null || invoice.Items == null)
                    throw new Exception("Invoice not found.");

                foreach (InvoiceItem item in invoice.Items)
                {
                    Product product = catalogManager.GetProductById(item.ProductID);

                    if (product == null)
                        throw new Exception("Product not found.");

                    // restore stock
                    product.Quantity += item.Quantity;
                    catalogManager.UpdateProduct(product);

                    // delete item
                    invoiceManager.DeleteInvoiceItem(item.InvoiceItemID);
                }

                // delete invoice
                invoiceManager.DeleteInvoice(invoiceId);
            }
            catch (Exception ex)
            {
                throw new Exception("Full invoice return failed: " + ex.Message);
            }
        }
    }
}