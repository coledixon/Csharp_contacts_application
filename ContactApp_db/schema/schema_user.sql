/* SCHEMA FOR C# CONTACT APPLICATION */
/* Copyright 2018 || Cole Dixon || All rights reserved */

USE [db_contacts]
GO

/* USER ROLES */
-- create default sysadmin user (server level)
IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = 'db_admin')
BEGIN
	CREATE LOGIN db_admin WITH PASSWORD = N'admin'
END
-- create default sysadmin user (database level)
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'db_admin')
BEGIN
	CREATE USER db_admin FOR LOGIN db_admin
	EXEC sp_addrolemember 'db_owner','db_admin'
END
