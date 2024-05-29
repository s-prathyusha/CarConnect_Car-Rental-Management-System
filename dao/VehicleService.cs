using Car_Connect.entity_model;
using Car_Connect.exception;
using Car_Connect.util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.dao
{
    public class VehicleService : IVehicleService
    {
        public void AddVehicle(Vehicle vehicleData)
        {
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = @"INSERT INTO Vehicle (Model, Make, Year, Color, RegistrationNumber, Availability, DailyRate)
                             VALUES (@Model, @Make, @Year, @Color, @RegistrationNumber, @Availability, @DailyRate)";
                SqlCommand command = new SqlCommand(query, connection);
                
                command.Parameters.AddWithValue("@Model", vehicleData.Model);
                command.Parameters.AddWithValue("@Make", vehicleData.Make);
                command.Parameters.AddWithValue("@Year", vehicleData.Year);
                command.Parameters.AddWithValue("@Color", vehicleData.Color);
                command.Parameters.AddWithValue("@RegistrationNumber", vehicleData.RegistrationNumber);
                command.Parameters.AddWithValue("@Availability", vehicleData.Availability);
                command.Parameters.AddWithValue("@DailyRate", vehicleData.DailyRate);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<Vehicle> GetAvailableVehicles()
        {
            List<Vehicle> availableVehicles = new List<Vehicle>();
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = "SELECT * FROM Vehicle WHERE Availability = 1";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Vehicle vehicle = new Vehicle
                    {
                        VehicleID = Convert.ToInt32(reader["VehicleID"]),
                        Model = reader["Model"].ToString(),
                        Make = reader["Make"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        Color = reader["Color"].ToString(),
                        RegistrationNumber = reader["RegistrationNumber"].ToString(),
                        Availability = Convert.ToInt32(reader["Availability"]),
                        DailyRate = Convert.ToDecimal(reader["DailyRate"])
                    };
                    availableVehicles.Add(vehicle);
                }
            }
            return availableVehicles;
        }

        public List<Vehicle> GetAllVehicles()
        {
            List<Vehicle> availableVehicles = new List<Vehicle>();
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = "SELECT * FROM Vehicle";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Vehicle vehicle = new Vehicle
                    {
                        VehicleID = Convert.ToInt32(reader["VehicleID"]),
                        Model = reader["Model"].ToString(),
                        Make = reader["Make"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        Color = reader["Color"].ToString(),
                        RegistrationNumber = reader["RegistrationNumber"].ToString(),
                        Availability = Convert.ToInt32(reader["Availability"]),
                        DailyRate = Convert.ToDecimal(reader["DailyRate"])
                    };
                    availableVehicles.Add(vehicle);
                }
            }
            return availableVehicles;
        }
        public void RemoveVehicle(int vehicleId)
        {
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = "DELETE FROM Vehicle WHERE VehicleID = @VehicleId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@VehicleId", vehicleId);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    try { throw new VehicleNotFoundException($"Vehicle with ID {vehicleId} not found."); }
                    catch (VehicleNotFoundException ex) { Console.WriteLine("\t*" + ex.Message); }
                }
                else
                {
                    Console.WriteLine("\t*Vehicle removed successfully.");
                }
            }
        }

        public void UpdateVehicle(Vehicle vehicleData)
        {
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = @"UPDATE Vehicle 
                             SET Model = @Model, Make = @Make, Year = @Year, Color = @Color, 
                                 RegistrationNumber = @RegistrationNumber, Availability = @Availability, DailyRate = @DailyRate
                             WHERE VehicleID = @VehicleId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Model", vehicleData.Model);
                command.Parameters.AddWithValue("@Make", vehicleData.Make);
                command.Parameters.AddWithValue("@Year", vehicleData.Year);
                command.Parameters.AddWithValue("@Color", vehicleData.Color);
                command.Parameters.AddWithValue("@RegistrationNumber", vehicleData.RegistrationNumber);
                command.Parameters.AddWithValue("@Availability", vehicleData.Availability);
                command.Parameters.AddWithValue("@DailyRate", vehicleData.DailyRate);
                command.Parameters.AddWithValue("@VehicleId", vehicleData.VehicleID);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    try { throw new VehicleNotFoundException($"Vehicle with ID {vehicleData.VehicleID} not found."); }
                    catch (VehicleNotFoundException ex) { Console.WriteLine(ex.Message); }
                }
                else
                {
                    Console.WriteLine("\t*Vehicle updated successfully.");
                }
            }
        }

        public Vehicle GetVehicleById(int vehicleId)
        {
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = "SELECT * FROM Vehicle WHERE VehicleID = @VehicleId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@VehicleId", vehicleId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Vehicle vehicle = new Vehicle
                    {
                        VehicleID = Convert.ToInt32(reader["VehicleID"]),
                        Model = reader["Model"].ToString(),
                        Make = reader["Make"].ToString(),
                        Year = Convert.ToInt32(reader["Year"]),
                        Color = reader["Color"].ToString(),
                        RegistrationNumber = reader["RegistrationNumber"].ToString(),
                        Availability = Convert.ToInt32(reader["Availability"]),
                        DailyRate = Convert.ToDecimal(reader["DailyRate"])
                    };
                    return vehicle;
                }
                try { throw new VehicleNotFoundException($"Vehicle with ID {vehicleId} not found."); }
                catch (VehicleNotFoundException ex) { Console.WriteLine(ex.Message); return null; }
            }
        }
    }
}