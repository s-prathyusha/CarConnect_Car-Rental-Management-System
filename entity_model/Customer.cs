using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.entity_model
{
    public class Customer
    {
        private int customerID;
        private string firstName;
        private string lastName;
        private string email;
        private string phoneNumber;
        private string address;
        private string username;
        private string password;
        private DateTime registrationDate;

        public Customer()
        {

        }

        public Customer(int customerID, string firstName, string lastName, string email, string phoneNumber, string address, string username, string password, DateTime registrationDate)
        {
            this.customerID = customerID;
            this.firstName = firstName;
            this.lastName = lastName;
            this.email = email;
            this.phoneNumber = phoneNumber;
            this.address = address;
            this.username = username;
            this.password = password;
            this.registrationDate = registrationDate;
        }

        public int CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
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

        public string Address
        {
            get { return address; }
            set { address = value; }
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

        public DateTime RegistrationDate
        {
            get { return registrationDate; }
            set { registrationDate = value; }
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
