using System;
using Persistence;
using DAL;

namespace BL
{
    public class CashierBL
    {
        private CashierDal dal = new CashierDal();
        public Cashier Login(Cashier cashier)
        {
            return dal.Login(cashier);
        }
    }
}
