using RMS.BL;
using RMS.Models;
using System;
using System.Collections.Generic;

namespace RMS.UI
{
    internal class AdminUI
    {
        private AdminBL adminBL = new AdminBL();

        private void AddAdminUI()
        {
            try
            {
                Console.Write("Enter Username: ");
                string username = Console.ReadLine();

                Console.Write("Enter Password: ");
                string password = Console.ReadLine();

                Admin admin = new Admin(username, password);

                adminBL.Add_Admin(admin);

                Console.WriteLine("\nAdmin added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }
        private void UpdateAdminUI()
        {
            try
            {
                Console.Write("Enter Admin ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("\nInvalid ID format!");
                    return;
                }

                Console.Write("Enter New Username: ");
                string username = Console.ReadLine();

                Console.Write("Enter New Password: ");
                string password = Console.ReadLine();

                Admin admin = new Admin(id, username, password);

                adminBL.Update_Admin(admin);

                Console.WriteLine("\nAdmin updated successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }
        private void DeleteAdminUI()
        {
            Console.Write("Enter Admin ID: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nInvalid ID format!");
                return;
            }

            Console.Write("Are you sure you want to delete (Y/N): ");
            string c = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(c))
            {
                Console.WriteLine("Invalid input!");
                return;
            }

            if (c == "n")
            {
                Console.WriteLine("Cancel Delete");
                return;
            }
            else if (c != "y")
            {
                Console.WriteLine("Invalid input! Use Y or N.");
                return;
            }

            try
            {               
                adminBL.Delete_Admin(id);

                Console.WriteLine("\nAdmin deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nError: " + ex.Message);
            }
        }
        private void ViewAllAdminsUI()
        {
            List<Admin> admins = adminBL.ViewAllAdmins();

            if (admins == null || admins.Count == 0)
            {
                Console.WriteLine("\nNo admins found.");
                return;
            }

            Console.WriteLine("\n----- ADMIN LIST -----");
            Console.WriteLine("{0,-5} {1,-20} {2,-25}", "ID", "Username", "Created At");
            Console.WriteLine(new string('-', 55));

            foreach (Admin admin in admins)
            {
                Console.WriteLine("{0,-5} {1,-20} {2,-25}",
                    admin.AdminID,
                    admin.UserName,
                    admin.DateOfCreation.ToString("yyyy-MM-dd HH:mm"));
            }
        }
        private void GetAdminUI()
        {
            try
            {
                Console.Write("Enter Admin ID: ");
                if (!int.TryParse(Console.ReadLine(), out int id))
                {
                    Console.WriteLine("\nInvalid ID format!");
                    return;
                }

                Admin admin = adminBL.GetAdmin(id);

                if (admin == null)
                {
                    Console.WriteLine("Admin not found.");
                    return;
                }

                Console.WriteLine("\n----- ADMIN DETAILS -----");
                Console.WriteLine($"ID: {admin.AdminID}");
                Console.WriteLine($"Username: {admin.UserName}");
                Console.WriteLine($"Created At: {admin.DateOfCreation}\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        // =========================
        // MAIN MENU
        // =========================
        public void ShowMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("\n========== ADMIN MENU ==========");
                Console.WriteLine("1. Add Admin");
                Console.WriteLine("2. Update Admin");
                Console.WriteLine("3. Delete Admin");
                Console.WriteLine("4. View All Admins");
                Console.WriteLine("5. View Admin By ID");
                Console.WriteLine("0. Back");
                Console.Write("Enter choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddAdminUI();
                        Pause();
                        break;
                    case "2":
                        UpdateAdminUI();
                        Pause();
                        break;
                    case "3":
                        DeleteAdminUI();
                        Pause();
                        break;
                    case "4":
                        ViewAllAdminsUI();
                        Pause();
                        break;
                    case "5":
                        GetAdminUI();
                        Pause();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid choice!");
                        Pause();
                        break;
                }
            }
        }

        // =========================
        // PAUSE
        // =========================
        private void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}