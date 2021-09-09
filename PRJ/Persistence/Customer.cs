using System;

namespace Persistence
{
    public class Customer
    {
        public int? Custmer_ID { set; get;}
        public string CustomerName { set; get;}
        public string CustomerAddress{ set; get;}

        public override bool Equals(object obj)
        {
            if(obj is Customer)
            {
                return ((Customer)obj).Custmer_ID.Equals(Custmer_ID);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Custmer_ID.GetHashCode();
        }
    }
}