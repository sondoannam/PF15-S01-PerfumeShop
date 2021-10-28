using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Persistence;
using BL;

namespace ConsoleAppPL
{
    class Program
    {
        static void Main(string[] args)
        {
            bool valid = false;
            string userName;
            string pass;
            do
            {
                Console.Clear();
                Console.WriteLine($" {"PF15 - PROJECT 01 - PERFUME SHOP",30}\n");
                Console.Write(DrawTitle("Login"));
                Console.Write(" # User Name: ");
                userName = Console.ReadLine();
                Console.Write(" # Password: ");
                pass = GetPassword();
                Console.WriteLine();
                int validUsername = 0, validPass1 = 0, validPass2 = 0;
                //valid username & password here
                if (userName.Length < 8)
                {
                    Console.WriteLine("Username can not be less than 8 characters!");
                    Console.ReadLine();
                }
                else validUsername = 1;

                if (pass.Length < 8)
                {
                    Console.WriteLine("Password must have at least 8 characters!");
                    Console.ReadLine();
                }
                else validPass1 = 1;

                var regexItem = new Regex("^[a-zA-Z0-9\x20]+$");
                if (regexItem.IsMatch(pass) == true)
                {
                    Console.WriteLine("Password must have at least 1 Lower & Upper case, 1 number and 1 special character");
                    Console.ReadLine();
                }
                else validPass2 = 1;

                if (validUsername == 1 && validPass1 == 1 && validPass2 == 1) valid = true;
                else valid = false;
            } while (valid == false);
            Cashier cashier = new Cashier(){UserName = userName, UserPass = pass};
            CashierBL bl = new CashierBL();
            cashier = bl.Login(cashier);
            if(cashier.Cashier_ID <= 0)
            {
                Console.WriteLine(" Can't login! Try again...");
                Console.ReadKey();
                Environment.Exit(0);
            }
            
            short mainChoose = 0, imChoose;
            string[] mainMenu = {"Search Perfume", "Create Invoice", "Exit"};
            string[] imMenu = {"Search By ID", "Search By Name", "Search By Gender", "Search By Brand", "Product List", "Back"};
            ItemBL iBL = new ItemBL();
            CustomerBL cusBL = new CustomerBL();
            InvoiceBL ivBL = new InvoiceBL();
            List<Perfume> list;
            List<Invoice> invList = new List<Invoice>();
            do
            {
                Console.Clear();
                Console.WriteLine(ProjectTitle());
                mainChoose = Menu("Cashier's System", mainMenu);
                switch (mainChoose)
                {
                    case 1:
                        do
                        {
                            Console.Clear();
                            Console.WriteLine(ProjectTitle());
                            imChoose = Menu("Search Perfume", imMenu);
                            switch (imChoose)
                            {
                                case 1: //Search by ID
                                    Console.Clear();
                                    Console.WriteLine(ProjectTitle());
                                    Console.Write(DrawTitle(" Get Perfume By ID"));
                                    int itemID;
                                    Console.Write(" Input Perfume ID: ");
                                    if (Int32.TryParse(Console.ReadLine(), out itemID))
                                    {
                                        Perfume p = iBL.GetItemByID(itemID);
                                        if (p != null)
                                        {
                                            ViewPerfumeInfor(p, itemID);
                                        }
                                        else
                                        {
                                            Console.WriteLine(" There is no item with ID " + itemID);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(" ID input must be number!");
                                    }
                                    Console.WriteLine("\n    Press any key to back...");
                                    Console.ReadLine();
                                    break; 
                                case 2: //Search By Name
                                    Console.Clear();
                                    Console.WriteLine(ProjectTitle());
                                    Console.Write(DrawTitle("Search By Name"));
                                    Console.Write(" Input Name: ");
                                    string nameSearch = Console.ReadLine();
                                    if (!nameSearch.Any(char.IsDigit))
                                    {
                                        list = iBL.GetByName(nameSearch);
                                        if (list.Count == 1)
                                        {
                                            Console.Clear();
                                            Console.WriteLine(ProjectTitle());
                                            Console.WriteLine("\n" + @" -- THERE IS 1 RESULT FOR ""{0}"" --", nameSearch);
                                        }
                                        else 
                                        {
                                            Console.Clear();
                                            Console.WriteLine(ProjectTitle());
                                            Console.WriteLine("\n" + @" -- THERE IS {0} RESULT FOR ""{1}"" --", list.Count, nameSearch);
                                        }
                                        ViewList(list);
                                    }
                                    else
                                    {
                                        Console.WriteLine(" Invalid ID input!");
                                    }
                                    break;
                                case 3: //Search By Gender
                                    string[] gender = {"Men", "Women", "Unisex", "Back"};
                                    short genderSelect=0;
                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine(ProjectTitle());
                                        genderSelect = Menu("Search By Gender", gender);
                                        switch (genderSelect)
                                        {
                                            case 1: 
                                                Console.Clear();
                                                Console.WriteLine(ProjectTitle());
                                                list = iBL.GetByGender("Men");
                                                Console.WriteLine(" -- Found {0} results for Men --", list.Count);
                                                ViewList(list);
                                                break;
                                            case 2: 
                                                Console.Clear();
                                                Console.WriteLine(ProjectTitle());
                                                list = iBL.GetByGender("Women");
                                                Console.WriteLine(" -- Found {0} results for Women --", list.Count);
                                                ViewList(list);
                                                break;
                                            case 3: 
                                                Console.Clear();
                                                Console.WriteLine(ProjectTitle());
                                                list = iBL.GetByGender("Unisex");
                                                Console.WriteLine(" -- Found {0} results for Unisex --", list.Count);
                                                ViewList(list);
                                                break;
                                        }
                                        
                                    } while (genderSelect != gender.Length);
                                    break;
                                case 4: //Search By Brands
                                    string[] brands = {"Versace", "Dolce & Gabbana", "Calvin Klein", "Mont Blanc", "Christian Dior", "Jimmy Choo", "Back"};
                                    short brandSelect = 0;
                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine(ProjectTitle());
                                        brandSelect = Menu("Search By Brands", brands);
                                        switch (brandSelect)
                                        {
                                            case 1:
                                                Console.Clear();
                                                Console.WriteLine(ProjectTitle());
                                                list = iBL.GetByBrand("Versace");
                                                Console.WriteLine(" -- Found {0} results from Versace --", list.Count);
                                                ViewList(list);
                                                break;
                                            case 2:
                                                Console.Clear();
                                                Console.WriteLine(ProjectTitle());
                                                list = iBL.GetByBrand("Dolce & Gabbana");
                                                Console.WriteLine(" -- Found {0} results from Dolce & Gabbana --", list.Count);
                                                ViewList(list);
                                                break;
                                            case 3:
                                                Console.Clear();
                                                Console.WriteLine(ProjectTitle());
                                                list = iBL.GetByBrand("Calvin Klein");
                                                Console.WriteLine(" -- Found {0} results from Calvin Klein --", list.Count);
                                                ViewList(list);
                                                break;
                                            case 4:
                                                Console.Clear();
                                                Console.WriteLine(ProjectTitle());
                                                list = iBL.GetByBrand("Mont Blanc");
                                                Console.WriteLine(" -- Found {0} results from Mont Blanc --", list.Count);
                                                ViewList(list);
                                                break;
                                            case 5:
                                                Console.Clear();
                                                Console.WriteLine(ProjectTitle());
                                                list = iBL.GetByBrand("Christian Dior");
                                                Console.WriteLine(" -- Found {0} results from Christian Dior --", list.Count);
                                                ViewList(list);
                                                break;
                                            case 6:
                                                Console.Clear();
                                                Console.WriteLine(ProjectTitle());
                                                list = iBL.GetByBrand("Jimmy Choo");
                                                Console.WriteLine(" -- Found {0} results from Jimmy Choo --", list.Count);
                                                ViewList(list);
                                                break;
                                        }
                                        
                                    } while (brandSelect != brands.Length);
                                    break;
                                case 5: //Show ALL
                                    Console.Clear();
                                    Console.WriteLine(ProjectTitle());
                                    Console.Write(DrawTitle("ALL PRODUCTS"));
                                    list = iBL.GetAll();
                                    Console.WriteLine(" -- Found {0} results --", list.Count);
                                    ViewList(list);
                                    break;
                            }
                        } while (imChoose != imMenu.Length);
                        break;
                /*  case 2:
                        Console.Clear();
                        Console.WriteLine($"{"PF15 - PROJECT 01 - PERFUME SHOP",15}\n");
                        string[] cusMN = {"Add Customer", "Customer List", "Back"};
                        short select = 0;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine($"{"PF15 - PROJECT 01 - PERFUME SHOP",15}\n");
                            select = Menu("Customers Management", cusMN);
                            switch (select)
                            {
                                case 1:
                                    Console.Clear();
                                    Console.WriteLine($"{"PF15 - PROJECT 01 - PERFUME SHOP",15}\n");
                                    Console.Write(DrawTitle("New Customer"));
                                    Console.Write("Enter customer name: ");
                                    string customerName = Console.ReadLine();
                                    string customerPhone;
                                    bool validPhone = false;
                                    int validPhone1 = 0, validPhone2 = 0;
                                    do
                                    {
                                        Console.Write("Enter customer phone number: ");
                                        customerPhone = Console.ReadLine();
                                        if (customerPhone.All(char.IsDigit))
                                        {
                                            validPhone1 = 1;
                                        }
                                        else
                                        {
                                            Console.WriteLine("Phone number is invalid!");
                                            Console.ReadKey();
                                        }

                                        if (customerPhone.Length > 10 || customerPhone.Length < 10)
                                        {
                                            Console.WriteLine("Phone number must have 10 digits");
                                            Console.ReadKey();
                                        }
                                        else validPhone2 = 1;

                                        if (validPhone1 == 1 && validPhone2 == 1) validPhone = true;
                                        else validPhone = false;
                                        
                                    } while (validPhone != true);
                                    Console.Write("Enter customer Address: ");
                                    string customerAddress = Console.ReadLine();
                                    Customer cus = new Customer { CustomerName = customerName, CustomerPhone = customerPhone, CustomerAddress = customerAddress};
                                    Console.WriteLine("...");
                                    Console.WriteLine("Add New Customer Completed! Customer ID is: " + cusBL.AddCustomer(cus));
                                    Console.ReadLine();
                                    break;    
                                case 2:

                                    break;
                                
                            }
                        } while (select != cusMN.Length);
                        break;      */
                    case 2:
                        Invoice invoice;
                        string[] choiceMN = {"New Invoice", "Invoices List", "Back"};
                        short choice = 0;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine(ProjectTitle());
                            choice = Menu("CREATE INVOICE", choiceMN);
                            switch (choice)
                            {
                                case 1:
                                    Console.Clear();
                                    Console.WriteLine(ProjectTitle());
                                    invoice = new Invoice();
                                    Console.Write(DrawTitle(" Add Customer"));
                                    string customerPhone;
                                    bool validPhone = false;
                                    int validPhone1 = 0, validPhone2 = 0;
                                    do
                                    {
                                        Console.Write(" Enter Customer phone number: ");
                                        customerPhone = Console.ReadLine();
                                        if (customerPhone.All(char.IsDigit))
                                        {
                                            validPhone1 = 1;
                                        }
                                        else
                                        {
                                            Console.WriteLine(" Phone number is invalid!");
                                            Console.ReadKey();
                                        }

                                        if (customerPhone.Length > 10 || customerPhone.Length < 10)
                                        {
                                            Console.WriteLine(" Phone number must have 10 digits");
                                            Console.ReadKey();
                                        }
                                        else validPhone2 = 1;

                                        if (validPhone1 == 1 && validPhone2 == 1) validPhone = true;
                                        else validPhone = false;
                                        
                                    } while (validPhone != true);
                                    Customer cus = cusBL.GetByPhone(customerPhone);
                                    if(cus != null)
                                    {
                                        string found = " -- Found 1 customer with phone number: " + customerPhone + " --\n";
                                        Console.WriteLine("\n" + found);
                                        Console.Write(" ");
                                        for (int i = 0; i < found.Length; i++) Console.Write("-");
                                        Console.WriteLine("\n Customer Name   : " + cus.CustomerName);
                                        Console.WriteLine(" Customer Address: " + cus.CustomerAddress);
                                        Console.Write(" ");
                                        for (int i = 0; i < found.Length; i++) Console.Write("-");
                                        Console.WriteLine("\n # Press 0 to turn back. Press any other keys to continue.");
                                        Console.Write(" # Your decision: ");
                                        string input = Console.ReadLine();
                                        if (input == "0") break;
                                        else invoice.InvoiceCustomer = cus;

                                    }
                                    else
                                    {
                                        Console.Write(" Enter customer name: ");
                                        string customerName = Console.ReadLine();
                                        Console.Write(" Enter customer Address: ");
                                        string customerAddress = Console.ReadLine();
                                        invoice.InvoiceCustomer = new Customer {Customer_ID = null, CustomerName = customerName, CustomerPhone = customerPhone, CustomerAddress = customerAddress};
                                        Console.WriteLine(" Add New Customer complete!\n # Press 0 to turn back, or any other keys to continue.");
                                        Console.Write(" # Your decision: ");
                                        string input = Console.ReadLine();
                                        if (input == "0") break;

                                    }
                                    
                                    string decision;
                                    int position = 0;
                                    Perfume p = new Perfume();
                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine(ProjectTitle());
                                        Console.Write(DrawTitle("Add perfumes to Invoice"));
                                        int itemID;
                                        int step = 0;
                                        bool contain;
                                        do
                                        {
                                            contain = false;
                                            Console.Write(" Input perfume ID: ");
                                            if (Int32.TryParse(Console.ReadLine(), out itemID))
                                            {
                                                foreach(Perfume per in invoice.ItemsList)
                                                {
                                                    if (itemID == per.Perfume_ID)
                                                    {
                                                        Console.Write(" This product has already been added!");
                                                        contain = true;
                                                    }
                                                }
                                                if(contain == true)
                                                {
                                                    step = 0;
                                                    Console.ReadLine();
                                                }
                                                else
                                                {
                                                    p = iBL.GetItemByID(itemID);
                                                    invoice.ItemsList.Add(p);
                                                    if (p != null)
                                                    {
                                                        ViewPerfumeInfor(p, itemID);
                                                        int quantity;
                                                        while(true)
                                                        {
                                                            Console.Write(" Input Perfume's quantity: ");
                                                            if(Int32.TryParse(Console.ReadLine(), out quantity))
                                                            {
                                                                if(quantity < 0 )
                                                                {
                                                                    Console.WriteLine(" Value cannot be negative num!");
                                                                    Console.ReadLine();
                                                                }
                                                                else if(quantity > p.TotalQuantity)
                                                                {
                                                                    Console.WriteLine(" The quantity input is too much!");
                                                                    Console.WriteLine(" Total quantity of the Perfume is: " + p.TotalQuantity);
                                                                    Console.ReadLine();
                                                                }
                                                                else if(itemID == 0)
                                                                {
                                                                    Console.Write("Remove Product!");
                                                                    invoice.ItemsList.RemoveAt(position);
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    invoice.ItemsList[position].TotalQuantity = quantity;
                                                                    position++;
                                                                    break;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine(" !! Input an number !!");
                                                                Console.ReadLine();
                                                            }

                                                        };
                                                        step = 1;
                                                    }
                                                    else
                                                    {
                                                        step = 0;
                                                        Console.WriteLine(" There is no item with ID " + itemID);
                                                        Console.ReadLine();
                                                    }
                                                }

                                            }
                                            else
                                            {
                                                step = 0;
                                                Console.WriteLine(" ID input must be number!");
                                                Console.ReadLine();
                                            }
                                        }while (step != 1);
                                        int then = 0;
                                        do
                                        {
                                            Console.WriteLine(" Do you want to add any product ? -- Yes(Y) or No(N) --");
                                            Console.Write(" Your decision: ");
                                            decision = Console.ReadLine();
                                            if(decision == "N" || decision == "n") break;
                                            if(decision == "Y" || decision == "y") then = 1;
                                            else 
                                            {
                                                Console.Write("Invalid option!");
                                                then = 0;
                                                Console.ReadLine();
                                            }
                                        } while (then != 1);
                                        if(decision == "N" || decision == "n") break;
                                    }while (decision == "Y" || decision == "y");
                                    decimal money;
                                    decimal exchange;
                                    decimal price, total = 0;
                                    do
                                    {
                                        foreach( Perfume per in invoice.ItemsList)
                                        {
                                            price = per.PerfumePrice * per.TotalQuantity;
                                            total = total + price;
                                        }
                                        Console.WriteLine(" Total have to pay: " + total);
                                        Console.Write(" Enter Customer pay: ");
                                        if(Decimal.TryParse(Console.ReadLine(), out money))
                                        {
                                            exchange = money - total;
                                            if(money < total)
                                            {
                                                Console.Write(" The cash does not enough!");
                                                Console.ReadLine();
                                            }
                                            else break;
                                            
                                        }
                                        else
                                        {
                                            Console.Write(" Invalid input!");
                                            Console.ReadLine();
                                        }
                                        
                                    } while (true);
                                    int commit;
                                    string[] commitMn = {"Export Invoice", "Cancel"};
                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine(ProjectTitle());
                                        Console.WriteLine();
                                        Console.Write(" ");
                                        for (int i = 0; i < 57; i++) Console.Write("_");
                                        Console.WriteLine();
                                        Console.WriteLine(" |{0,28}{1,-28}|", "╔╗ ┌─┐┌─┐┬ ┬┌┬┐┬┌─┐┌─┐", " ╦┌┐┌┬  ┬┌─┐┬┌─┐┌─┐");
                                        Console.WriteLine(" |{0,28}{1,-28}|", "╠╩╗├┤ ├─┤│ │ │ │├┤ └─┐", " ║│││└┐┌┘│ │││  ├┤");
                                        Console.WriteLine(" |{0,28}{1,-28}|", "╚═╝└─┘┴ ┴└─┘ ┴ ┴└─┘└─┘", " ╩┘└┘ └┘ └─┘┴└─┘└─┘");
                                        Console.WriteLine(" |{0,28}{1,-28}|", "S H - P e", " r f u m e");
                                        Console.Write(" |");
                                        for (int i = 0; i < 56; i++) Console.Write("_");
                                        Console.WriteLine("|");
                                        Console.WriteLine(" |{0,15}{1,41}|", "Product List", " ");
                                        Console.WriteLine(" | +===+=============================+========+========+  |");
                                        Console.WriteLine(" | |NUM| Perfume's name              |Quantity| Price  |  |");
                                        Console.WriteLine(" | +===+=============================+========+========+  |");
                                        decimal unitPrice;
                                        int j = 1;
                                        foreach( Perfume per in invoice.ItemsList)
                                        {
                                            unitPrice = per.PerfumePrice * per.TotalQuantity;
                                            Console.WriteLine(" | |{0,3}| {1,-28}|{2,8}|{3,8}|  |", j, per.PerfumeName, per.TotalQuantity,"$" + unitPrice);
                                            j++;
                                        }
                                        Console.WriteLine(" | +===+=============================+========+========+  |");
                                        Console.WriteLine(" | |{0,-25}{1,26}|  |", "Total        : ", total);
                                        Console.WriteLine(" | |{0,-25}{1,26}|  |", "Customer Pay : ", money);
                                        Console.WriteLine(" | |{0,-25}{1,26}|  |", "Exchange     : ", exchange);
                                        Console.WriteLine(" | +===+=============================+========+========+  |");
                                        Console.WriteLine(" |{0,56}|", " ");
                                        Console.WriteLine(" |{0,16}{1,40}|", "Client Details", " ");
                                        Console.WriteLine(" | Name     : {0,-44}|",invoice.InvoiceCustomer.CustomerName);
                                        Console.WriteLine(" | Phone    : {0,-44}|",invoice.InvoiceCustomer.CustomerPhone);
                                        Console.WriteLine(" | Address  : {0,-44}|",invoice.InvoiceCustomer.CustomerAddress);
                                        Console.WriteLine(" |{0,56}|", " ");
                                        Console.WriteLine(" |{0,56}|", "Keep this invoice for returning and installment! ");
                                        Console.Write(" |");
                                        for (int i = 0; i < 56; i++) Console.Write("_");
                                        Console.WriteLine("|");
                                        commit = Menu("Commit Invoice", commitMn);
                                        switch(commit)
                                        {
                                            case 1: 
                                                Console.WriteLine(" Create Invoice: " + (ivBL.CreateInvoice(invoice) ? "Complete!" : "Incompleted!"));
                                                commit = 2;
                                                Console.ReadLine();
                                                break;
                                            case 2:

                                                break;
                                                    
                                        }

                                    }while (commit != commitMn.Length);


                                    break;
                                case 2:                                  
                                    invList = ivBL.GetInvoices();
                                    int size = 5;
                                    List<List<Invoice>> chunks = new List<List<Invoice>>();
                                    int chunkCount = invList.Count / size;

                                    if (invList.Count % size > 0)  chunkCount++;
                                    
                                    for (var i = 0; i < chunkCount; i++) chunks.Add(invList.Skip(i * size).Take(size).ToList());

                                    int page = 0, pages;
                                    string choose;
                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine(ProjectTitle() + "\n");
                                        Console.WriteLine(" +=====+==============================+========+============+");
                                        Console.WriteLine(" | ID  | Customer Name                | Total  | Status     |");
                                        Console.WriteLine(" +=====+==============================+========+============+");
                                        decimal productPrice, totalInvoice = 0;
                                        string statusInvoice = " ";
                                        foreach(Invoice inv in chunks[page])
                                        {
                                            foreach(Perfume product in inv.ItemsList)
                                            {
                                                productPrice = product.PerfumePrice * product.TotalQuantity;
                                                totalInvoice = totalInvoice + productPrice;
                                            }
                                            switch(inv.Status)
                                            {
                                                case 1: 
                                                    statusInvoice = "CREATED";
                                                    break;
                                                case 2:
                                                    statusInvoice = "COMPLETED";
                                                    break;
                                                case 3: 
                                                    statusInvoice = "CANCELED";
                                                    break;
                                            }
                                            Console.WriteLine(" | {0,-4}| {1,-29}| {2,-7}| {3,-11}|", inv.Invoice_ID, inv.InvoiceCustomer.CustomerName, totalInvoice, statusInvoice);

                                        }
                                        pages = page + 1;
                                        Console.WriteLine($"{"-- Page " + pages + "/" + chunkCount + " --",45}");
                                        Console.Write(" # Press N(next) or B(back) to see the next or previous page. X to Back menu.\n Input Invoice ID to see details.\n # Your choice: ");
                                        choose = Console.ReadLine();
                                        if(choose == "N" || choose == "n")
                                        {
                                            if(page >= 0 && page < (chunkCount - 1))
                                            {
                                                page++;
                                                Console.ReadLine();
                                            }
                                            else
                                            {
                                                Console.Write(" You are at the last page!");
                                                Console.ReadLine();
                                            }
                                        }
                                        if(choose == "B" || choose == "b")
                                        {
                                            if(page > 0 && page <= (chunkCount - 1))
                                            {
                                                page--;
                                                Console.ReadLine();
                                            }
                                            else
                                            {
                                                Console.Write(" You are at the first page!");
                                                Console.ReadLine();
                                            }
                                        }
                                        if(Int32.TryParse(choose, out int invoiceID))
                                        {
                                            invoice = ivBL.GetInvoiceByID(invoiceID);
                                            if(invoice != null)
                                            {
                                                Console.Clear();
                                                Console.WriteLine(ProjectTitle() + "\n");
                                                Console.Write(DrawTitle("INVOICE ID - " + invoice.Invoice_ID));
                                                Console.WriteLine($" | CUSTOMER: {invoice.InvoiceCustomer.CustomerName, -64}|");
                                                Console.WriteLine(" |{0,15}{1,60}|", "Product List", " ");
                                                Console.WriteLine(" | +===+=============================+========+========+                     |");
                                                Console.WriteLine(" | |NUM| Perfume's name              |Quantity| Price  |                     |");
                                                Console.WriteLine(" | +===+=============================+========+========+                     |");
                                                decimal unitPrice;
                                                decimal totalPay = 0;
                                                int j = 1;
                                                foreach( Perfume per in invoice.ItemsList)
                                                {
                                                    unitPrice = per.PerfumePrice * per.TotalQuantity;
                                                    Console.WriteLine(" | |{0,3}| {1,-24}|{2,8}|{3,8}|                         |", j, per.PerfumeName, per.TotalQuantity,"$" + unitPrice);
                                                    j++;
                                                    totalPay = totalPay + unitPrice;
                                                }
                                                Console.WriteLine(" | +===+=============================+========+========+                     |");
                                                Console.WriteLine(" | |{0,-23}{1,24}|                         |", "Total : ", totalPay);
                                                Console.WriteLine(" | +===+=============================+========+========+                     |");
                                                switch(invoice.Status)
                                                {
                                                    case 1: 
                                                        statusInvoice = "CREATED";
                                                        break;
                                                    case 2:
                                                        statusInvoice = "COMPLETED";
                                                        break;
                                                    case 3: 
                                                        statusInvoice = "CANCELED";
                                                        break;
                                                }
                                                Console.Write(DrawTitle("Invoice Status: " + statusInvoice));
                                                string changeDecision;
                                                string[] statusMn = {"CREATED", "COMPLETED", "CANCELED"};
                                                short statusChange = 0;
                                                Console.WriteLine(" # CHANGE THIS INVOICE STATUS ? -- Y to continue / Any key to skip --");
                                                Console.Write(" # YOUR CHOICE: ");
                                                changeDecision = Console.ReadLine();
                                                if(changeDecision == "Y" || changeDecision == "y")
                                                {
                                                    statusChange = Menu("CHANGE INVOICE STATUS", statusMn);
                                                    Console.WriteLine(" Change invoice status: " + (ivBL.ChangeInvoiceStatus(invoice, statusChange) ? "Completed!" : "Incompleted!"));
                                                    choose = "X";
                                                    Console.ReadLine();
                                                }
                                                else Console.Write(" Returning to invoice list...");
                                                Console.ReadLine();

                                            }
                                            else
                                            {
                                                Console.WriteLine("Invoice not exist!");
                                                Console.ReadLine();
                                            }
                                        }
                                    }while(choose != "X");
                                    break;
                            }
                        } while(choice != choiceMN.Length);

                        break;
                }
            } while (mainChoose != mainMenu.Length);
        }

        static string GetPassword()
        {
            var pass = string.Empty;
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    Console.Write("\b \b");
                    pass = pass[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    pass += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
            return pass;
        }

        static string ProjectTitle()
        {
            string title = 
@"  _____ _   _       ____________  ___  ___________  ___  _   _ _____ _____ 
 /  ___| | | |      |  ___| ___ \/ _ \|  __ \ ___ \/ _ \| \ | /  __ \  ___|
 \ `--.| |_| |______| |_  | |_/ / /_\ \ |  \/ |_/ / /_\ \  \| | /  \/ |__  
  `--. \  _  |______|  _| |    /|  _  | | __|    /|  _  | . ` | |   |  __| 
 /\__/ / | | |      | |   | |\ \| | | | |_\ \ |\ \| | | | |\  | \__/\ |___ 
 \____/\_| |_/      \_|   \_| \_\_| |_/\____|_| \_\_| |_|_| \_/\____|____/
 =================================.-/\-.==================================
 \_______________________________| /  \ |________________________________/
 /'''''''''''''''''''''''''''''''| \  / |''''''''''''''''''''''''''''''''\
 '-.____________________________/ '-\/-' \_____________________________.-'";
            return title;
        }
        
        static string DrawTitle(string title)
        {
            string titleDrew =" +-----------------------------------------------------------------------+\n"
                            + $" |{title, 35}" + $"{" ", 36}|"
                            + "\n +-----------------------------------------------------------------------+\n";
            return titleDrew;
        }

        private static short Menu(string title, string[] listChoices)
        {
            short choose = 0;
            Console.Write(DrawTitle(title));
            for (int i = 0; i < listChoices.Length; i++)
            {
                Console.WriteLine(" | " + (i + 1) + ". " + $"{listChoices[i], -67}|");
            }
            Console.Write(" +-----------------------------------------------------------------------+\n");
            do
            {
                Console.Write(" # Your choice: ");
                try
                {
                    choose = Int16.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine(" # Your Choice is invalid!");
                    continue;
                }
            } while (choose <= 0 || choose > listChoices.Length);
            return choose;
        }

        private static void ViewList(List<Perfume> list)
        {
            int size = 5;
            List<List<Perfume>> chunks = new List<List<Perfume>>();
            int chunkCount = list.Count / size;

            if (list.Count % size > 0)  chunkCount++;
               
            for (var i = 0; i < chunkCount; i++) chunks.Add(list.Skip(i * size).Take(size).ToList());
        
            int page = 0, pages;
            string choice;
            do
            {
                Console.WriteLine(" +=====+===============================+=====================+=========+===========+");
                Console.WriteLine(" | ID  | Product Name                  | Brand               | Gender  | Price     |");
                Console.WriteLine(" +-----+-------------------------------+---------------------+---------+-----------+");
                string price;
                foreach (Perfume p in chunks[page])
                {
                    price = p.PerfumePrice + "USD";
                    Console.WriteLine(" | {0,-4}| {1,-30}| {2, -20}| {3,-8}| {4,-10}|", p.Perfume_ID, p.PerfumeName, p.BrandName, p.Gender, price);
                }
                Console.WriteLine(" +-----+-------------------------------+---------------------+---------+-----------+");
                pages = page + 1;
                Console.WriteLine($"{"-- Page " + pages + "/" + chunkCount + " --",45}");
                Console.Write(" # Press N(next) or B(back) to see the next or previous page. Enter 0 to Back menu.\n # Your choice: ");
                choice = Console.ReadLine();
                if(choice == "N" || choice == "n")
                {
                    if(page >= 0 && page < (chunkCount - 1))
                    {
                        page++;
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.Write(" You are at the last page!");
                        Console.ReadLine();
                    }
                }
                if(choice == "B" || choice == "b")
                {
                    if(page > 0 && page <= (chunkCount - 1))
                    {
                        page--;
                        Console.ReadLine();
                    }
                    else
                    {
                        Console.Write(" You are at the first page!");
                        Console.ReadLine();
                    }
                }
            } while (choice != "0");
            Console.ReadLine();
        }

        private static void ViewPerfumeInfor(Perfume p, int itemID)
        {
            Console.Clear();
            Console.WriteLine(ProjectTitle());
            Console.WriteLine(" Found 1 product for ID: " + itemID);
            string perfumeName = " Perfume name: " + p.PerfumeName + " - ID: " + p.Perfume_ID + "\n ";
            Console.WriteLine(perfumeName);
            for (int i = 0; i < perfumeName.Length; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine("\n Fragrance family      : " + p.FragranceFamily);
            Console.WriteLine(" Classification        : " + p.Volume);
            Console.WriteLine(" Top notes             : " + p.TopNotes);
            Console.WriteLine(" Heart notes           : " + p.HeartNotes);
            Console.WriteLine(" Base notes            : " + p.BaseNotes);
            Console.WriteLine(" Gender                : " + p.Gender);
            CutString(p.Ingredients, " Ingredients           : ");
            Console.WriteLine("\n Form                  : " + p.Form);
            Console.WriteLine(" Year launched         : " + p.YearLaunched);
            Console.WriteLine(" Strength              : " + p.Strength);
            Console.WriteLine(" Origin                : " + p.Origin);
            Console.WriteLine(" Quantity              : " + p.TotalQuantity);                                                    
            Console.WriteLine(" Status                : " + p.Status);
            Console.WriteLine(" Brand                 : " + p.BrandName);
            CutString(p.Description, " Description           : ");
            Console.WriteLine("\n Price                 : " + p.PerfumePrice);
            for (int i = 0; i < perfumeName.Length; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
        }

        

        private static void CutString(string str, string desc)
        {
            string[] subStr = str.Split(' ');
            int count = subStr.Length;
            
            Console.Write(desc + subStr[0]);
            for (int i = 1; i < subStr.Length; i++)
            {
                if (i % 7 == 0) Console.Write("\n\t\t\t");
                Console.Write(subStr[i] + " ");
            }
        }
    }
}