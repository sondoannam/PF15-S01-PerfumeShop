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

        //LoginTest
        [Theory]
        [InlineData("sondoannam", "20052002@Zz", 1)]
        [InlineData("lehuyhoang", "PF15@VTCAcademy", 2)]
        [InlineData("lamdinhkhoa", "PF15@VTCAcademy", 3)]
        public void LoginTest2(string userName, string password, int expected)
        {
            cashier.UserName = userName; 
            cashier.UserPass = password;
            Cashier result = dal.Login(cashier);
            Assert.True(expected.Equals(result.Cashier_ID));
        }

        [Theory]
        [InlineData("Doan Nam Son","doannamson", "20052002#Zz", 0)]
        [InlineData("Le Huy Hoang","hoanghuyle", "PF15@VTCAcademy", 0)]
        public void InsertTest1(string cashierName, string userName, string password, int expected)
        {
            cashier.Cashier_name = cashierName;
            cashier.UserName = userName;
            cashier.UserPass = password;
            int result = dal.Insert(cashier);
            Assert.True(result<=expected);
        }
    }
}
