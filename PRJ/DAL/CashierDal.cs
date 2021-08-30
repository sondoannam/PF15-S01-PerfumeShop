using System;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class CashierDal
    {
        public int Login(Cashier cashier)
        {
            int login = 0;
            Console.WriteLine(cashier.UserName + " - " + cashier.UserPass);
            try
            {
                MySqlConnection connection = DbHelper.GetConnection();
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "select * from Cashiers where user_name = '"
                    + cashier.UserName +"' and user_pass = '"
                    + Md5Algorithms.CreateMD5(cashier.UserPass) +"';";
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read())
                {
                    login = reader.GetInt32("cashier_ID");
                }
                else
                {
                    login = 0;
                }
                reader.Close();
                connection.Close();
            }
            catch
            {
                login = -1;
            }
            Console.WriteLine(login);
            return login;
        }
    }
}