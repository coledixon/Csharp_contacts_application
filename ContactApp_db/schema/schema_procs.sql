/* SCHEMA FOR C# CONTACT APPLICATION */
/* Copyright 2018 || Cole Dixon || All rights reserved */

USE [db_contacts]
GO


/* --- PROCS ---*/
-- drop and create in case of master schema changes

-----
--- INSERT PROC for all audit table reocrds (audit_contact / audit_info)
-----
IF OBJECT_ID ('dbo.spwrite_audit') is not null DROP PROC [dbo].[spwrite_audit]
GO

	CREATE PROC spwrite_audit
	@retval int = NULL OUTPUT,
	@errmess varchar(250) = null OUTPUT 
	AS 
	DECLARE @audit_key int

	-- assume #tmp_audit exists/is populated from trDEL_vcontact_data_all
	IF OBJECT_ID('tempdb..#tmp_audit') is null
	BEGIN
		SELECT @retval = -1, @errmess = '#tmp_audit was sent from trDEL_vcontact_data_all empty'
		GOTO ERROR
	END

	INSERT audit_contact (contact_id, first_name, last_name, del_date)
	SELECT contact_id, first_name, last_name, GETDATE() FROM #tmp_audit

		IF @@ROWCOUNT > 0
		BEGIN
			SELECT @audit_key = MAX(audit_key) FROM audit_contact -- get newly created audit_key
		END
		ELSE BEGIN
			SELECT @retval = -1, @errmess = 'FAILURE CREATING audit_contact RECORD'
			GOTO ERROR
		END

	INSERT audit_info
	SELECT @audit_key, contact_id, address, city, state, zip, 
		phone_home, phone_cell, phone_work, email_personal, 
		email_work, website, github  
			FROM #tmp_audit

		IF @@ROWCOUNT > 0
		BEGIN
			SELECT @retval = 1 -- assume successful write
			GOTO SPEND
		END
		ELSE BEGIN
			SELECT @retval = -1, @errmess = 'ERROR CREATING AUDIT RECORD(S) IN spwrite_audit'
			GOTO ERROR
		END

	SPEND:
		SELECT 'SUCCESS', @retval retval
		RETURN
	
	ERROR:
		SELECT 'FAIL', @retval retval, @errmess err
		RETURN

GO

-----
--- FETCH PROC for returning next contact_id val to app
-----
IF OBJECT_ID('dbo.spgetNextContactId') is not null DROP PROC [dbo].[spgetNextContactId]
GO

	CREATE PROC [dbo].[spgetNextContactId]
	@contact_id int = 0 OUTPUT,
	@retval int = NULL OUTPUT,
	@errmess varchar(250) = NULL OUTPUT
	AS

	SELECT @contact_id = COALESCE(MAX(contact_id),1)
		FROM contact_main

		IF (COALESCE(@contact_id,0) = 0)
		BEGIN
			SELECT @retval = -1, @errmess = 'ERROR FETCHING NEXT contact_id'
			GOTO ERROR
		END
		ELSE BEGIN
			SELECT @contact_id = (@contact_id + 1) /* increment id */, @retval = 1
			GOTO SPEND
		END

	SPEND:
		SELECT 'SUCCESS', @retval retval
		RETURN
	
	ERROR:
		SELECT @retval retval, @errmess errmess
		RETURN

GO