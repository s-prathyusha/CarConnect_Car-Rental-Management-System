using Car_Connect.dao;
using Car_Connect.entity_model;
using Car_Connect.exception;
using Car_Connect.util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect
{
    public class MainModule
    {
        private static AuthenticationService authentication;
        private static CustomerService customer;
        private static AdminService admin;
        private static ReservationService reservation;
        private static VehicleService vehicle;
        private static ReportGenerator reportGenerator;
        public MainModule()
        {
            authentication = new AuthenticationService();
            customer = new CustomerService();
            admin = new AdminService();
            reservation = new ReservationService();
            vehicle = new VehicleService();
            reportGenerator = new ReportGenerator();
        }
        public static void Main()
        {
            MainModule m = new MainModule();
            bool exit = false;
            Console.WriteLine("\nWelcome to CarConnect - a Car Rental Platform!");
            while (!exit)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. New Registration");
                Console.WriteLine("2. Already Registered - Get Services!");
                Console.WriteLine("3. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        RegistrationMenu();
                        break;
                    case "2":
                        AuthenticationMenu();
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\t*Invalid choice. Please try again.");
                        break;
                }
            }
        }
        public static void RegistrationMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nRegistration Menu - Welcome New Comers!");
                Console.WriteLine("1. New Customer Registration");
                Console.WriteLine("2. New Admin Registration");
                Console.WriteLine("3. Go back to Main Menu");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.WriteLine("Register a new customer:");
                            Console.Write("Enter First Name: ");
                            string firstName = Console.ReadLine();
                            Console.Write("Enter Last Name: ");
                            string lastName = Console.ReadLine();
                            Console.Write("Enter Email: ");
                            string email = Console.ReadLine();
                            Console.Write("Enter Phone Number: ");
                            string phoneNumber = Console.ReadLine();
                            Console.Write("Enter Address: ");
                            string address = Console.ReadLine();
                            Console.Write("Enter Username: ");
                            string newUsername = Console.ReadLine();
                            Console.Write("Enter Password: ");
                            string newPassword = Console.ReadLine();
                            DateTime registrationDate = DateTime.Today;

                            Customer newCustomer = new Customer
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                Email = email,
                                PhoneNumber = phoneNumber,
                                Address = address,
                                Username = newUsername,
                                Password = newPassword,
                                RegistrationDate = registrationDate
                            };
                            customer.RegisterCustomer(newCustomer);
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        Console.WriteLine("\t*Customer Registration Successful!");
                        break;

                    case "2":
                        try
                        {
                            Console.WriteLine("Register a new admin:");
                            Console.Write("Enter First Name: ");
                            string firstName = Console.ReadLine();
                            Console.Write("Enter Last Name: ");
                            string lastName = Console.ReadLine();
                            Console.Write("Enter Email: ");
                            string email = Console.ReadLine();
                            Console.Write("Enter Phone Number: ");
                            string phoneNumber = Console.ReadLine();
                            Console.Write("Enter Username: ");
                            string newUsername = Console.ReadLine();
                            Console.Write("Enter Password: ");
                            string newPassword = Console.ReadLine();
                            Console.Write("Enter Role: ");
                            string role = Console.ReadLine();

                            DateTime joinDate = DateTime.Today;

                            var newAdmin = new Admin
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                Email = email,
                                PhoneNumber = phoneNumber,
                                Username = newUsername,
                                Password = newPassword,
                                Role = role,
                                JoinDate = joinDate
                            };

                            admin.RegisterAdmin(newAdmin);

                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }

                        break;
                    case "3":
                        Console.WriteLine("\t*Returning back....");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("\t*Invalid choice. Please try again.");
                        break;
                }
            }
        }
        public static void AuthenticationMenu()
        {
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nAuthentication Menu - Get Authenticated and Authorized!");
                Console.WriteLine("1. Customer Login");
                Console.WriteLine("2. Admin Login");
                Console.WriteLine("3. Go back to Main Menu");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.Write("Enter username: ");
                            string customerUsername = Console.ReadLine();
                            Console.Write("Enter password: ");
                            string customerPassword = Console.ReadLine();
                            bool customerAuthenticated = authentication.AuthenticateCustomer(customerUsername, customerPassword);
                            if (customerAuthenticated)
                            {
                                Console.WriteLine("\t*Customer authentication successful!\n\tEnjoy our Services!");
                                using (SqlConnection c = DatabaseContext.GetSqlConnection())
                                {
                                    string q = "SELECT CustomerID FROM Customer WHERE Username = @Username";
                                    SqlCommand sc = new SqlCommand(q, c);
                                    sc.Parameters.AddWithValue("@Username", customerUsername);
                                    c.Open();
                                    int ID = (int)sc.ExecuteScalar();
                                    CustomerServices(ID);
                                }
                            }

                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("\t*" + "There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;

                    case "2":
                        try
                        {
                            Console.Write("Enter username: ");
                            string adminUsername = Console.ReadLine();
                            Console.Write("Enter password: ");
                            string adminPassword = Console.ReadLine();
                            bool adminAuthenticated = authentication.AuthenticateAdmin(adminUsername, adminPassword);
                            if (adminAuthenticated)
                            {
                                Console.WriteLine("\t*Admin authentication successful!\n\tEnjoy our Services!");
                                AdminServices();
                            }

                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine(e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;

                    case "3":
                        Console.WriteLine("\t*Exiting...");
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("\t*Invalid choice. Please try again.");
                        break;
                }
            }
        }
        public static void AdminServices()
        {
            Console.WriteLine("\nWelcome to CarConnect - Admin Services!");
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nServices accessible by Admin:");
                Console.WriteLine("1. Manages Customer Data");
                Console.WriteLine("2. Manage Admin Data");
                Console.WriteLine("3. Manage Reservation Data");
                Console.WriteLine("4. Manage Vehicle Data");
                Console.WriteLine("5. Generate Reports");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CustomerMenu();
                        break;
                    case "2":
                        AdminMenu();
                        break;
                    case "3":
                        ReservationMenu();
                        break;
                    case "4":
                        VehicleMenu();
                        break;
                    case "5":
                        ReportsMenu();
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\t*Invalid choice. Please try again.");
                        break;
                }
            }
        }
        public static void CustomerMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nCustomer Services Menu");
                Console.WriteLine("1. Get Customer by ID");
                Console.WriteLine("2. Get Customer by Username");
                Console.WriteLine("3. Register new Customer");
                Console.WriteLine("4. Update Customer");
                Console.WriteLine("5. Delete Customer");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.Write("Enter Customer ID: ");
                            int customerId = Convert.ToInt32(Console.ReadLine());
                            Customer c = customer.GetCustomerById(customerId);
                            if (c != null)
                            {
                                Console.WriteLine("\t*Customer found:");
                                Console.WriteLine($"\tID: {c.CustomerID}");
                                Console.WriteLine($"\tName: {c.FirstName} {c.LastName}");
                                Console.WriteLine($"\tEmail: {c.Email}");
                                Console.WriteLine($"\tPhone: {c.PhoneNumber}");
                                Console.WriteLine($"\tAddress: {c.Address}");
                                Console.WriteLine($"\tUsername: {c.Username}");
                                Console.WriteLine($"\tRegistration Date: {c.RegistrationDate}");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "2":
                        try
                        {
                            Console.Write("Enter Customer Username: ");
                            string username = Console.ReadLine();
                            Customer customerByUsername = customer.GetCustomerByUsername(username);
                            if (customerByUsername != null)
                            {
                                Console.WriteLine("\t*Customer found:");
                                Console.WriteLine($"\tID: {customerByUsername.CustomerID}");
                                Console.WriteLine($"\tName: {customerByUsername.FirstName} {customerByUsername.LastName}");
                                Console.WriteLine($"\tEmail: {customerByUsername.Email}");
                                Console.WriteLine($"\tPhone: {customerByUsername.PhoneNumber}");
                                Console.WriteLine($"\tAddress: {customerByUsername.Address}");
                                Console.WriteLine($"\tUsername: {customerByUsername.Username}");
                                Console.WriteLine($"\tRegistration Date: {customerByUsername.RegistrationDate}");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "3":
                        try
                        {
                            Console.WriteLine("Register a new customer:");
                            Console.Write("Enter First Name: ");
                            string firstName = Console.ReadLine();
                            Console.Write("Enter Last Name: ");
                            string lastName = Console.ReadLine();
                            Console.Write("Enter Email: ");
                            string email = Console.ReadLine();
                            Console.Write("Enter Phone Number: ");
                            string phoneNumber = Console.ReadLine();
                            Console.Write("Enter Address: ");
                            string address = Console.ReadLine();
                            Console.Write("Enter Username: ");
                            string newUsername = Console.ReadLine();
                            Console.Write("Enter Password: ");
                            string newPassword = Console.ReadLine();
                            DateTime registrationDate = DateTime.Today;

                            Customer newCustomer = new Customer
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                Email = email,
                                PhoneNumber = phoneNumber,
                                Address = address,
                                Username = newUsername,
                                Password = newPassword,
                                RegistrationDate = registrationDate
                            };
                            customer.RegisterCustomer(newCustomer);
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        Console.WriteLine("\t*Customer Registration Successful!");
                        break;
                    case "4":
                        try
                        {
                            Console.WriteLine("Update Customer:");
                            Console.Write("Enter Customer ID: ");
                            int customerIdToUpdate = Convert.ToInt32(Console.ReadLine());
                            var existingCustomer = customer.GetCustomerById(customerIdToUpdate);
                            if (existingCustomer != null)
                            {
                                Console.WriteLine("Enter updated details:");
                                Console.Write("First Name: ");
                                string newfirstName = Console.ReadLine();
                                Console.Write("Last Name: ");
                                string newlastName = Console.ReadLine();
                                Console.Write("Email: ");
                                string newemail = Console.ReadLine();
                                Console.Write("Phone Number: ");
                                string newphoneNumber = Console.ReadLine();
                                Console.Write("Address: ");
                                string newaddress = Console.ReadLine();
                                Console.Write("New Password: ");
                                string Password = Console.ReadLine();


                                existingCustomer.FirstName = newfirstName;
                                existingCustomer.LastName = newlastName;
                                existingCustomer.Email = newemail;
                                existingCustomer.PhoneNumber = newphoneNumber;
                                existingCustomer.Address = newaddress;
                                existingCustomer.Password = Password;


                                customer.UpdateCustomer(existingCustomer);

                            }
                            else
                            {
                                Console.WriteLine("\t*Customer not found.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        Console.WriteLine("\t*Customer updated successfully.");
                        break;
                    case "5":
                        try
                        {
                            Console.Write("Enter Customer ID to delete: ");
                            int customerIdToDelete = Convert.ToInt32(Console.ReadLine());
                            customer.DeleteCustomer(customerIdToDelete);
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\t*Invalid choice. Please try again.");
                        break;
                }
            }
        }
        public static void AdminMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nAdmin Services Menu");
                Console.WriteLine("1. Get Admin by ID");
                Console.WriteLine("2. Get Admin by Username");
                Console.WriteLine("3. Register Admin");
                Console.WriteLine("4. Update Admin");
                Console.WriteLine("5. Delete Admin");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.Write("Enter Admin ID: ");
                            int adminId = Convert.ToInt32(Console.ReadLine());
                            var adminById = admin.GetAdminById(adminId);
                            if (adminById != null)
                            {
                                Console.WriteLine("\t*" + "Admin found:");
                                Console.WriteLine("\t" + $"ID: {adminById.AdminID}");
                                Console.WriteLine("\t" + $"Name: {adminById.FirstName} {adminById.LastName}");
                                Console.WriteLine("\t" + $"Email: {adminById.Email}");
                                Console.WriteLine("\t" + $"Phone: {adminById.PhoneNumber}");
                                Console.WriteLine("\t" + $"Username: {adminById.Username}");
                                Console.WriteLine("\t" + $"Role: {adminById.Role}");
                                Console.WriteLine("\t" + $"Join Date: {adminById.JoinDate}");
                            }
                            else
                            {
                                Console.WriteLine("\t*Admin not found.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\t*" + ex.Message);
                        }
                        break;
                    case "2":
                        try
                        {
                            Console.Write("Enter Admin Username: ");
                            string username = Console.ReadLine();
                            var adminByUsername = admin.GetAdminByUsername(username);
                            if (adminByUsername != null)
                            {
                                Console.WriteLine("\t*Admin found:");
                                Console.WriteLine($"\tID: {adminByUsername.AdminID}");
                                Console.WriteLine($"\tName: {adminByUsername.FirstName} {adminByUsername.LastName}");
                                Console.WriteLine($"\tEmail: {adminByUsername.Email}");
                                Console.WriteLine($"\tPhone: {adminByUsername.PhoneNumber}");
                                Console.WriteLine($"\tRole: {adminByUsername.Role}");
                                Console.WriteLine($"\tJoin Date: {adminByUsername.JoinDate}");
                            }
                            else
                            {
                                Console.WriteLine("\t*Admin not found.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "3":
                        try
                        {
                            Console.WriteLine("Register a new admin:");
                            Console.Write("Enter First Name: ");
                            string firstName = Console.ReadLine();
                            Console.Write("Enter Last Name: ");
                            string lastName = Console.ReadLine();
                            Console.Write("Enter Email: ");
                            string email = Console.ReadLine();
                            Console.Write("Enter Phone Number: ");
                            string phoneNumber = Console.ReadLine();
                            Console.Write("Enter Username: ");
                            string newUsername = Console.ReadLine();
                            Console.Write("Enter Password: ");
                            string newPassword = Console.ReadLine();
                            Console.Write("Enter Role: ");
                            string role = Console.ReadLine();

                            DateTime joinDate = DateTime.Today;

                            var newAdmin = new Admin
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                Email = email,
                                PhoneNumber = phoneNumber,
                                Username = newUsername,
                                Password = newPassword,
                                Role = role,
                                JoinDate = joinDate
                            };

                            admin.RegisterAdmin(newAdmin);

                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }

                        break;
                    case "4":
                        try
                        {
                            Console.WriteLine("Update Admin:");
                            Console.Write("Enter Admin ID: ");
                            int adminIdToUpdate = Convert.ToInt32(Console.ReadLine());
                            var existingAdmin = admin.GetAdminById(adminIdToUpdate);
                            if (existingAdmin != null)
                            {
                                Console.WriteLine("Enter updated details:");
                                Console.Write("First Name: ");
                                string newfirstName = Console.ReadLine();
                                Console.Write("Last Name: ");
                                string newlastName = Console.ReadLine();
                                Console.Write("Email: ");
                                string newemail = Console.ReadLine();
                                Console.Write("Phone Number: ");
                                string newphoneNumber = Console.ReadLine();
                                Console.Write("Username: ");
                                string newusername = Console.ReadLine();
                                Console.Write("New Password: ");
                                string Password = Console.ReadLine();
                                Console.Write("Role: ");
                                string newrole = Console.ReadLine();


                                existingAdmin.FirstName = newfirstName;
                                existingAdmin.LastName = newlastName;
                                existingAdmin.Email = newemail;
                                existingAdmin.PhoneNumber = newphoneNumber;
                                existingAdmin.Username = newusername;
                                existingAdmin.Password = Password;
                                existingAdmin.Role = newrole;


                                admin.UpdateAdmin(existingAdmin);

                            }
                            else
                            {
                                Console.WriteLine("\t*Admin not found.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }

                        break;

                    case "5":
                        try
                        {
                            Console.Write("Enter Admin ID to delete: ");
                            int adminIdToDelete = Convert.ToInt32(Console.ReadLine());
                            admin.DeleteAdmin(adminIdToDelete);

                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\t*Invalid choice. Please try again.");
                        break;
                }
            }
        }
        private static void ReservationMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nReservation Services Menu");
                Console.WriteLine("1. Get Reservation by ID");
                Console.WriteLine("2. Get Reservations by Customer ID");
                Console.WriteLine("3. Create Reservation");
                Console.WriteLine("4. Update Reservation");
                Console.WriteLine("5. Cancel Reservation");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.Write("Enter Reservation ID: ");
                            int reservationId = Convert.ToInt32(Console.ReadLine());
                            var reservationById = reservation.GetReservationById(reservationId);
                            if (reservationById != null)
                            {
                                Console.WriteLine("\t*Reservation found:");
                                Console.WriteLine($"\tID: {reservationById.ReservationID}");
                                Console.WriteLine($"\tCustomer ID: {reservationById.CustomerID}");
                                Console.WriteLine($"\tVehicle ID: {reservationById.VehicleID}");
                                Console.WriteLine($"\tStart Date: {reservationById.StartDate}");
                                Console.WriteLine($"\tEnd Date: {reservationById.EndDate}");
                                Console.WriteLine($"\tTotal Cost: {reservationById.TotalCost}");
                                Console.WriteLine($"\tStatus: {reservationById.Status}");
                            }
                            else
                            {
                                Console.WriteLine("\t*Reservation not found.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "2":
                        try
                        {
                            Console.Write("Enter Customer ID: ");
                            int customerId = Convert.ToInt32(Console.ReadLine());
                            var reservationsByCustomerId = reservation.GetReservationsByCustomerId(customerId);
                            if (reservationsByCustomerId.Any())
                            {
                                Console.WriteLine($"\t*Reservations for Customer ID {customerId}:");
                                foreach (var reservation in reservationsByCustomerId)
                                {
                                    Console.WriteLine("\n\t*Reservation found:");
                                    Console.WriteLine($"\tID: {reservation.ReservationID}");
                                    Console.WriteLine($"\tCustomer ID: {reservation.CustomerID}");
                                    Console.WriteLine($"\tVehicle ID: {reservation.VehicleID}");
                                    Console.WriteLine($"\tStart Date: {reservation.StartDate}");
                                    Console.WriteLine($"\tEnd Date: {reservation.EndDate}");
                                    Console.WriteLine($"\tTotal Cost: {reservation.TotalCost}");
                                    Console.WriteLine($"\tStatus: {reservation.Status}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\t*No reservations found for the given customer ID.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "3":
                        try
                        {
                            Console.WriteLine("Create a new reservation:");
                            Console.Write("Enter Customer ID: ");
                            int customerID = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter Vehicle ID: ");
                            int vehicleId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter Start Date and Time (YYYY-MM-DD HH:MM): ");
                            DateTime startDate = Convert.ToDateTime(Console.ReadLine());
                            Console.Write("Enter End Date and Time (YYYY-MM-DD HH:MM): ");
                            DateTime endDate = Convert.ToDateTime(Console.ReadLine());
                            if (endDate <= startDate)
                            {
                                throw new InvalidInputException("Invalid Input : End Date Should be greater than the Start Date! ");
                            }
                            string status = "pending";

                            Reservation newReservation = new Reservation
                            {
                                CustomerID = customerID,
                                VehicleID = vehicleId,
                                StartDate = startDate,
                                EndDate = endDate,
                                Status = status
                            };
                            newReservation.TotalCost = newReservation.CalculateTotalCost();
                            reservation.CreateReservation(newReservation);
                        }
                        catch (InvalidInputException e)
                        {
                            Console.WriteLine("\t*" + e.Message); 
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message+ex.StackTrace); }
                        break;

                    case "4":
                        try
                        {
                            Console.WriteLine("Update an existing reservation:");
                            Console.Write("Enter Reservation ID: ");
                            int reservationIdToUpdate = Convert.ToInt32(Console.ReadLine());
                            var existingReservation = reservation.GetReservationById(reservationIdToUpdate);
                            if (existingReservation != null)
                            {
                                Console.WriteLine("Enter updated details:");
                                Console.Write("Start Date and Time (YYYY-MM-DD HH:MM): ");
                                DateTime updatedStartDate = Convert.ToDateTime(Console.ReadLine());
                                Console.Write("End Date and Time (YYYY-MM-DD HH:MM): ");
                                DateTime updatedEndDate = Convert.ToDateTime(Console.ReadLine());
                                if (updatedEndDate <= updatedStartDate)
                                {
                                    throw new InvalidInputException("Invalid Input : End Date Should be greater than the Start Date! ");
                                }
                                string updatedStatus = "pending";


                                existingReservation.StartDate = updatedStartDate;
                                existingReservation.EndDate = updatedEndDate;
                                existingReservation.TotalCost = existingReservation.CalculateTotalCost();
                                existingReservation.Status = updatedStatus;

                                reservation.UpdateReservation(existingReservation);
                                Console.WriteLine("\t*Reservation updated successfully.");
                            }
                            else
                            {
                                Console.WriteLine("\t*Reservation not found.");
                            }
                        }
                        catch (InvalidInputException e)
                        {
                            Console.WriteLine("\t*" + e.Message);
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;

                    case "5":
                        try
                        {
                            Console.Write("Enter Reservation ID to cancel: ");
                            int reservationIdToCancel = Convert.ToInt32(Console.ReadLine());
                            reservation.CancelReservation(reservationIdToCancel);

                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\t*Invalid choice. Please try again.");
                        break;
                }
            }
        }
        private static void VehicleMenu()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nVehicle Services Menu");
                Console.WriteLine("1. Get Vehicle by ID");
                Console.WriteLine("2. Get Available Vehicles");
                Console.WriteLine("3. Add Vehicle");
                Console.WriteLine("4. Update Vehicle");
                Console.WriteLine("5. Remove Vehicle");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.Write("Enter Vehicle ID: ");
                            int vehicleId = Convert.ToInt32(Console.ReadLine());
                            var vehicleById = vehicle.GetVehicleById(vehicleId);
                            if (vehicleById != null)
                            {
                                Console.WriteLine("\t*Vehicle found:");
                                Console.WriteLine($"\tID: {vehicleById.VehicleID}");
                                Console.WriteLine($"\tModel: {vehicleById.Model}");
                                Console.WriteLine($"\tMake: {vehicleById.Make}");
                                Console.WriteLine($"\tYear: {vehicleById.Year}");
                                Console.WriteLine($"\tColor: {vehicleById.Color}");
                                Console.WriteLine($"\tRegistration Number: {vehicleById.RegistrationNumber}");
                                Console.WriteLine($"\tAvailability: {vehicleById.Availability}");
                                Console.WriteLine($"\tDaily Rate: {vehicleById.DailyRate}");
                            }
                            else
                            {
                                Console.WriteLine("\t*Vehicle not found.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "2":
                        try
                        {
                            var availableVehicles = vehicle.GetAvailableVehicles();
                            if (availableVehicles.Any())
                            {
                                Console.WriteLine("\t*Available Vehicles:");
                                foreach (var vehicle in availableVehicles)
                                {
                                    Console.WriteLine("\n\t*Vehicle found:");
                                    Console.WriteLine($"\tID: {vehicle.VehicleID}");
                                    Console.WriteLine($"\tModel: {vehicle.Model}");
                                    Console.WriteLine($"\tMake: {vehicle.Make}");
                                    Console.WriteLine($"\tYear: {vehicle.Year}");
                                    Console.WriteLine($"\tColor: {vehicle.Color}");
                                    Console.WriteLine($"\tRegistration Number: {vehicle.RegistrationNumber}");
                                    Console.WriteLine($"\tAvailability: {vehicle.Availability}");
                                    Console.WriteLine($"\tDaily Rate: {vehicle.DailyRate}");

                                }
                            }
                            else
                            {
                                Console.WriteLine("\t*No available vehicles found.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "3":
                        try
                        {
                            Console.WriteLine("Add a new vehicle:");
                            Console.Write("Enter Model: ");
                            string model = Console.ReadLine();
                            Console.Write("Enter Make: ");
                            string make = Console.ReadLine();
                            Console.Write("Enter Year: ");
                            int year = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter Color: ");
                            string color = Console.ReadLine();
                            Console.Write("Enter Registration Number: ");
                            string registrationNumber = Console.ReadLine();
                            Console.Write("Enter Availability (0/1): ");
                            int availability = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter Daily Rate: ");
                            decimal dailyRate = Convert.ToDecimal(Console.ReadLine());

                            Vehicle newVehicle = new Vehicle
                            {
                                Model = model,
                                Make = make,
                                Year = year,
                                Color = color,
                                RegistrationNumber = registrationNumber,
                                Availability = availability,
                                DailyRate = dailyRate
                            };


                            vehicle.AddVehicle(newVehicle);
                            Console.WriteLine("\t*Vehicle added successfully.");
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;

                    case "4":
                        try
                        {
                            Console.WriteLine("Update an existing vehicle:");
                            Console.Write("Enter Vehicle ID: ");
                            int vehicleIdToUpdate = Convert.ToInt32(Console.ReadLine());
                            var existingVehicle = vehicle.GetVehicleById(vehicleIdToUpdate);
                            if (existingVehicle != null)
                            {
                                Console.WriteLine("Enter updated details:");
                                Console.Write("Model: ");
                                string updatedModel = Console.ReadLine();
                                Console.Write("Make: ");
                                string updatedMake = Console.ReadLine();
                                Console.Write("Year: ");
                                int updatedYear = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Color: ");
                                string updatedColor = Console.ReadLine();
                                Console.Write("Registration Number: ");
                                string updatedRegistrationNumber = Console.ReadLine();
                                Console.Write("Availability (0/1): ");
                                int updatedAvailability = Convert.ToInt32(Console.ReadLine());
                                Console.Write("Daily Rate: ");
                                decimal updatedDailyRate = Convert.ToDecimal(Console.ReadLine());

                                existingVehicle.Model = updatedModel;
                                existingVehicle.Make = updatedMake;
                                existingVehicle.Year = updatedYear;
                                existingVehicle.Color = updatedColor;
                                existingVehicle.RegistrationNumber = updatedRegistrationNumber;
                                existingVehicle.Availability = updatedAvailability;
                                existingVehicle.DailyRate = updatedDailyRate;

                                vehicle.UpdateVehicle(existingVehicle);

                            }
                            else
                            {
                                Console.WriteLine("\t*Vehicle not found.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;

                    case "5":
                        try
                        {
                            Console.Write("Enter Vehicle ID to remove: ");
                            int vehicleIdToRemove = Convert.ToInt32(Console.ReadLine());
                            vehicle.RemoveVehicle(vehicleIdToRemove);

                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\t*Invalid choice. Please try again.");
                        break;
                }
            }
        }
        private static void ReportsMenu()
        {
            bool reportsMenuActive = true;
            while (reportsMenuActive)
            {
                Console.WriteLine("\nReports Menu:");
                Console.WriteLine("1. Generate Reservation Report");
                Console.WriteLine("2. Generate Vehicle Report");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.WriteLine("\t*Generating Reservation Report...");
                            reportGenerator.GenerateReservationReport();
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;

                    case "2":
                        try
                        {
                            Console.WriteLine("\t*Generating Vehicle Report...");
                            reportGenerator.GenerateVehicleReport();
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;

                    case "3":
                        reportsMenuActive = false;
                        break;

                    default:
                        Console.WriteLine("\t*Invalid option. Please try again.");
                        break;
                }
            }
        }
        public static void CustomerServices(int ID)
        {
            Console.WriteLine("\nWelcome to CarConnect - Customer Services!");
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\n\nServices accessible by Customer:");
                Console.WriteLine("1. Manage your Account");
                Console.WriteLine("2. Get Vehicle Info");
                Console.WriteLine("3. Reservation Services");
                Console.WriteLine("4. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageAccount(ID);
                        break;
                    case "2":
                        VehicleInfo();
                        break;
                    case "3":
                        ReservationServices(ID);
                        break;
                    case "4":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\t*Invalid choice. Please try again.");
                        break;
                }
            }
        }
        public static void ManageAccount(int ID)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nManage Your Account:");
                Console.WriteLine("1. Update your Details");
                Console.WriteLine("2. Delete your Account");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.WriteLine("Update Customer:");
                            int customerIdToUpdate =ID;
                            var existingCustomer = customer.GetCustomerById(customerIdToUpdate);
                            if (existingCustomer != null)
                            {
                                Console.WriteLine("Enter updated details:");
                                Console.Write("First Name: ");
                                string newfirstName = Console.ReadLine();
                                Console.Write("Last Name: ");
                                string newlastName = Console.ReadLine();
                                Console.Write("Email: ");
                                string newemail = Console.ReadLine();
                                Console.Write("Phone Number: ");
                                string newphoneNumber = Console.ReadLine();
                                Console.Write("Address: ");
                                string newaddress = Console.ReadLine();
                                Console.Write("New Password: ");
                                string Password = Console.ReadLine();


                                existingCustomer.FirstName = newfirstName;
                                existingCustomer.LastName = newlastName;
                                existingCustomer.Email = newemail;
                                existingCustomer.PhoneNumber = newphoneNumber;
                                existingCustomer.Address = newaddress;
                                existingCustomer.Password = Password;


                                customer.UpdateCustomer(existingCustomer);
                                Console.WriteLine("\t*Customer updated successfully.");

                            }
                            else
                            {
                                Console.WriteLine("\t*Customer not found.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }

                        break;
                    case "2":
                        try
                        {
                            Console.Write("\t*Account Deletion:\n");
                            customer.DeleteCustomer(ID);
                            Console.WriteLine("\t*Your account has been deactivated. Best wishes!");
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine(ex.Message); }
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\t*Invalid choice. Please try again.");
                        break;
                }
            }
        }
        private static void ReservationServices(int ID)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nReservation Services Menu");
                Console.WriteLine("1. Get Reservation by Reservation ID");
                Console.WriteLine("2. Get your Reservations");
                Console.WriteLine("3. Create New Reservation");
                Console.WriteLine("4. Update Reservation");
                Console.WriteLine("5. Cancel Reservation");
                Console.WriteLine("6. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                List<int> reservationIds = new List<int>();
                using (SqlConnection connection = DatabaseContext.GetSqlConnection())
                {
                    string query = "SELECT ReservationID FROM Reservation WHERE CustomerID = @CustomerID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", ID);

                        try
                        {
                            connection.Open();
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    reservationIds.Add(reader.GetInt32(0)); // Assuming ReservationID is an int
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("\t*An error occurred: " + ex.Message);
                        }
                    }
                }
                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.Write("Enter Reservation ID: ");
                            int reservationId = Convert.ToInt32(Console.ReadLine());
                            if (!reservationIds.Contains(reservationId))
                            {
                                Console.WriteLine($"\t*Reservation with ID {reservationId} does not belong to you! - Access Denied");
                            }
                            else
                            {
                                var reservationById = reservation.GetReservationById(reservationId);
                                if (reservationById != null)
                                {
                                    Console.WriteLine("\t*Reservation found:");
                                    Console.WriteLine($"\tID: {reservationById.ReservationID}");
                                    Console.WriteLine($"\tCustomer ID: {reservationById.CustomerID}");
                                    Console.WriteLine($"\tVehicle ID: {reservationById.VehicleID}");
                                    Console.WriteLine($"\tStart Date: {reservationById.StartDate}");
                                    Console.WriteLine($"\tEnd Date: {reservationById.EndDate}");
                                    Console.WriteLine($"\tTotal Cost: {reservationById.TotalCost}");
                                    Console.WriteLine($"\tStatus: {reservationById.Status}");
                                }
                                else
                                {
                                    Console.WriteLine("\t*Reservation not found.");
                                }
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "2":
                        try
                        {
                            int customerId =ID;
                            var reservationsByCustomerId = reservation.GetReservationsByCustomerId(customerId);
                            if (reservationsByCustomerId.Any())
                            {
                                Console.WriteLine($"\t*Reservations for Customer ID {customerId}:");
                                foreach (var reservation in reservationsByCustomerId)
                                {
                                    Console.WriteLine("\n\t*Reservation found:");
                                    Console.WriteLine($"\tID: {reservation.ReservationID}");
                                    Console.WriteLine($"\tCustomer ID: {reservation.CustomerID}");
                                    Console.WriteLine($"\tVehicle ID: {reservation.VehicleID}");
                                    Console.WriteLine($"\tStart Date: {reservation.StartDate}");
                                    Console.WriteLine($"\tEnd Date: {reservation.EndDate}");
                                    Console.WriteLine($"\tTotal Cost: {reservation.TotalCost}");
                                    Console.WriteLine($"\tStatus: {reservation.Status}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\t*No reservations found for the given customer ID.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "3":
                        try
                        {
                            Console.WriteLine("Create a new reservation:");
                            int customerID = ID;
                            Console.Write("Enter Vehicle ID: ");
                            int vehicleId = Convert.ToInt32(Console.ReadLine());
                            Console.Write("Enter Start Date and Time (YYYY-MM-DD HH:MM): ");
                            DateTime startDate = Convert.ToDateTime(Console.ReadLine());
                            //if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out startDate))
                            //{
                            //    throw new FormatException("Invalid format for Start Date and Time. Please use YYYY-MM-DD HH:MM.");
                            //}
                            Console.Write("Enter End Date and Time (YYYY-MM-DD HH:MM): ");
                            DateTime endDate = Convert.ToDateTime(Console.ReadLine());
                            //if (!DateTime.TryParseExact(Console.ReadLine(), "yyyy-MM-dd HH:mm", null, System.Globalization.DateTimeStyles.None, out endDate))
                            //{
                            //    throw new FormatException("Invalid format for End Date and Time. Please use YYYY-MM-DD HH:MM.");
                            //}
                            string status = "pending";
                            if (endDate <= startDate)
                            {
                                Console.WriteLine($"{endDate.ToString()}, {startDate.ToString()}");
                                throw new InvalidInputException("Invalid Input : End Date Should be greater than the Start Date! ");
                            }
                            else
                            {
                                Reservation newReservation = new Reservation
                                {
                                    CustomerID = customerID,
                                    VehicleID = vehicleId,
                                    StartDate = startDate,
                                    EndDate = endDate,
                                    Status = status
                                };
                                newReservation.TotalCost = newReservation.CalculateTotalCost();
                                reservation.CreateReservation(newReservation);
                            }
                        }
                        catch (InvalidInputException e)
                        {
                            Console.WriteLine("\t*" + e.Message);
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;

                    case "4":
                        try
                        {
                            Console.WriteLine("Update an existing reservation:");
                            Console.Write("Enter Reservation ID: ");
                            int reservationIdToUpdate = Convert.ToInt32(Console.ReadLine());
                            if (!reservationIds.Contains(reservationIdToUpdate))
                            {
                                Console.WriteLine($"\t*Reservation with ID {reservationIdToUpdate} does not belong to you! - Access Denied");
                            }
                            else
                            {
                                var existingReservation = reservation.GetReservationById(reservationIdToUpdate);
                                if (existingReservation != null)
                                {
                                    Console.WriteLine("Enter updated details:");
                                    Console.Write("Start Date and Time (YYYY-MM-DD HH:MM): ");
                                    DateTime updatedStartDate = Convert.ToDateTime(Console.ReadLine());
                                    Console.Write("End Date and Time (YYYY-MM-DD HH:MM): ");
                                    DateTime updatedEndDate = Convert.ToDateTime(Console.ReadLine());
                                    string updatedStatus = "pending";
                                    if (updatedEndDate <= updatedStartDate)
                                    {
                                        throw new InvalidInputException("Invalid Input : End Date Should be greater than the Start Date! ");
                                    }

                                    else
                                    {
                                        existingReservation.StartDate = updatedStartDate;
                                        existingReservation.EndDate = updatedEndDate;
                                        existingReservation.TotalCost = existingReservation.CalculateTotalCost();
                                        existingReservation.Status = updatedStatus;

                                        reservation.UpdateReservation(existingReservation);
                                        Console.WriteLine("\t*Reservation updated successfully.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("\t*Reservation not found.");
                                }
                            }
                        }
                        catch (InvalidInputException e)
                        {
                            Console.WriteLine("\t*" + e.Message);
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;

                    case "5":
                        try
                        {
                            Console.Write("Enter Reservation ID to cancel: ");
                            int reservationIdToCancel = Convert.ToInt32(Console.ReadLine());
                            if (!reservationIds.Contains(reservationIdToCancel))
                            {
                                Console.WriteLine($"\t*Reservation with ID {reservationIdToCancel} does not belong to you! - Access Denied");
                            }
                            else
                                reservation.CancelReservation(reservationIdToCancel);

                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "6":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\t*Invalid choice. Please try again.");
                        break;
                }
            }
        }
        private static void VehicleInfo()
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nVehicle Services Menu");
                Console.WriteLine("1. Get Vehicle by ID");
                Console.WriteLine("2. Get Available Vehicles");
                Console.WriteLine("3. Back to Main Menu");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        try
                        {
                            Console.Write("Enter Vehicle ID: ");
                            int vehicleId = Convert.ToInt32(Console.ReadLine());
                            var vehicleById = vehicle.GetVehicleById(vehicleId);
                            if (vehicleById != null)
                            {
                                Console.WriteLine("\t*Vehicle found:");
                                Console.WriteLine($"\tID: {vehicleById.VehicleID}");
                                Console.WriteLine($"\tModel: {vehicleById.Model}");
                                Console.WriteLine($"\tMake: {vehicleById.Make}");
                                Console.WriteLine($"\tYear: {vehicleById.Year}");
                                Console.WriteLine($"\tColor: {vehicleById.Color}");
                                Console.WriteLine($"\tRegistration Number: {vehicleById.RegistrationNumber}");
                                Console.WriteLine($"\tAvailability: {vehicleById.Availability}");
                                Console.WriteLine($"\tDaily Rate: {vehicleById.DailyRate}");
                            }
                            else
                            {
                                Console.WriteLine("\t*Vehicle not found.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "2":
                        try
                        {
                            var availableVehicles = vehicle.GetAvailableVehicles();
                            if (availableVehicles.Any())
                            {
                                Console.WriteLine("\t*Available Vehicles:");
                                foreach (var vehicle in availableVehicles)
                                {
                                    Console.WriteLine("\n\t*Vehicle found:");
                                    Console.WriteLine($"\tID: {vehicle.VehicleID}");
                                    Console.WriteLine($"\tModel: {vehicle.Model}");
                                    Console.WriteLine($"\tMake: {vehicle.Make}");
                                    Console.WriteLine($"\tYear: {vehicle.Year}");
                                    Console.WriteLine($"\tColor: {vehicle.Color}");
                                    Console.WriteLine($"\tRegistration Number: {vehicle.RegistrationNumber}");
                                    Console.WriteLine($"\tAvailability: {vehicle.Availability}");
                                    Console.WriteLine($"\tDaily Rate: {vehicle.DailyRate}");

                                }
                            }
                            else
                            {
                                Console.WriteLine("\t*No available vehicles found.");
                            }
                        }
                        catch (FormatException fe)
                        {
                            try { throw new InvalidInputException("There is a mismatch in the Data types of the Input :" + fe.Message); }
                            catch (InvalidInputException e) { Console.WriteLine("\t*" + e.Message); }
                        }
                        catch (Exception ex) { Console.WriteLine("\t*" + ex.Message); }
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("\t*Invalid choice. Please try again.");
                        break;
                }
            }
        }
    }
}