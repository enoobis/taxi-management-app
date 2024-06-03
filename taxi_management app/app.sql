-- Создание таблицы Orders
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY,
    PhoneNumber NVARCHAR(50),
    FromLocation NVARCHAR(100),
    HasLuggage BIT,
    HasChildren BIT,
    ToLocation NVARCHAR(100),
    OrderType NVARCHAR(50),
    Status NVARCHAR(50),
    DriverId INT,
    PlannedTime DATETIME
);

-- Создание таблицы Drivers
CREATE TABLE Drivers (
    Id INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(100),
    CarBrand NVARCHAR(50),
    CarModel NVARCHAR(50),
    CarType NVARCHAR(50),
    CarNumber NVARCHAR(50),
    PhoneNumber NVARCHAR(50),
    Year INT,
    Color NVARCHAR(50),
    Status NVARCHAR(50)
);

-- Создание таблицы CallLog
CREATE TABLE CallLog (
    Id INT PRIMARY KEY IDENTITY,
    CallTime DATETIME,
    CallerName NVARCHAR(100),
    CallerPhoneNumber NVARCHAR(50),
    CallDuration TIME,
    CallNotes NVARCHAR(MAX)
);
