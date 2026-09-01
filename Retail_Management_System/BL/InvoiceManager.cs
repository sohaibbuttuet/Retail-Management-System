using RMS.DL;
using RMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.BL
{
    internal class InvoiceManager
    {
        public int AddInvoice(Invoice invoice)
        {
            if (invoice == null)
                throw new ArgumentNullException(nameof(invoice));

            return InvoiceDL.AddInvoice(invoice);
        }
        public void UpdateInvoice(int invoiceid, double TotalAmount)
        {
            InvoiceDL.UpdateInvoiceTotal(invoiceid, TotalAmount);
        }
        public void DeleteInvoice(int invoiceid)
        {
             InvoiceDL.DeleteInvoice(invoiceid);
        }
        public List<Invoice> GetAllInvoicesWithItems()
        {
            List<Invoice> invoices = InvoiceDL.GetAllInvoices();
            AttachItems(invoices);

            return invoices;
        }
        public List<Invoice> GetInvoicesByCustomerID(int customerId)
        {
            if (customerId <= 0)
                throw new ArgumentOutOfRangeException(nameof(customerId));

            List<Invoice> invoices = InvoiceDL.GetInvoicesByCustomerId(customerId);
            AttachItems(invoices);

            return invoices;
        }
        public List<Invoice> GetInvoicesByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
                throw new ArgumentException("Start date cannot be greater than end date.");

            List<Invoice> invoices = InvoiceDL.GetInvoicesByDateRange(startDate,endDate);
            AttachItems(invoices);

            return invoices;
        }
        public Invoice GetInvoiceByID(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentOutOfRangeException("Invalid Invoice ID.");
            }

            Invoice invoice = InvoiceDL.GetInvoiceById(id);
            AttachItems(invoice);

            return invoice;
        }

        public int AddInvoiceItem(InvoiceItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (item.InvoiceID <= 0 || item.ProductID <= 0)
                throw new ArgumentOutOfRangeException("Invalid Invoice Item Data.");

            return InvoiceItemDL.AddInvoiceItem(item);
        }
        public void UpdateInvoiceItem(InvoiceItem invoiceItem)
        {
            if (invoiceItem == null)
                throw new ArgumentNullException(nameof(invoiceItem));

            if (!InvoiceItemDL.InvoiceItemExists(invoiceItem.InvoiceItemID))
                throw new InvalidOperationException("Invoice item does not exist.");

            InvoiceItemDL.UpdateInvoiceItem(invoiceItem);
        }
        public void DeleteInvoiceItem(int invoiceItemID)
        {
            if (invoiceItemID <= 0)
                throw new ArgumentOutOfRangeException("Invalid Invoice Item ID.");

            if (!InvoiceItemDL.InvoiceItemExists(invoiceItemID))
                throw new InvalidOperationException("Invoice item does not exist.");

            InvoiceItemDL.DeleteInvoiceItemByID(invoiceItemID);
        }
        public InvoiceItem GetInvoiceItem(int invoiceId, int productId)
        {
            if (invoiceId <= 0 || productId <= 0)
                throw new ArgumentOutOfRangeException("Invalid Input");

            return InvoiceItemDL.GetInvoiceItem(invoiceId, productId);
        }

        private void AttachItems(List<Invoice> invoices)
        {
            if (invoices == null) return;

            foreach (Invoice invoice in invoices)
            {
                AttachItems(invoice);
            }
        }

        private void AttachItems(Invoice invoice)
        {
            if (invoice == null)
                return;

            invoice.Items = InvoiceItemDL.GetInvoiceItemsByInvoiceID(invoice.InvoiceId) ?? new List<InvoiceItem>();
        }
    }
}
