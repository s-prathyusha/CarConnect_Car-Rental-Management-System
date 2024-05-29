using Car_Connect.exception;
using Car_Connect.util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.dao
{
    public class AuthenticationService
    {
        public bool AuthenticateCustomer(string username, string password)
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    connection.Open();
                    string query = "SELECT Password FROM Customer WHERE Username = @Username";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader.GetString(0);
                                if (HashPassword(password) == storedPassword)
                                    return true;
                                else
                                {
                                    throw new Car_Connect.exception.AuthenticationException("User Authentication Failed!");
                                }
                            }
                        }
                    }
                }
                throw new CustomerNotFoundException($"User with UserName : {username} is not found.");
            }
            catch (Car_Connect.exception.AuthenticationException ex) { Console.WriteLine("\t*" + ex.Message); return false; }
        }

        public bool AuthenticateAdmin(string username, string password)
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    connection.Open();
                    string query = "SELECT Password FROM Admin WHERE Username = @Username";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader.GetString(0);
                                if (HashPassword(password) == storedPassword)
                                    return true;
                                else
                                    throw new Car_Connect.exception.AuthenticationException("Admin Authentication Failed!");
                            }
                        }
                    }
                }
                throw new AdminNotFoundException($"Admin with UserName : {username} is not found.");
            }
            catch (AdminNotFoundException ex) { Console.WriteLine("\t*" + ex.Message); }
            return false;
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
