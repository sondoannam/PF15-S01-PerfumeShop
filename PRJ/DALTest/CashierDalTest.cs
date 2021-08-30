using System;
using Xunit;
using DAL;
using Persistence;

namespace DALTest
{
    public class CashierDalTest
    {
        private CashierDal dal = new CashierDal();
        [Fact]
        public void LoginTest1()
        {
            Cashier cashier = new Cashier(){UserName = "sondoannam", UserPass = "20052002Zz"};
            int expected = 1;
            int result = dal.Login(cashier);
            Assert.True(expected == result);
        }

        [Theory]
        [InlineData("sondoannam", "20052002Zz", 1)]
        public void LoginTest2(string userName, string password, int expected)
        {
            Cashier cashier = new Cashier(){UserName = userName, UserPass = password};
            int result = dal.Login(cashier);
            Assert.True(expected == result);
        }
    }
}
