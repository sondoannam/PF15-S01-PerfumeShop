using System;
using System.Collections.Generic;
using Persistence;
using DAL;

namespace BL
{
    public class CustomerBL
    {
        private CustomerDal cusdal = new CustomerDal();
        public Customer GetByPhone(string customerPhone)
        {
            return cusdal.GetByPhone(customerPhone);
        }
        public Customer GetByID(int customerID)
        {
            return cusdal.GetByID(customerID);
        }

        public int AddCustomer(Customer customer)
        {
            return cusdal.AddCustomer(customer) ?? 0;
        }
    }
}