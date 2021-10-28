using System;
using System.Collections.Generic;

namespace Persistence
{
    public static class InvoiceStatus
    {
        public const int CREATE_NEW_INVOICE = 1;
        public const int INVOICE_COMPLETED = 2;
        public const int INVOICE_CANCELED = 3;
    }

    public class Invoice
    {
        public int Invoice_ID { set; get; }
        public DateTime InvoiceDate { set; get; }
        public int? Status { set; get; }
        public Customer InvoiceCustomer { set; get; }
        public List<Perfume> ItemsList { set; get; }

        public Perfume this[int index]
        {
            get
            {
                if (ItemsList == null || ItemsList.Count == 0 || index < 0 || ItemsList.Count < index) return null;
                return ItemsList[index];
            }
            set
            {
                if (ItemsList == null) ItemsList = new List<Perfume>();
                ItemsList.Add(value);
            }
        }

        public Invoice()
        {
            ItemsList = new List<Perfume>();
        }

        public override bool Equals(object obj)
        {
            if (obj is Invoice) return ((Invoice)obj).Invoice_ID.Equals(Invoice_ID);
            return false;
        }

        public override int GetHashCode()
        {
            return Invoice_ID.GetHashCode();
        }
    }
}