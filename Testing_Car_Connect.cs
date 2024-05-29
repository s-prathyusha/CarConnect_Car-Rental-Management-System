using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Car_Connect.dao;
using Car_Connect.entity_model;
using Car_Connect.util;

namespace Car_Connect_Testing_Project
{
    [TestFixture]
    public class TestingCarConnect
    {
        private ICustomerService customerService;
        private IVehicleService vehicleService;

        [Test]
        public void TestCustomerAuthentication()
        {

            string invalidUsername = "prathyusha_";
            string invalidPassword = "Prathyusha";

            AuthenticationService authenticationService = new AuthenticationService();
            bool isAuthenticated = authenticationService.AuthenticateCustomer(invalidUsername, invalidPassword);

            Assert.That(isAuthenticated, Is.EqualTo(false));
        }

        [Test]
        public void TestUpdateCustomer()
        {
            customerService = new CustomerService();
            Customer existingCustomer = new Customer
            {
                CustomerID = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@gmail.com",
                PhoneNumber = "9123456780",
                Address = "123 Main St, Springfield",
                Username = "johndoe",
                Password = "John@!6780",
                RegistrationDate = new DateTime(2023, 11, 17)
            };


            existingCustomer.FirstName = "Johnny";
            customerService.UpdateCustomer(existingCustomer);


            Customer updatedCustomer = customerService.GetCustomerById(existingCustomer.CustomerID);
            Assert.That("Johnny", Is.EqualTo(updatedCustomer.FirstName));
        }

        [Test]
        public void TestAddNewVehicle()
        {
            vehicleService = new VehicleService();
            Vehicle newVehicle = new Vehicle
            {
                Model = "Toyota Camry",
                Make = "Toyota",
                Year = 2022,
                Color = "Black",
                RegistrationNumber = "ABC123",
                Availability = 1,
                DailyRate = 500
            };

            vehicleService.AddVehicle(newVehicle);
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = "SELECT VehicleID FROM Vehicle WHERE RegistrationNumber = 'ABC123'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int ID = (int)command.ExecuteScalar();

                Vehicle addedVehicle = vehicleService.GetVehicleById(ID);
                Assert.That(addedVehicle, Is.Not.Null);
            }
            
        }

        [Test]
        public void TestUpdateVehicle()
        {
            vehicleService = new VehicleService();
            Vehicle existingVehicle = new Vehicle
            {
                VehicleID = 101,
                Model = "Civic",
                Make = "Honda",
                Year = 2022,
                Color = "Blue",
                RegistrationNumber = "MH123456",
                Availability = 1,
                DailyRate = 2500
            };


            existingVehicle.Color = "Red";
            vehicleService.UpdateVehicle(existingVehicle);

            Vehicle updatedVehicle = vehicleService.GetVehicleById(existingVehicle.VehicleID);
            Assert.That("Red", Is.EqualTo(updatedVehicle.Color));
            
        }

        [Test]
        public void TestGetAvailableVehicles()
        {

            List<Vehicle> availableVehicles = vehicleService.GetAvailableVehicles();
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = "SELECT Count(*) FROM Vehicle WHERE Availability = 1";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int count = (int)command.ExecuteScalar();

                Assert.That(availableVehicles.Count, Is.EqualTo(count));
            }
        }

        [Test]
        public void TestGetAllVehicles()
        {
            List<Vehicle> allVehicles = vehicleService.GetAllVehicles();
            using (SqlConnection connection = DatabaseContext.GetSqlConnection())
            {
                string query = "SELECT Count(*) FROM Vehicle";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int count = (int)command.ExecuteScalar();

                Assert.That(allVehicles.Count, Is.EqualTo(count));
            }
        }
    }
}

