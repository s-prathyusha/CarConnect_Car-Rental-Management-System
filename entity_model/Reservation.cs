using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Car_Connect.util;

namespace Car_Connect.entity_model
{
    public class Reservation
    {
        private int reservationID;
        private int customerID;
        private int vehicleID;
        private DateTime startDate;
        private DateTime endDate;
        private decimal totalCost;
        private string status;

        public Reservation()
        {

        }

        public Reservation(int reservationID, int customerID, int vehicleID, DateTime startDate, DateTime endDate, decimal totalCost, string status)
        {
            this.reservationID = reservationID;
            this.customerID = customerID;
            this.vehicleID = vehicleID;
            this.startDate = startDate;
            this.endDate = endDate;
            this.totalCost = totalCost;
            this.status = status;
        }

        public int ReservationID
        {
            get { return reservationID; }
            set { reservationID = value; }
        }

        public int CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }

        public int VehicleID
        {
            get { return vehicleID; }
            set { vehicleID = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public decimal TotalCost
        {
            get { return totalCost; }
            set { totalCost = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public decimal CalculateTotalCost()
        {
            decimal dailyRate = 0;
            int numberOfDays = (int)(endDate - startDate).TotalDays;
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = "SELECT DailyRate FROM Vehicle WHERE VehicleID = @VehicleID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@VehicleID", VehicleID);

                    try
                    {
                        connection.Open();
                        var result = command.ExecuteScalar();
                        if (result != null)
                        {
                            dailyRate = Convert.ToDecimal(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\t*An error occurred: " + ex.Message);
                    }
                }
            }
            TotalCost = numberOfDays * dailyRate;
            return TotalCost;
        }
    }
}

