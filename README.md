This project was made in Visual studio using the ASP.NET Core Web App (Model-View-Controller) template and was coded using the .NET 7.0 Framework.
The goal of this project was to create a appointment management system using MVC and host it using a cloud based service. 
The project was originally hosted using Microsoft Azure which hosted both the project and the mysql database it was connected to.
Currently this project is no longer being hosted using Azure so in order to view it functionality first hand you would have to setup your own mysql database and download the proper connectors (.NET, Visual Studio).
Otherwise you can view this video to see a demonstration showing most of the applications functionality. ""
If you choose to set up your own test envirorment you can use the following mysql code to create a test database
"-- Create user Table
CREATE TABLE user (
    userId INT AUTO_INCREMENT PRIMARY KEY,
    userName VARCHAR(255),
    userPassword VARCHAR(255)
);
-- Fill user Table With test account
INSERT INTO user (userName, userPassword)
VALUES
('test', 'test');
-- Create appointment Table
CREATE TABLE appointment (
    appointmentId INT auto_increment PRIMARY KEY,
    userId INT,
    customerName VARCHAR(255),
    appointmentType VARCHAR(255),
    customerPhone VARCHAR(255),
    appointmentStart DATETIME,
    appointmentEnd DATETIME,
    createdBy VARCHAR(255),
    FOREIGN KEY (userId) REFERENCES user(userId)
);
-- Fill appointment Table with  test data
INSERT INTO appointment (userId, customerName, appointmentType, customerPhone, appointmentStart, appointmentEnd, createdBy)
VALUES 
    (
        1, 
        'John Doe', 
        'check-up', 
        CONCAT('(', ROUND(RAND()*1000), ')-', ROUND(RAND()*1000), '-', ROUND(RAND()*10000)), 
        DATE_ADD(NOW(), INTERVAL 30 DAY), 
        DATE_ADD(DATE_ADD(NOW(), INTERVAL 30 DAY), INTERVAL 1 HOUR), 
        'test'
    ),
    (
		1, 
        'Jane Smith', 
        'other', 
        CONCAT('(', ROUND(RAND()*1000), ')-', ROUND(RAND()*1000), '-', ROUND(RAND()*10000)), 
        DATE_ADD(NOW(), INTERVAL 37 DAY), 
        DATE_ADD(DATE_ADD(NOW(), INTERVAL 37 DAY), INTERVAL 1 HOUR),  
        'test'),
    (
		1, 
        'Micheal Johnson', 
        'check-up', 
        CONCAT('(', ROUND(RAND()*1000), ')-', ROUND(RAND()*1000), '-', ROUND(RAND()*10000)), 
        DATE_ADD(NOW(), INTERVAL 17 DAY), 
        DATE_ADD(DATE_ADD(NOW(), INTERVAL 17 DAY), INTERVAL 1 HOUR), 
        'test'),
    (
		1, 
        'Sarah Brown', 
        'check-up', 
        CONCAT('(', ROUND(RAND()*1000), ')-', ROUND(RAND()*1000), '-', ROUND(RAND()*10000)), 
        DATE_ADD(NOW(), INTERVAL 19 DAY), 
        DATE_ADD(DATE_ADD(NOW(), INTERVAL 19 DAY), INTERVAL 1 HOUR), 
        'test'),
    (
		1, 
        'David Williams', 
        'check-up', 
        CONCAT('(', ROUND(RAND()*1000), ')-', ROUND(RAND()*1000), '-', ROUND(RAND()*10000)), 
        DATE_ADD(NOW(), INTERVAL 15 DAY), 
        DATE_ADD(DATE_ADD(NOW(), INTERVAL 15 DAY), INTERVAL 1 HOUR), 
        'test');
