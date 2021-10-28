using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class CustomerDal
    {
        private string query;
        private MySqlConnection connection = DbConfig.GetConnection();
        private MySqlDataReader reader;
        public CustomerDal() { }
        public Customer GetByPhone (string customerPhone)
        {
            Customer c = null;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = @"SELECT customer_ID, customer_name, customer_phone,
                            ifnull(customer_address, '') as customer_address
                            FROM Customers 
                            WHERE customer_phone=" + customerPhone + ";";
                    reader = (new MySqlCommand(query, connection)).ExecuteReader();
                    if (reader.Read())
                    {
                        c = GetCustomer(reader);
                    }
                    reader.Close();
                }
                catch
                {

                }
                finally
                {
                    connection.Close();
                }
            }
            return c;
        }
        public Customer GetByID (int customerID)
        {
            Customer c = null;
            lock (connection)
            {
                try
                {
                    connection.Open();
                    query = @"SELECT customer_ID, customer_name, customer_phone,
                            ifnull(customer_address, '') as customer_address
                            FROM Customers 
                            WHERE customer_ID = " + customerID + ";";
                    reader = (new MySqlCommand(query, connection)).ExecuteReader();
                    if (reader.Read())
                    {
                        c = GetCustomer(reader);
                    }
                    reader.Close();
                }
                catch
                {

                }
                finally
                {
                    connection.Close();
                }
            }
            return c;
        }

        internal Customer GetCustomer(MySqlDataReader reader)
        {
            Customer c = new Customer();
            c.Customer_ID = reader.GetInt32("customer_ID");
            c.CustomerName = reader.GetString("customer_name");
            c.CustomerPhone = reader.GetString("customer_phone");
            c.CustomerAddress = reader.GetString("customer_address");
            return c;
        }

        public int? AddCustomer(Customer c)
        {
            int? result = null;
            lock (connection)
            {    
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
                MySqlCommand cmd = new MySqlCommand("sp_createCustomer", connection);
                try
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerName", c.CustomerName);
                    cmd.Parameters["@customerName"].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("@customerPhone", c.CustomerPhone);
                    cmd.Parameters["@customerPhone"].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("@customerAddress", c.CustomerAddress);
                    cmd.Parameters["@customerAddress"].Direction = System.Data.ParameterDirection.Input;
                    cmd.Parameters.AddWithValue("@customerId", MySqlDbType.Int32);
                    cmd.Parameters["@customerId"].Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@customerId", MySqlDbType.Int32);
                    cmd.Parameters["@customerId"].Direction = System.Data.ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    result = (int)cmd.Parameters["@customerId"].Value;
                }
                catch
                {

                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }
    }
}