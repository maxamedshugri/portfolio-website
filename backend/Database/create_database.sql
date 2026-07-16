-- ============================================
-- Portfolio Website Database
-- Run this script in SQL Server Management Studio (SSMS)
-- or via sqlcmd against your local SQL Server instance
-- ============================================

CREATE DATABASE PortfolioDB;
GO

USE PortfolioDB;
GO

-- Users table (admin login)
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- Contact messages table (from the homepage contact form)
CREATE TABLE ContactMessages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE()
);
GO

-- NOTE: The default admin user (username: admin / password: Admin@123)
-- is created automatically the first time you run the API project,
-- because the password needs to be securely hashed with BCrypt in C#
-- rather than typed as plain text into SQL. See Program.cs.
