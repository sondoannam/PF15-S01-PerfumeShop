using System;

namespace Persistence
{
    public class Customer
    {
        public int? Customer_ID { set; get;}
        public string CustomerName { set; get;}
        public string CustomerPhone { set; get;}
        public string CustomerAddress{ set; get;}

        public override bool Equals(object obj)
        {
            if(obj is Customer)
            {
                return ((Customer)obj).Customer_ID.Equals(Customer_ID);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Customer_ID.GetHashCode();
        }
    }
}