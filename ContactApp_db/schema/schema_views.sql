/* SCHEMA FOR C# CONTACT APPLICATION */
/* Copyright 2018 || Cole Dixon || All rights reserved */

USE [db_contacts]
GO


/* --- VIEWS --- */
-- drop and create in case of master schema changes

-----
--- VIEW for all related contact data
-----
IF OBJECT_ID('dbo.vcontact_data_all') is not null DROP VIEW [dbo].[vcontact_data_all]
GO

	CREATE VIEW [dbo].[vcontact_data_all]
	AS

	SELECT main.contact_id, first_name, last_name, address, city, state, zip, 
		phone_home, phone_cell, phone_work, email_personal, email_work, website, github 
		FROM contact_main main
			LEFT JOIN contact_address addr (NOLOCK) ON main.contact_id = addr.contact_id
			LEFT JOIN contact_phone phone (NOLOCK) ON addr.contact_id = phone.contact_id
			LEFT JOIN contact_email email (NOLOCK) ON phone.contact_id = email.contact_id
			LEFT JOIN contact_website website (NOLOCK) ON email.contact_id = website.contact_id

	GO

-----
--- VIEW for all related audit data
-----
IF OBJECT_ID('dbo.vcontact_audit_all') is not null DROP VIEW [dbo].[vcontact_audit_all]
GO

	CREATE VIEW [dbo].[vcontact_audit_all]
	AS

	SELECT main.audit_key, main.contact_id, first_name, last_name, address, city, state, zip, 
		phone_home, phone_cell, phone_work, email_personal, email_work, website, github 
		FROM audit_contact main
			LEFT JOIN audit_info info (NOLOCK) ON main.audit_key = info.audit_key


	GO

