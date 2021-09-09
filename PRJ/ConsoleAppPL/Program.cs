using System;
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
            Console.Write(DrawTitle("Login"));
            Console.Write("User Name: ");
            userName = Console.ReadLine();
            Console.Write("Password: ");
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
                Console.WriteLine("Can't login! Try again...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Welcome ...");
            }
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

        static string DrawTitle(string title)
        {
            string titleDrew ="\n+------------------------------+\n"
                            + $"|{title, 15}" + $"{" ", 15}|"
                            + "\n+------------------------------+\n";
            return titleDrew;
        }

        private static short Menu(string title, string[] listChoices)
        {
            short choose = 0;
            Console.Write(DrawTitle(title));
            for (int i = 0; i < listChoices.Length; i++)
            {
                Console.WriteLine("| " + (i + 1) + ". " + $"{listChoices[i], -25}");
            }
            Console.Write("\n+------------------------------+\n");
            do
            {
                Console.Write("Your choice: ");
                try
                {
                    choose = Int16.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Your Choice is invalid!");
                    continue;
                }
            } while (choose <= 0 || choose > listChoices.Length);
            return choose;
        }
    }
}
