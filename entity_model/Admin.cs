using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.entity_model
{
    public class Admin
    {
        private int adminID;
        private string firstName;
        private string lastName;
        private string email;
        private string phoneNumber;
        private string username;
        private string password;
        private string role;
        private DateTime joinDate;

        public Admin()
        {

        }

        public Admin(int adminID, string firstName, string lastName, string email, string phoneNumber, string username, string password, string role, DateTime joinDate)
        {
            this.adminID = adminID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.phoneNumber = phoneNumber;
            this.username = username;
            this.password = password;
            this.role = role;
            this.joinDate = joinDate;
        }

        public int AdminID
        {
            get { return adminID; }
            set { adminID = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public string Role
        {
            get { return role; }
            set { role = value; }
        }

        public DateTime JoinDate
        {
            get { return joinDate; }
            set { joinDate = value; }
        }

        public bool Authenticate(string inputPassword)
        {
            try
            {
                if (inputPassword != Password)
                {
                    throw new AuthenticationException("Password is incorrect");

                }
            }
            catch (AuthenticationException ae)
            {
                Console.WriteLine("Error: " + ae.Message);
            }
            return inputPassword == Password;
        }
    }
}
