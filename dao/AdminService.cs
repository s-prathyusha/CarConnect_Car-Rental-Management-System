using Car_Connect.entity_model;
using Car_Connect.exception;
using Car_Connect.util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.dao
{
    internal class AdminService : IAdminService
    {
        public void DeleteAdmin(int adminId)
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    string query = "DELETE FROM Admin WHERE AdminID = @AdminId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@AdminId", adminId);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new AdminNotFoundException($"Admin with ID {adminId} not found.");

                    }
                    else
                    {
                        Console.WriteLine("\t*Admin deleted successfully.");
                    }
                }
            }
            catch (AdminNotFoundException ex) { Console.WriteLine("\t*" + ex.Message); }
        }

        public Admin GetAdminById(int adminId)
        {
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = "SELECT * FROM Admin WHERE AdminID = @AdminId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@AdminId", adminId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Admin admin = new Admin
                    {
                        AdminID = Convert.ToInt32(reader["AdminID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Username = reader["Username"].ToString(),
                        Password = reader["Password"].ToString(),
                        Role = reader["Role"].ToString(),
                        JoinDate = Convert.ToDateTime(reader["JoinDate"])
                    };
                    return admin;
                }
                try { throw new AdminNotFoundException($"Admin with ID {adminId} not found."); }
                catch (AdminNotFoundException ex) { Console.WriteLine("\t*" + ex.Message); return null; }
            }
        }

        public Admin GetAdminByUsername(string username)
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    string query = "SELECT * FROM Admin WHERE Username = @Username";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        Admin admin = new Admin
                        {
                            AdminID = Convert.ToInt32(reader["AdminID"]),
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString(),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = reader["Role"].ToString(),
                            JoinDate = Convert.ToDateTime(reader["JoinDate"])
                        };
                        return admin;
                    }
                    throw new AdminNotFoundException($"Admin with username {username} not found.");

                }
            }
            catch (AdminNotFoundException ex) { Console.WriteLine("\t*" + ex.Message); return null; }
        }

        public void RegisterAdmin(Admin adminData)
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    string query = @"INSERT INTO Admin (FirstName, LastName, Email, PhoneNumber, Username, Password, Role, JoinDate)
                             VALUES (@FirstName, @LastName, @Email, @PhoneNumber, @Username,@Password, @Role, @JoinDate)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", adminData.FirstName);
                    command.Parameters.AddWithValue("@LastName", adminData.LastName);
                    command.Parameters.AddWithValue("@Email", adminData.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", adminData.PhoneNumber);
                    command.Parameters.AddWithValue("@Username", adminData.Username);
                    command.Parameters.AddWithValue("@Password", HashPassword(adminData.Password));
                    command.Parameters.AddWithValue("@Role", adminData.Role);
                    command.Parameters.AddWithValue("@JoinDate", adminData.JoinDate);
                    connection.Open();
                    command.ExecuteNonQuery();

                    string query2 = @"INSERT INTO Logins VALUES (@UserName,@Password)";
                    SqlCommand command2 = new SqlCommand(query2, connection);
                    command2.Parameters.AddWithValue("@Password", adminData.Password);
                    command2.Parameters.AddWithValue("@UserName", adminData.Username);
                    command2.ExecuteNonQuery();
                }
                Console.WriteLine("\t*Admin registered successfully.");
            }
            catch (AdminNotFoundException ex) { Console.WriteLine("\t*" + ex.Message); }

        }

        public void UpdateAdmin(Admin adminData)
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    string query = @"UPDATE Admin 
                             SET FirstName = @FirstName, LastName = @LastName, Email = @Email, 
                                 PhoneNumber = @PhoneNumber, Username = @Username, Password = @Password, 
                                 Role = @Role, JoinDate = @JoinDate
                             WHERE AdminID = @AdminId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", adminData.FirstName);
                    command.Parameters.AddWithValue("@LastName", adminData.LastName);
                    command.Parameters.AddWithValue("@Email", adminData.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", adminData.PhoneNumber);
                    command.Parameters.AddWithValue("@Username", adminData.Username);
                    command.Parameters.AddWithValue("@Password", HashPassword(adminData.Password));
                    command.Parameters.AddWithValue("@Role", adminData.Role);
                    command.Parameters.AddWithValue("@JoinDate", adminData.JoinDate);
                    command.Parameters.AddWithValue("@AdminId", adminData.AdminID);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        throw new AdminNotFoundException($"Admin with ID {adminData.AdminID} not found.");

                    }
                    else
                    {
                        Console.WriteLine("\t*Admin updated successfully.");
                        string query2 = @"UPDATE Logins SET Password = @Password WHERE UserName = @UserName";
                        SqlCommand command2 = new SqlCommand(query2, connection);
                        command2.Parameters.AddWithValue("@Password", adminData.Password);
                        command2.Parameters.AddWithValue("@UserName", adminData.Username);
                        command2.ExecuteNonQuery();
                    }
                }
            }
            catch (AdminNotFoundException ex) { Console.WriteLine("\t*" + ex.Message); }
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
