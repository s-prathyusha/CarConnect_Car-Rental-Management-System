using Car_Connect.util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.dao
{
    internal class ReportGenerator
    {
        public void GenerateReservationReport()
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    string query = "SELECT * FROM Reservation";
                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine($"\tReservationID : {reader["ReservationID"]},CustomerID : {reader["CustomerID"]},VehicleID : {reader["VehicleID"]}, StartDate : {reader["StartDate"]},EndDate : {reader["EndDate"]}, TotalCost : {reader["TotalCost"]}, Status : {reader["Status"]}");
                    }
                }

                Console.WriteLine("\t*Reservation report generated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\t*Error generating reservation report: {ex.Message}");
            }
        }

        public void GenerateVehicleReport()
        {
            try
            {
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    string query = "SELECT * FROM Vehicle";
                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Console.WriteLine($"\tVehicleID : {reader["VehicleID"]},Model : {reader["Model"]}, Make : {reader["Make"]}, Year : {reader["Year"]},Color : {reader["Color"]},RegistrationNumber : {reader["RegistrationNumber"]},Availability {reader["Availability"]},  DailyRate : {reader["DailyRate"]}");
                    }
                }

                Console.WriteLine("\t*Vehicle report generated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\t*Error generating vehicle report: {ex.Message}");
            }
        }
    }
}
