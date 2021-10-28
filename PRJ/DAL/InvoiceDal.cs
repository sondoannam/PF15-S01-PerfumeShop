using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Persistence;

namespace DAL
{
    public class InvoiceDal
    {
        private string query;
        private MySqlConnection connection = DbConfig.GetConnection();
        public Invoice GetInvoiceByID(int invID)
        {
            Invoice invoice = null;
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(" ", connection);
                MySqlDataReader reader = null;
                query = @"select invoice_ID, invoice_date, invoice_status, customer_name, customer_phone
                            from Invoices 
                            inner join Customers
                            on Invoices.customer_ID = Customers.customer_ID and invoice_ID = @invoiceID;";
                command.CommandText = query;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@invoiceID", invID);
                reader = command.ExecuteReader();

                if(reader.Read())
                {
                    invoice = new Invoice() { Invoice_ID = invID, InvoiceDate = reader.GetDateTime("invoice_date"), Status = reader.GetInt16("invoice_status"),
                                            InvoiceCustomer = new Customer() {CustomerName = reader.GetString("customer_name"), CustomerPhone = reader.GetString("customer_phone")}};
                }
                else
                {
                    throw new Exception("Not exist item!");
                }
                reader.Close();

                query = @"select invoice_ID,  InvoiceLineItems.perfume_ID, perfume_name, unit_price, quantity
                                    from InvoiceLineItems inner join Perfumes
                                    on InvoiceLineItems.perfume_ID = Perfumes.perfume_ID and invoice_ID = @invoiceID;";
                command.CommandText = query;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@invoiceID", invID);
                reader = command.ExecuteReader();
                while(reader.Read())
                {
                    invoice.ItemsList.Add(new Perfume(){Perfume_ID = reader.GetInt32("perfume_ID"), PerfumeName = reader.GetString("perfume_name"), PerfumePrice = reader.GetDecimal("unit_price"), TotalQuantity = reader.GetInt16("quantity")});
                }
                reader.Close();
                
            }
            catch(Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
            finally
            { 
                connection.Close(); 
            }
            return invoice;
        }
        public List<Invoice> GetInvoices()
        {
            List<Invoice> invList = new List<Invoice>();
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(" ", connection);
                MySqlDataReader reader = null;
                query = @"select invoice_ID, invoice_date, invoice_status, customer_name, customer_phone
                            from Invoices 
                            inner join Customers
                            on Invoices.customer_ID = Customers.customer_ID;";
                command.CommandText = query;
                reader = command.ExecuteReader();
                while(reader.Read())
                {
                    invList.Add(new Invoice(){ Invoice_ID = reader.GetInt32("invoice_ID"), InvoiceDate = reader.GetDateTime("invoice_date"), Status = reader.GetInt16("invoice_status"),
                                            InvoiceCustomer = new Customer(){ CustomerName = reader.GetString("customer_name"), CustomerPhone = reader.GetString("customer_phone")}});
                }
                reader.Close();
                
                foreach(Invoice inv in invList)
                {
                    query = @"select invoice_ID, perfume_ID, unit_price, quantity
                                from InvoiceLineItems
                                where invoice_ID = @invoiceID;";
                    command.CommandText = query;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@invoiceID", inv.Invoice_ID);
                    reader = command.ExecuteReader();
                    while(reader.Read())
                    {
                        inv.ItemsList.Add(new Perfume(){Perfume_ID = reader.GetInt32("perfume_ID"), PerfumePrice = reader.GetDecimal("unit_price"), TotalQuantity = reader.GetInt16("quantity")});
                    }
                    reader.Close();
                }
            }
            catch(Exception ex){ Console.WriteLine(ex.Message); }
            finally{ connection.Close(); }
            return invList;
        }

        public bool ChangeInvoiceStatus(Invoice invoice, int status)
        {
            if (invoice == null || invoice.ItemsList == null || invoice.ItemsList.Count == 0)
            {
                return false;
            }
            bool result = false;

            try
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.Connection = connection;
                //Lock update table Invoices
                cmd.CommandText = "lock tables Invoices write;";
                cmd.ExecuteNonQuery();
                MySqlTransaction trans = connection.BeginTransaction();
                cmd.Transaction = trans;
                
                try
                {
                    //update status in Invoices
                    cmd.CommandText = "update Invoices set invoice_status = @invoiceStatus where invoice_ID = " + invoice.Invoice_ID + ";";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@invoiceStatus", status);
                    cmd.ExecuteNonQuery();

                    //commit transaction
                    trans.Commit();
                    result = true;
                }
                catch
                {
                    try
                    {
                        trans.Rollback();
                    }
                    catch(Exception ex) { Console.WriteLine(ex.Message); }
                }
                finally
                {
                    //unlock all tables;
                    cmd.CommandText = "unlock tables;";
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex){ Console.WriteLine(ex.Message);}
            finally{ connection.Close(); }
            return result;
        }
        public bool CreateInvoice(Invoice invoice)
        {
            if (invoice == null || invoice.ItemsList == null || invoice.ItemsList.Count == 0)
            {
                return false;
            }
            bool result = false;

            try
            {
                connection.Open();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.Connection = connection;

                //Lock update all tables
                cmd.CommandText = "lock tables Customers write, Invoices write, Perfumes write, InvoiceLineItems write;";
                cmd.ExecuteNonQuery();
                MySqlTransaction trans = connection.BeginTransaction();
                cmd.Transaction = trans;
                MySqlDataReader reader = null;
                if (invoice.InvoiceCustomer == null || invoice.InvoiceCustomer.CustomerName == null || invoice.InvoiceCustomer.CustomerName == "")
                {
                //set default customer with customer id = 1
                invoice.InvoiceCustomer = new Customer() { Customer_ID = 1 };
                }
                try
                {
                    if (invoice.InvoiceCustomer.Customer_ID == null)
                    {
                        //Insert new Customer
                        cmd.CommandText = @"insert into Customers(customer_name, customer_phone, customer_address)
                                        values ('" + invoice.InvoiceCustomer.CustomerName + "','" + invoice.InvoiceCustomer.CustomerPhone + "','" + (invoice.InvoiceCustomer.CustomerAddress ?? "") + "');";
                        cmd.ExecuteNonQuery();
                        //Get new customer id
                        cmd.CommandText = "select customer_ID from Customers order by customer_ID desc limit 1;";
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                        invoice.InvoiceCustomer.Customer_ID = reader.GetInt32("customer_ID");
                        }
                        reader.Close();
                    }
                    else
                    {
                        //get Customer by ID
                        cmd.CommandText = "select * from Customers where customer_ID=@customerID;";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@customerID", invoice.InvoiceCustomer.Customer_ID);
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                        invoice.InvoiceCustomer = new CustomerDal().GetCustomer(reader);
                        }
                        reader.Close();
                    }
                    if (invoice.InvoiceCustomer == null || invoice.InvoiceCustomer.Customer_ID == null)
                    {
                        throw new Exception("Can't find Customer!");
                    }

                    //Insert Order
                    cmd.CommandText = "insert into Invoices(customer_ID, invoice_status) values (@customerID, @invoiceStatus);";
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@customerID", invoice.InvoiceCustomer.Customer_ID);
                    cmd.Parameters.AddWithValue("@invoiceStatus", InvoiceStatus.CREATE_NEW_INVOICE);
                    cmd.ExecuteNonQuery();
                    //get new Order_ID
                    cmd.CommandText = "select LAST_INSERT_ID() as invoice_ID";
                    reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        invoice.Invoice_ID = reader.GetInt32("invoice_ID");
                    }
                    reader.Close();

                    //insert Order Details table
                    foreach (var item in invoice.ItemsList)
                    {
                        if (item.Perfume_ID == null || item.TotalQuantity <= 0)
                        {
                        throw new Exception("Not Exists Item");
                        }
                        //get unit_price
                        cmd.CommandText = "select perfume_name, price from Perfumes where perfume_ID=@perfumeID";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@perfumeID", item.Perfume_ID);
                        reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            item.PerfumeName = reader.GetString("perfume_name");
                            item.PerfumePrice = reader.GetDecimal("price");
                        }
                        else
                        {
                            throw new Exception("Not Exists Item");
                        }
                        reader.Close();

                        //insert to Order Details
                        cmd.CommandText = @"insert into InvoiceLineItems(invoice_ID, perfume_ID, unit_price, quantity) values 
                                        (" + invoice.Invoice_ID + ", " + item.Perfume_ID + ", " + item.PerfumePrice + ", " + item.TotalQuantity + ");";
                        cmd.ExecuteNonQuery();

                        //update amount in Items
                        cmd.CommandText = "update Perfumes set total_quantity = total_quantity - @quantity where perfume_ID = " + item.Perfume_ID + ";";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@quantity", item.TotalQuantity);
                        cmd.ExecuteNonQuery();
                    }
                    //commit transaction
                    trans.Commit();
                    result = true;
                
                }
                catch
                {
                    try
                    {
                        trans.Rollback();
                    }
                    catch(Exception ex) { Console.WriteLine(ex.Message); }
                }
                finally
                {
                    //unlock all tables;
                    cmd.CommandText = "unlock tables;";
                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return result;
            
        }


    }
}