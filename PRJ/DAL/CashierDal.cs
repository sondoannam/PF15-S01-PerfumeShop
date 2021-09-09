using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class CashierDal
    {
        private MySqlConnection connection = DbHelper.GetConnection();
        public Cashier Login(Cashier cashier)
        {
            lock (connection)
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();
                    command.CommandText = "select * from Cashiers where user_name=@userName and user_pass=@userPass;";
                    command.Parameters.AddWithValue("@userName", cashier.UserName);
                    command.Parameters.AddWithValue("@userPass", Md5Algorithms.CreateMD5(cashier.UserPass));
                    MySqlDataReader reader = command.ExecuteReader();
                    if(reader.Read())
                    {
                        cashier.Cashier_ID = reader.GetInt32("cashier_ID");
                    }
                    else
                    {
                        cashier.Cashier_ID = 0;
                    }
                    reader.Close();
                }
                catch
                {
                    cashier.Cashier_ID = -1;
                } 
                finally
                {
                    connection.Close();
                }
            }
            return cashier;
        }

        public int Insert(Cashier cashier)
        {
            int? result = null;
            MySqlConnection connection = DbHelper.GetConnection();
            string sql = @"insert into Cashiers(cashier_name, user_name, user_pass) values 
                        (@cashierName, @userName, @userPass);";
            lock (connection)
            {
                try
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@cashierName", cashier.Cashier_name);
                    command.Parameters.AddWithValue("@userName", cashier.UserName);
                    command.Parameters.AddWithValue("@userPass", cashier.UserPass);
                    result = command.ExecuteNonQuery();
                }
                catch
                {
                    result = -2;
                }
                finally
                {
                    connection.Close();
                }
            }
            return result ?? 0;
        }

        public List<Cashier> GetAll()
        {
            return null;
        }
    }
}