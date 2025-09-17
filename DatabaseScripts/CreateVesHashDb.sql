-- Script to create the VesHashDB database and map the shared SQL login
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = N'VesHashDB')
BEGIN
    PRINT 'Creating database VesHashDB';
    CREATE DATABASE [VesHashDB];
END
ELSE
BEGIN
    PRINT 'Database VesHashDB already exists';
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

USE [VesHashDB];
GO

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = N'test')
BEGIN
    PRINT 'Creating database user test for VesHashDB';
    CREATE USER [test] FOR LOGIN [test];
END
ELSE
BEGIN
    PRINT 'Database user test already exists in VesHashDB';
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
    PRINT 'Adding test user to db_owner role in VesHashDB';
    ALTER ROLE [db_owner] ADD MEMBER [test];
END
ELSE
BEGIN
    PRINT 'User test is already member of db_owner role in VesHashDB';
END
GO
