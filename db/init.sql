-- Create database if not exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'appdb')
BEGIN
    CREATE DATABASE appdb;
END
GO

USE appdb;
GO

-- Create Users table safely
IF NOT EXISTS (
    SELECT * FROM sys.tables WHERE name = 'Users'
)
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(100) NOT NULL
    );

    INSERT INTO Users (Name, Email)
    VALUES
    ('Alice', 'alice@example.com'),
    ('Bob', 'bob@example.com');
END
GO
