using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RMS.DL;
using RMS.Models;

namespace RMS.BL
{
    internal class AdminBL
    {
        public Admin Login(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) && string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Username and Password cannot be empty.");

            if (string.IsNullOrWhiteSpace(userName))
                throw new InvalidOperationException("Username cannot be empty.");

            if (string.IsNullOrWhiteSpace(password))
                throw new InvalidOperationException("Password cannot be empty.");

            Admin admin = AdminDL.LogIn(userName, password);

            if (admin == null)
                throw new InvalidOperationException("Invalid username or password.");

            return admin;
        }

        public void Add_Admin(Admin admin)
        {
            if (admin == null)
                throw new ArgumentNullException(nameof(admin));

            if (string.IsNullOrWhiteSpace(admin.UserName))
                throw new ArgumentException("Username cannot be empty.");

            if (string.IsNullOrWhiteSpace(admin.Password))
                throw new ArgumentException("Password cannot be empty.");

            if (AdminDL.GetAdminByUsername(admin.UserName) != null)
                throw new InvalidOperationException("Admin with this username already exists.");

            admin.AdminID = AdminDL.Add_Admin(admin);
        }
        public void Update_Admin(Admin admin)
        {
            if (admin == null)
                throw new ArgumentNullException("Admin cannot be null.");

            if (admin.AdminID <= 0)
                throw new ArgumentOutOfRangeException("Invalid Admin ID.");

            if (string.IsNullOrWhiteSpace(admin.UserName))
                throw new ArgumentException("Username cannot be empty.");

            if (string.IsNullOrWhiteSpace(admin.Password))
                throw new ArgumentException("Password cannot be empty.");

            // 1. Check admin exists by ID
            Admin existingAdmin = AdminDL.Get_Admin(admin.AdminID);

            if (existingAdmin == null)
                throw new InvalidOperationException("Admin does not exist.");

            // 2. Check username uniqueness
            Admin userCheck = AdminDL.GetAdminByUsername(admin.UserName);

            if (userCheck != null && userCheck.AdminID != admin.AdminID)
                throw new InvalidOperationException("Username already taken.");

            // 3. Update
            AdminDL.Update_Admin(admin);
        }
        public void Delete_Admin(int adminId)
        {

            if (adminId <= 0)
                throw new ArgumentOutOfRangeException("Invalid Admin ID.");

            if (!AdminDL.AdminExists(adminId))
                throw new InvalidOperationException("Admin does not exists.");

            AdminDL.Delete_Admin(adminId);
        }
        public List<Admin> ViewAllAdmins()
        {
            return AdminDL.GetAllAdmins();
        }
        public Admin GetAdmin(int adminId)
        {
            return AdminDL.Get_Admin(adminId);
        }
    }
}
