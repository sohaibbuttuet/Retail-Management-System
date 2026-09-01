using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Models
{
    internal class Admin
    {
        private int adminID;
        private string userName;
        private string password;
        DateTime dateOfCreation;

        public int AdminID
        {
            get { return adminID; }
            set { adminID = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public DateTime DateOfCreation
        {
            get { return dateOfCreation; }
            set
            {
                if (value > DateTime.Now)
                {
                    throw new ArgumentException("Invoice date cannot be in the future.");
                }
                dateOfCreation = value;
            }
        }
        public Admin()
        {
            dateOfCreation = DateTime.Now;
        }
        public Admin(string userName, string password)
        {
            UserName = userName;
            Password = password;
            DateOfCreation = DateTime.Now;
        }
        public Admin(int id, string userName, string password)
        {
            AdminID = id;
            UserName = userName;
            Password = password;
        }
    }
}
