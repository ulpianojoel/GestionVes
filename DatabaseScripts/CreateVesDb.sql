-- Script to create the VesDB database, SQL login, and database user
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'VesDB')
BEGIN
    PRINT 'Creating database VesDB';
    CREATE DATABASE [VesDB];
END
ELSE
BEGIN
    PRINT 'Database VesDB already exists';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.server_principals WHERE name = N'test')
BEGIN
    PRINT 'Creating SQL login test';
    CREATE LOGIN [test] WITH PASSWORD = 'test', CHECK_POLICY = OFF, CHECK_EXPIRATION = OFF;
END
ELSE
BEGIN
    PRINT 'Login test already exists';
END
GO

USE [VesDB];
GO

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'test')
BEGIN
    PRINT 'Creating database user test for VesDB';
    CREATE USER [test] FOR LOGIN [test];
END
ELSE
BEGIN
    PRINT 'Database user test already exists in VesDB';
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.database_role_members drm
    JOIN sys.database_principals rp ON rp.principal_id = drm.role_principal_id
    JOIN sys.database_principals mp ON mp.principal_id = drm.member_principal_id
    WHERE rp.name = N'db_owner' AND mp.name = N'test'
)
BEGIN
    PRINT 'Adding test user to db_owner role in VesDB';
    ALTER ROLE [db_owner] ADD MEMBER [test];
END
ELSE
BEGIN
    PRINT 'User test is already member of db_owner role in VesDB';
END
GO
