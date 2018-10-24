/* SCHEMA FOR C# CONTACT APPLICATION */

IF NOT EXISTS(SELECT name FROM sys.databases WHERE name ='db_contacts')
BEGIN
	CREATE DATABASE [db_contacts]
END
GO

USE [db_contacts]
GO


/* --- TABLES --- */

IF OBJECT_ID('dbo.contact_main') is null
BEGIN
	-- master contact table
	CREATE TABLE contact_main (
			[contact_id] int IDENTITY(1,1),
			[first_name] varchar(50) not null,
			[last_name] varchar(50) not null,
		PRIMARY KEY NONCLUSTERED 
		(
			[contact_id] ASC
		) ON [PRIMARY]
	)
END

GO

IF OBJECT_ID('dbo.contact_address') is null
BEGIN
	-- contact address
	CREATE TABLE contact_address (
			[contact_id] int not null,
			[address] varchar(200) null,
			[city] varchar(50) null,
			[state] varchar(2) null,
			[zip] int null,
		CONSTRAINT fk_addressContact FOREIGN KEY (contact_id) REFERENCES contact_main(contact_id)
	)

		CREATE UNIQUE NONCLUSTERED INDEX natKey_address ON contact_address (
			[contact_id]
		)
END

GO


IF OBJECT_ID('dbo.contact_phone') is null
BEGIN
	-- contact phone numbers
	CREATE TABLE contact_phone (
			[contact_id] int not null,
			[phone_home] varchar(14) null,
			[phone_cell] varchar(14) null,
			[phone_work] varchar(14) null
		CONSTRAINT fk_phoneContact FOREIGN KEY (contact_id) REFERENCES contact_main(contact_id)
	)

		CREATE UNIQUE NONCLUSTERED INDEX natKey_phone ON contact_phone (
			[contact_id]
		)
END

GO

IF OBJECT_ID('dbo.contact_email') is null
BEGIN
	-- contact email records
	CREATE TABLE contact_email (
			[contact_id] int not null,
			[email_personal] varchar(50) null,
			[email_work] varchar(50) null,
		CONSTRAINT fk_emailContact FOREIGN KEY (contact_id) REFERENCES contact_main(contact_id)
	)

		CREATE UNIQUE NONCLUSTERED INDEX natKey_email ON contact_email (
			[contact_id]
		)
END

GO

IF OBJECT_ID('dbo.contact_website') is null
BEGIN
	-- contact websites
	CREATE TABLE contact_website (
			[contact_id] int not null,
			[website] varchar(MAX) null,
			[github] varchar(MAX) null,
		CONSTRAINT fk_websiteContact FOREIGN KEY (contact_id) REFERENCES contact_main(contact_id)
	)

		CREATE UNIQUE NONCLUSTERED INDEX natKey_website ON contact_website (
			[contact_id]
		)
END

GO
