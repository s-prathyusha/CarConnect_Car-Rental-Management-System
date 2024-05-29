using System;
using Car_Connect.entity_model;
using Car_Connect.util;
using Car_Connect.exception;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace Car_Connect.dao
{
    public class CustomerService : ICustomerService
    {
        public void DeleteCustomer(int customerId)
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    string query = "DELETE FROM Customer WHERE CustomerID = @CustomerId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new CustomerNotFoundException($"Customer with ID {customerId} not found.");
                    }
                    else
                    {
                        Console.WriteLine("\t*Customer Data Deleted Successfully!");
                    }
                }
            }
            catch (CustomerNotFoundException ex) { Console.WriteLine("\t*" + ex.Message); }
        }

        public Customer GetCustomerById(int customerId)
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    string query = "SELECT * FROM Customer WHERE CustomerID = @CustomerId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Customer c = new Customer
                        {
                            CustomerID = Convert.ToInt32(reader["CustomerID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Address = reader["Address"].ToString(),
                            Username = reader["Username"].ToString(),
                            RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"])
                        };
                        return c;
                    }
                    throw new CustomerNotFoundException($"Customer with ID {customerId} not found.");
                }
            }
            catch (CustomerNotFoundException ex) { Console.WriteLine("\t*" + ex.Message); return null; }
        }

        public Customer GetCustomerByUsername(string username)
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    string query = "SELECT * FROM Customer WHERE Username = @Username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Customer c = new Customer
                        {
                            CustomerID = Convert.ToInt32(reader["CustomerID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Address = reader["Address"].ToString(),
                            Username = reader["Username"].ToString(),
                            RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"])
                        };
                        return c;
                    }
                    throw new CustomerNotFoundException($"Customer with username {username} not found.");

                }
            }
            catch (CustomerNotFoundException ex) { Console.WriteLine("\t*" + ex.Message); return null; }
        }

        public void RegisterCustomer(Customer customerData)
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    string query = @"INSERT INTO Customer (FirstName, LastName, Email, PhoneNumber, Address, Username, Password, RegistrationDate)
                             VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Address, @Username, @Password, @RegistrationDate)";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@FirstName", customerData.FirstName);
                    command.Parameters.AddWithValue("@LastName", customerData.LastName);
                    command.Parameters.AddWithValue("@Email", customerData.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", customerData.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", customerData.Address);
                    command.Parameters.AddWithValue("@Username", customerData.Username);
                    command.Parameters.AddWithValue("@Password", HashPassword(customerData.Password));
                    command.Parameters.AddWithValue("@RegistrationDate", customerData.RegistrationDate);
                    connection.Open();
                    command.ExecuteNonQuery();

                    string query2 = @"INSERT INTO Logins VALUES (@UserName,@Password)";
                    SqlCommand command2 = new SqlCommand(query2, connection);
                    command2.Parameters.AddWithValue("@Password", customerData.Password);
                    command2.Parameters.AddWithValue("@UserName", customerData.Username);
                    command2.ExecuteNonQuery();
                }
            }
            catch (CustomerNotFoundException ex) { Console.WriteLine("\t*" + ex.Message); }
        }

        public void UpdateCustomer(Customer customerData)
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    string query = @"UPDATE Customer 
                             SET FirstName = @FirstName, LastName = @LastName, Email = @Email, 
                                 PhoneNumber = @PhoneNumber, Address = @Address, Password = @Password
                             WHERE CustomerID = @CustomerId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", customerData.FirstName);
                    command.Parameters.AddWithValue("@LastName", customerData.LastName);
                    command.Parameters.AddWithValue("@Email", customerData.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", customerData.PhoneNumber);
                    command.Parameters.AddWithValue("@Address", customerData.Address);
                    command.Parameters.AddWithValue("@Password", HashPassword(customerData.Password));
                    command.Parameters.AddWithValue("@CustomerId", customerData.CustomerID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new CustomerNotFoundException($"Customer with ID {customerData.CustomerID} not found.");
                    }
                    else
                    {
                        string query2 = @"UPDATE Logins SET Password = @Password WHERE UserName = @UserName";
                        SqlCommand command2 = new SqlCommand(query2, connection);
                        command2.Parameters.AddWithValue("@Password", customerData.Password);
                        command2.Parameters.AddWithValue("@UserName", customerData.Username);
                        command2.ExecuteNonQuery();
                    }
                }
            }
            catch (CustomerNotFoundException ex) { Console.WriteLine("\t*" + ex.Message); }
        }
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF32.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}