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
    internal class ReservationService : IReservationService
    {
        public void CancelReservation(int reservationId)
        {
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string q = "SELECT Status FROM Reservation WHERE ReservationID = @ReservationId";
                SqlCommand cmd = new SqlCommand(q, connection);
                cmd.Parameters.AddWithValue("@ReservationId", reservationId);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string status = reader.GetString(0);
                        if (status == "cancelled")
                        {
                            Console.WriteLine("\t*The Status of this Reservation is already Cancelled!");

                        }
                        else
                        {
                            reader.Close();
                            q = "UPDATE Reservation SET Status = 'cancelled' WHERE ReservationID = @ReservationId";
                            cmd= new SqlCommand(q, connection);
                            cmd.Parameters.AddWithValue("@ReservationId", reservationId);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                try { throw new ReservationException($"Reservation with ID {reservationId} not found."); }
                                catch (ReservationException ex) { Console.WriteLine("\t*" + ex.Message); }
                            }
                            else
                            {
                                Console.WriteLine("\t*Reservation cancelled successfully.");
                            }
                        }
                    }
                    else
                    {
                        try { throw new ReservationException($"Reservation with ID {reservationId} not found."); }
                        catch (ReservationException ex) { Console.WriteLine("\t*" + ex.Message); }
                    }

                }
                
            }
        }

        public void CreateReservation(Reservation reservationData)
        {
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string q = "SELECT Availability FROM Vehicle WHERE VehicleID = @VehicleId";
                SqlCommand cmd = new SqlCommand(q, connection);
                cmd.Parameters.AddWithValue("@VehicleId", reservationData.VehicleID);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bool availability = reader.GetBoolean(0);
                        connection.Close();
                        if (!availability)
                        {
                            Console.WriteLine($"\t*The Vehicle with vehicle ID {reservationData.VehicleID} is not available to reserve.It's already booked!");
                        }
                        else
                        {
                            string query = @"INSERT INTO Reservation (CustomerID, VehicleID, StartDate, EndDate, TotalCost, Status)
                             VALUES (@CustomerId, @VehicleId, @StartDate, @EndDate, @TotalCost, @Status)";
                            cmd= new SqlCommand(query, connection);

                            cmd.Parameters.AddWithValue("@CustomerId", reservationData.CustomerID);
                            cmd.Parameters.AddWithValue("@VehicleId", reservationData.VehicleID);
                            cmd.Parameters.AddWithValue("@StartDate", reservationData.StartDate);
                            cmd.Parameters.AddWithValue("@EndDate", reservationData.EndDate);
                            cmd.Parameters.AddWithValue("@TotalCost", reservationData.TotalCost);
                            cmd.Parameters.AddWithValue("@Status", reservationData.Status);
                            connection.Open();
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                try { throw new ReservationException($"Unable to Create!"); }
                                catch (ReservationException ex) { Console.WriteLine(ex.Message); }
                            }
                            connection.Close();
                            query = "UPDATE Vehicle SET Availability = 0 WHERE VehicleID = " + reservationData.VehicleID;
                            cmd = new SqlCommand(query, connection);
                            connection.Open();
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("\t*Reservation created Successfully!");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Vehicle with ID : {reservationData.VehicleID} not exists to reserve!");
                    }
                }
            }
        }

        public Reservation GetReservationById(int reservationId)
        {
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = "SELECT * FROM Reservation WHERE ReservationID = @ReservationId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ReservationId", reservationId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Reservation r = new Reservation
                    {
                        ReservationID = Convert.ToInt32(reader["ReservationID"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        VehicleID = Convert.ToInt32(reader["VehicleID"]),
                        StartDate = Convert.ToDateTime(reader["StartDate"]),
                        EndDate = Convert.ToDateTime(reader["EndDate"]),
                        TotalCost = Convert.ToDecimal(reader["TotalCost"]),
                        Status = reader["Status"].ToString()
                    };
                    return r;
                }
                try { throw new ReservationException($"Reservation with ID {reservationId} not found."); }
                catch (ReservationException ex) { Console.WriteLine(ex.Message); return null; }
            }
        }


        public List<Reservation> GetReservationsByCustomerId(int customerId)
        {
            List<Reservation> reservations = new List<Reservation>();
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = "SELECT * FROM Reservation WHERE CustomerID = @CustomerId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerId", customerId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Reservation r = new Reservation
                    {
                        ReservationID = Convert.ToInt32(reader["ReservationID"]),
                        CustomerID = Convert.ToInt32(reader["CustomerID"]),
                        VehicleID = Convert.ToInt32(reader["VehicleID"]),
                        StartDate = Convert.ToDateTime(reader["StartDate"]),
                        EndDate = Convert.ToDateTime(reader["EndDate"]),
                        TotalCost = Convert.ToDecimal(reader["TotalCost"]),
                        Status = reader["Status"].ToString()
                    };
                    reservations.Add(r);
                }
            }
            return reservations;
        }

        public void UpdateReservation(Reservation reservationData)
        {
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = @"UPDATE Reservation 
                             SET StartDate = @StartDate, EndDate = @EndDate, TotalCost = @TotalCost, Status = @Status
                             WHERE ReservationID = @ReservationId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@StartDate", reservationData.StartDate);
                command.Parameters.AddWithValue("@EndDate", reservationData.EndDate);
                command.Parameters.AddWithValue("@TotalCost", reservationData.TotalCost);
                command.Parameters.AddWithValue("@Status", reservationData.Status);
                command.Parameters.AddWithValue("@ReservationId", reservationData.ReservationID);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected == 0)
                {
                    try { throw new ReservationException($"Reservation with ID {reservationData.ReservationID} not found."); }
                    catch (ReservationException ex) { Console.WriteLine(ex.Message); }
                }
            }
        }
    }
}
