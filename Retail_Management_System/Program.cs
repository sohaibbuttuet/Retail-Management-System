using RMS.BL;
using RMS.UI;
using RMS.Models;
using System;
using System.Collections.Generic;

namespace RMS
{
    internal class Program
    {
        static CatalogUI catalogUI = new CatalogUI();
        static SalesUI salesUI = new SalesUI();
        static AdminUI adminUI = new AdminUI();
        static InvoiceUI invoiceUI = new InvoiceUI();
        static CustomerUI customerUI = new CustomerUI();
        static AdminBL adminBL = new AdminBL();

        static void Main(string[] args)
        {
            WelcomeScreen();

            Admin loggedIn = LoginUI();

            if (loggedIn != null)
            {
                ShowMenu();
            }
        }

        // =========================
        // LOGIN (DB BASED)
        // =========================
        static Admin LoginUI()
        {
            int attempts = 3;

            Console.WriteLine("Dear User, you have only 3 attempts to login.");

            while (attempts > 0)
            {
                Console.WriteLine($"\nRemaining Attempts: {attempts}\n");

                Console.Write("Enter Username: ");
                string username = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                try
                {
                    Admin admin = adminBL.Login(username, password);
                    Console.WriteLine("\n Login Successful!\n");
                    return admin;
                }
                catch (Exception ex)
                {
                    attempts--;
                    Console.WriteLine("\nError: " + ex.Message);
                }
            }

            Console.WriteLine("\nAccess Denied. Restart program.");
            Environment.Exit(0);  // forcefully stop the entire program immediately
            return null;
        }

        // =========================
        static void WelcomeScreen()
        {
            Console.WriteLine("\n*******************************************************");
            Console.WriteLine("*        Welcome to Retail Management System          *");
            Console.WriteLine("*******************************************************\n");
        }

        // =========================
        static void ShowMenu()
        {
            while (true)
            {
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("\n===== RMS SYSTEM =====");
                Console.WriteLine("1. Catalog Management");
                Console.WriteLine("2. Customer Management");
                Console.WriteLine("3. Sales (Buy / Return)");
                Console.WriteLine("4. Invoices Report");
                Console.WriteLine("5. Admin Management");
                Console.WriteLine("0. Exit");
                Console.Write("Select: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        catalogUI.ShowMenu();
                        break;
                    case "2":
                        customerUI.ShowMenu();
                        break;
                    case "3":
                        salesUI.ShowMenu();
                        break;
                    case "4":
                        invoiceUI.ShowMenu();
                        break;
                    case "5":                        
                        adminUI.ShowMenu();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice!");
                        Console.ReadKey();
                        break;
                }
            }
        }
    }
}