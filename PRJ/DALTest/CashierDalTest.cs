using System;
using Xunit;
using DAL;
using Persistence;

namespace DALTest
{
    public class CashierDalTest
    {
        private CashierDal dal = new CashierDal();
        private Cashier cashier = new Cashier();

        [Fact]
        public void LoginTest1()
        {
            UserName = "sondoannam"; 
            UserPass = "20052002@Zz";
            int expected = 1;
            int result = dal.Login(cashier).cashier_ID;
            Assert.True(expected == result);
        }

        [Theory]
        [InlineData("sondoannam", "20052002@Zz", 1)]
        public void LoginTest2(string userName, string password, int expected)
        {
            UserName = userName; 
            UserPass = password;
            int result = dal.Login(cashier).cashier_ID;
            Assert.True(expected == result);
        }

        [Theory]
        [InlineData("Doan Nam Son","doannamson", "20052002#Zz", 0)]
        public void InsertTest1(string cashierName, string userName, string password, int expected)
        {
            cashier.cashier_name = cashierName;
            cashier.user_name = userName;
            cashier.user_pass = password;
            int result = dal.Insert(cashier);
            Assert.True(result<=expected);
        }
    }
}
