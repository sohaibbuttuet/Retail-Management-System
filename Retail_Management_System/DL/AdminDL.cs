using MySql.Data.MySqlClient; // Required for MySqlParameter
using System;
using System.Collections.Generic;
using System.Data;
using RMS.Models;

namespace RMS.DL
{
    internal class AdminDL
    {
        public static Admin LogIn(string username, string password)
        {
            string query = "SELECT AdminID, Username, Password, CreatedAt FROM admins WHERE Username = @Username AND Password = @Password;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@Username", username),
                new MySqlParameter("@Password", password)
            };

            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return MapRow(dt.Rows[0]);
        }

        public static int Add_Admin(Admin admin)
        {
            string query = "INSERT INTO admins (UserName, Password, CreatedAt) VALUES (@UserName, @Password, @CreatedAt); SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = {
                new MySqlParameter("@UserName", admin.UserName),
                new MySqlParameter("@Password", admin.Password),
                new MySqlParameter("@CreatedAt", admin.DateOfCreation) // The parameter handles the date formatting safely
            };

            return DatabaseHelper.Instance.ExecuteScalar(query, parameters);
        }

        public static void Update_Admin(Admin admin)
        {
            string query = "UPDATE admins SET Username = @Username, Password = @Password WHERE AdminID = @AdminID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@Username", admin.UserName),
                new MySqlParameter("@Password", admin.Password),
                new MySqlParameter("@AdminID", admin.AdminID)
            };

            DatabaseHelper.Instance.Update(query, parameters);
        }

        public static void Delete_Admin(int adminID)
        {
            string query = "DELETE FROM admins WHERE AdminID = @AdminID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@AdminID", adminID)
            };

            DatabaseHelper.Instance.Update(query, parameters);
        }

        public static Admin Get_Admin(int adminID)
        {
            string query = "SELECT AdminID, Username, Password, CreatedAt FROM admins WHERE AdminID = @AdminID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@AdminID", adminID)
            };

            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return MapRow(dt.Rows[0]);
        }

        public static Admin GetAdminByUsername(string username)
        {
            string query = "SELECT AdminID, Username, Password, CreatedAt FROM admins WHERE Username = @Username;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@Username", username)
            };

            DataTable dt = DatabaseHelper.Instance.GetDataTable(query, parameters);

            if (dt.Rows.Count == 0)
                return null;

            return MapRow(dt.Rows[0]);
        }

        public static List<Admin> GetAllAdmins()
        {
            string query = "SELECT AdminID, Username, Password, CreatedAt FROM admins;";
            DataTable dt = DatabaseHelper.Instance.GetDataTable(query);

            List<Admin> admins = new List<Admin>();
            foreach (DataRow row in dt.Rows)
            {
                admins.Add(MapRow(row));
            }
            return admins;
        }

        public static bool AdminExists(int adminId)
        {
            string query = "SELECT COUNT(*) FROM admins WHERE AdminID = @AdminID;";
            MySqlParameter[] parameters = {
                new MySqlParameter("@AdminID", adminId)
            };

            int count = DatabaseHelper.Instance.ExecuteScalar(query, parameters);
            return count > 0;
        }

        private static Admin MapRow(DataRow row)
        {
            return new Admin
            {
                AdminID = Convert.ToInt32(row["AdminID"]),
                UserName = row["Username"].ToString(),
                Password = row["Password"].ToString(),
                DateOfCreation = Convert.ToDateTime(row["CreatedAt"])
            };
        }
    }
}