using System;
using Xunit;
using DAL;
using Persistence;
using System.Collections.Generic;

namespace DALTest
{
    public class InvoiceDalTest
    {
        private InvoiceDal invDal = new InvoiceDal();
        private Invoice invoice = new Invoice();

        private List<Invoice> invList = new List<Invoice>();

        //Get Invoice by ID test
        [Theory]
        [InlineData(1, "Nguyen Thanh Tung", 2)]
        [InlineData(2, "Thieu Bao Tram", 3)]
        [InlineData(3, "Nguyen Thanh Tung", 1)]
        [InlineData(4, "Ngo Ba Kha", 1)]
        [InlineData(5, "Taylor Swift", 1)]
        [InlineData(6, "Taylor Swift", 1)]
        [InlineData(7, "Taylor Swift", 1)]
        [InlineData(8, "Nguyen Thanh Tung", 2)]
        public void GetInvoiceByIDTest(int invID, string customerName, int expected)
        {
            invoice = invDal.GetInvoiceByID(invID);
            Assert.True(invoice != null);
            Assert.True(invoice.Invoice_ID == invID);
            Assert.True(invoice.InvoiceCustomer.CustomerName == customerName);
            Assert.True(invoice.ItemsList.Count == expected);
        }

        //Get List Invoice test
        [Theory]
        [InlineData(11)]
        public void GetInvoicesTest(int expected)
        {
            invList = invDal.GetInvoices();
            Assert.True(invList != null);
            Assert.True(invList.Count == expected);
        }

        //Change Invoice ID test
        [Theory]
        [InlineData(7, 2, true)]
        [InlineData(8, 2, true)]
        [InlineData(9, 2, true)]
        [InlineData(10, 2, true)]
        [InlineData(11, 2, true)]
        public void ChangeInvoiceStatusTest(int ivID, int ivStatus, bool expected)
        {
            invoice = invDal.GetInvoiceByID(ivID);
            Assert.True(invDal.ChangeInvoiceStatus(invoice, ivStatus) == expected);
        }
    }
}