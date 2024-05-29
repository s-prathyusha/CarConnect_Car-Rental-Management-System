--CaseStudy - CarConnect, a Car Rental Platform

CREATE DATABASE Car_Connect;

USE Car_Connect;

CREATE TABLE Customer (
	CustomerID INT PRIMARY KEY IDENTITY(1,1),
	FirstName VARCHAR(50),
	LastName VARCHAR(30),
	Email VARCHAR(50) UNIQUE,
	PhoneNumber VARCHAR(20) UNIQUE,
	Address VARCHAR(255),
	Username VARCHAR(30) UNIQUE,
	Password VARCHAR(64),
	RegistrationDate DATE
);

CREATE TABLE Vehicle(
	VehicleID INT PRIMARY KEY IDENTITY(101,1),
	Model VARCHAR(30),
	Make VARCHAR(30),
	Year INT,
	Color VARCHAR(20),
	RegistrationNumber VARCHAR(20) UNIQUE, 
	Availability BIT,
	DailyRate DECIMAL(12,2));CREATE TABLE Reservation(	ReservationID INT PRIMARY KEY IDENTITY(201,1),
	CustomerID INT FOREIGN KEY REFERENCES Customer(CustomerID) ON DELETE CASCADE,
	VehicleID INT FOREIGN KEY REFERENCES Vehicle(VehicleID) ON DELETE CASCADE,
	StartDate DATETIME,
	EndDate DATETIME,
	TotalCost DECIMAL (12,2),
	Status VARCHAR(10)
);

CREATE TABLE Admin(
	AdminID INT PRIMARY KEY IDENTITY(301,1),
	FirstName VARCHAR(30),
	LastName VARCHAR(30),
	Email VARCHAR(50) UNIQUE,
	PhoneNumber VARCHAR(20) UNIQUE,
	Username VARCHAR(30) UNIQUE,
	Password VARCHAR(64),
	Role VARCHAR(30),
	JoinDate DATE
);

CREATE TABLE Logins(
	UserName VARCHAR(50) PRIMARY KEY,
	Password VARCHAR(50)
);


--Inserting Data
INSERT INTO Customer (FirstName, LastName, Email, PhoneNumber, Address, Username, Password, RegistrationDate)
VALUES
('John', 'Doe', 'john.doe@gmail.com', '9123456780', '123 Main St, Springfield', 'johndoe', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'John@!6780'), 2), '2023-11-17'),
('Jane', 'Smith', 'jane.smith@gmail.com', '9123456781', '456 Elm St, Willowdale', 'janesmith', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Jane#6781'), 2), '2023-12-18'),
('Michael', 'Johnson', 'michael.johnson@gmail.com', '9123456782', '789 Oak St, Greenfield', 'michaeljohnson', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Michael$6782'), 2), '2024-01-19'),
('Emily', 'Williams', 'emily.williams@gmail.com', '9123456783', '101 Pine St, Oakwood', 'emilywilliams', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Emily&6783'), 2), '2024-02-20'),
('David', 'Brown', 'david.brown@gmail.com', '9123456784', '202 Cedar St, Riverside', 'davidbrown', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'David*6784'), 2), '2024-03-21'),
('Jessica', 'Jones', 'jessica.jones@gmail.com', '9123456785', '303 Maple St, Springvale', 'jessicajones', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Jessica^6785'), 2), '2024-04-22'),
('Christopher', 'Martinez', 'chris.martinez@gmail.com', '9123456786', '505 Walnut St, Brookside', 'chrismartinez', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Christopher_6786'), 2), '2024-05-23'),
('Amanda', 'Taylor', 'amanda.taylor@gmail.com', '9123456787', '707 Pine St, Pineville', 'amandataylor', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Amanda@6787'), 2), '2024-06-24'),
('Daniel', 'Anderson', 'daniel.anderson@gmail.com', '9123456788', '909 Oak St, Cedarville', 'danielanderson', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Daniel!6788'), 2), '2024-07-25'),
('Sarah', 'Garcia', 'sarah.garcia@gmail.com', '9123456789', '1001 Cedar St, Hillcrest', 'sarahgarcia', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Sarah%6789'), 2), '2024-08-26');


INSERT INTO Vehicle (Model, Make, Year, Color, RegistrationNumber, Availability, DailyRate)
VALUES
('Civic', 'Honda', 2022, 'Blue', 'MH123456', 1, 2500.00),
('Accord', 'Honda', 2019, 'Black', 'DL654321', 1, 3000.00),
('Camry', 'Toyota', 2020, 'Silver', 'KA987654', 0, 2750.00),
('Corolla', 'Toyota', 2018, 'White', 'TN456789', 1, 2250.00),
('Altima', 'Nissan', 2021, 'Red', 'UP234567', 1, 2750.00),
('Sentra', 'Nissan', 2019, 'Gray', 'GJ765432', 0, 2000.00),
('Fusion', 'Ford', 2020, 'Green', 'WB345678', 1, 3250.00),
('Focus', 'Ford', 2017, 'Yellow', 'HR876543', 1, 2000.00),
('Corvette', 'Chevrolet', 2023, 'Orange', 'MP543210', 1, 7500.00),
('Camaro', 'Chevrolet', 2022, 'Black', 'UP012345', 0, 6000.00);


INSERT INTO Reservation (CustomerID, VehicleID, StartDate, EndDate, TotalCost, Status)
VALUES
(1, 101, '2024-04-17 10:00:00', '2024-04-19 12:00:00', 5000.00, 'confirmed'),
(2, 102, '2024-04-18 11:00:00', '2024-04-20 13:00:00', 6000.00, 'confirmed'),
(3, 103, '2024-04-19 12:00:00', '2024-04-21 14:00:00', 5500.00, 'pending'),
(4, 104, '2024-04-20 13:00:00', '2024-04-22 15:00:00', 4500.00, 'pending'),
(5, 105, '2024-04-21 14:00:00', '2024-04-23 16:00:00', 5500.00, 'confirmed'),
(6, 106, '2024-04-22 15:00:00', '2024-04-24 17:00:00', 4000.00, 'confirmed'),
(7, 107, '2024-04-23 16:00:00', '2024-04-25 18:00:00', 6500.00, 'pending'),
(8, 108, '2024-04-14 17:00:00', '2024-04-16 19:00:00', 4000.00, 'completed'),
(9, 109, '2024-04-25 18:00:00', '2024-04-27 20:00:00', 7500.00, 'pending'),
(10,110, '2024-04-26 19:00:00', '2024-04-28 21:00:00', 6000.00, 'confirmed');


INSERT INTO Admin (FirstName, LastName, Email, PhoneNumber, Username, Password, Role, JoinDate)
VALUES
('Robert', 'Johnson', 'robert.johnson@gmail.com', '9123456780', 'robertjohnson', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Robert@1234'), 2), 'super admin', '2023-11-17'),
('Emma', 'Davis', 'emma.davis@gmail.com', '9123456781', 'emmadavis', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Emma!5678'), 2), 'fleet manager', '2023-12-18'),
('William', 'Brown', 'william.brown@gmail.com', '9123456782', 'williambrown', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'William#91011'), 2), 'admin', '2024-01-19'),
('Olivia', 'Wilson', 'olivia.wilson@gmail.com', '9123456783', 'oliviawilson', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Olivia$121314'), 2), 'admin', '2024-02-20'),
('James', 'Taylor', 'james.taylor@gmail.com', '9123456784', 'jamestaylor', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'James%151617'), 2), 'fleet manager', '2024-03-21'),
('Sophia', 'Moore', 'sophia.moore@gmail.com', '9123456785', 'sophiamoore', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Sophia^181920'), 2), 'admin', '2024-04-22'),
('Benjamin', 'Anderson', 'benjamin.anderson@gmail.com', '9123456786', 'benjaminanderson', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Benjamin&212223'), 2), 'admin', '2024-05-23'),
('Isabella', 'Martinez', 'isabella.martinez@gmail.com', '9123456787', 'isabellamartinez', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Isabella*242526'), 2), 'fleet manager', '2024-06-24'),
('Mason', 'Harris', 'mason.harris@gmail.com', '9123456788', 'masonharris', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Mason@272829'), 2), 'admin', '2024-07-25'),
('Charlotte', 'Clark', 'charlotte.clark@gmail.com', '9123456789', 'charlotteclark', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'Charlotte!303132'), 2), 'super admin', '2024-08-26');

UPDATE Reservation  SET TotalCost = DATEDIFF(DAY, StartDate, EndDate) * (SELECT DailyRate FROM Vehicle V WHERE Reservation.VehicleID = V.VehicleID) ;
Update Vehicle SET Availability = 0 where VehicleID in (SELECT VehicleID FROM Reservation);

SELECT * FROM Customer;
SELECT * FROM Vehicle;
SELECT * FROM Reservation;
SELECT * FROM Admin;
SELECT * FROM Logins;

