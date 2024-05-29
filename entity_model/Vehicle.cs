using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.entity_model
{
    public class Vehicle
    {
        private int vehicleID;
        private string model;
        private string make;
        private int year;
        private string color;
        private string registrationNumber;
        private int availability;
        private decimal dailyRate;

        public Vehicle()
        {

        }

        public Vehicle(int vehicleID, string model, string make, int year, string color, string registrationNumber, int availability, decimal dailyRate)
        {
            this.vehicleID = vehicleID;
            this.model = model;
            this.make = make;
            this.year = year;
            this.color = color;
            this.registrationNumber = registrationNumber;
            this.availability = availability;
            this.dailyRate = dailyRate;
        }

        public int VehicleID
        {
            get { return vehicleID; }
            set { vehicleID = value; }
        }

        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        public string Make
        {
            get { return make; }
            set { make = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string RegistrationNumber
        {
            get { return registrationNumber; }
            set { registrationNumber = value; }
        }

        public int Availability
        {
            get { return availability; }
            set { availability = value; }
        }

        public decimal DailyRate
        {
            get { return dailyRate; }
            set { dailyRate = value; }
        }
    }
}
