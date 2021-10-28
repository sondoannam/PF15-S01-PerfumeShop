using System;
using System.Collections.Generic;
using Persistence;
using DAL;

namespace BL
{
    public class InvoiceBL
    {
        private InvoiceDal ivDal = new InvoiceDal();
        public Invoice GetInvoiceByID(int invoiceID)
        {
            return ivDal.GetInvoiceByID(invoiceID);
        }
        public List<Invoice> GetInvoices()
        {
            return ivDal.GetInvoices();
        }
        public bool CreateInvoice(Invoice invoice)
        {
            bool result = ivDal.CreateInvoice(invoice);
            return result;
        }
        public bool ChangeInvoiceStatus(Invoice invoice, int invoiceStatus)
        {
            bool result = ivDal.ChangeInvoiceStatus(invoice, invoiceStatus);
            return result;
        }
    }
}