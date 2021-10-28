using System;
using MySql.Data.MySqlClient;

namespace DAL
{
    public class DbConfig
    {
        private static MySqlConnection connection = new MySqlConnection();
        private DbConfig(){}
        public static MySqlConnection GetDefaultConnection()
        {
            connection.ConnectionString = "server=localhost;user id=pf15;password=vtcacademy;port=3306;database=LoginDB;";
            return connection;
        }

        public static MySqlConnection GetConnection()
        {
            try{
                string conString;
                using (System.IO.FileStream fileStream = System.IO.File.OpenRead("DbConfig.txt"))
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                    {
                        conString = reader.ReadLine();
                        reader.Close();
                    }
                    fileStream.Close();
                }
                return GetConnection(conString);
            }catch
            {
                return null;
            }
        }

        public static MySqlConnection GetConnection(string connectionString)
        {
            connection.ConnectionString = connectionString;
            return connection;
        }
    }
}
