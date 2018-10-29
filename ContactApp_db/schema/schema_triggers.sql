/* SCHEMA FOR C# CONTACT APPLICATION */
/* Copyright 2018 || Cole Dixon || All rights reserved */

USE [db_contacts]
GO


/* --- TRIGGERS --- */
-- drop and create in case of master schema changes

-- TO DO
-- del triggers on audit_contact / audit_info

-----
--- INSERT/UPDATE TRIGGER on vcontact_data_all
-----
IF OBJECT_ID('dbo.trINSUPD_vcontact_data_all') is not null DROP TRIGGER [dbo].[trINSUPD_vcontact_data_all] 
GO

	CREATE TRIGGER [dbo].[trINSUPD_vcontact_data_all] ON [dbo].[vcontact_data_all]
	INSTEAD OF INSERT, UPDATE
	AS
	BEGIN
		DECLARE @contact_id int, @retval int, @errmess varchar(MAX)

		-- create temp table
		SELECT inserted.* INTO #vcontact_data_all FROM inserted

		-- handle insert / update logic
		IF EXISTS(SELECT 1 FROM vcontact_data_all a
			JOIN inserted i (NOLOCK) ON a.contact_id = i.contact_id)
		BEGIN
			-- update existing records
			UPDATE exist SET first_name = i.first_name, last_name = i.last_name, upd_date = GETDATE()
				FROM inserted i
				JOIN contact_main exist (NOLOCK) ON exist.contact_id = i.contact_id

			UPDATE exist SET address = i.address, city = i.city, state = i.state, zip = i.zip
				FROM inserted i 
				JOIN contact_address exist (NOLOCK) ON exist.contact_id = i.contact_id

			UPDATE exist SET phone_home = i.phone_home, phone_cell = i.phone_cell, phone_work = i.phone_work
				FROM inserted i
				JOIN contact_phone exist (NOLOCK) ON exist.contact_id = i.contact_id

			UPDATE	exist SET email_personal = i.email_personal, email_work = i.email_work
				FROM inserted i
				JOIN contact_email exist (NOLOCK) ON exist.contact_id = i.contact_id

			UPDATE exist SET website = i.website, github = i.github
				FROM inserted i
				JOIN contact_website exist (NOLOCK) ON exist.contact_id = i.contact_id
			
			IF @@ROWCOUNT = 0
			BEGIN
				SELECT @retval = -1, @errmess = 'NO RECORD(S) UPDATED IN trINSUPD_vcontact_data_all'
				GOTO ERROR
			END
		END
		ELSE BEGIN

			-- create new records
			INSERT contact_main
			SELECT contact_id, first_name, last_name, GETDATE(), null FROM inserted i

			INSERT contact_address
			SELECT contact_id, address, city, state, zip FROM inserted i 

			INSERT contact_phone
			SELECT contact_id, phone_home, phone_cell, phone_work FROM inserted i 

			INSERT contact_email
			SELECT contact_id, email_personal, email_work FROM inserted i 

			INSERT contact_website
			SELECT contact_id, website, github FROM inserted i 

				SELECT @retval = 1, @errmess = NULL -- assume success if reach this point
				GOTO SPEND
		END

		SPEND:
			SELECT 'SUCCESS'
			RETURN

		ERROR:
			SELECT 'FAIL', @retval retval, @errmess err
			RETURN
	END

GO

-----
--- DELETE TRIGGER on vcontact_data_all
-----
IF OBJECT_ID('dbo.trDEL_vcontact_data_all') is not null DROP TRIGGER [dbo].[trDEL_vcontact_data_all] 
GO

	CREATE TRIGGER [dbo].[trDEL_vcontact_data_all] ON [dbo].[vcontact_data_all]
	INSTEAD OF DELETE
	AS
	BEGIN
		DECLARE @contact_id int, @retval int, @errmess varchar(MAX)

		-- create temp table
		SELECT * INTO #tmp_audit FROM deleted

		IF @@ROWCOUNT > 0
		BEGIN
			-- write audit record(s)
			EXEC spwrite_audit @retval = @retval OUTPUT, @errmess = @errmess OUTPUT

			IF (COALESCE(@retval,0) <= 0)
			BEGIN
				SELECT @retval = COALESCE(@retval,-1), @errmess = COALESCE(@errmess, 'ERROR RUNNING spwrite_audit')
				GOTO ERROR
			END
		END

		-- begin cascading delete of record(s)
		SELECT @contact_id = COALESCE(contact_id,0) FROM #tmp_audit

			IF (COALESCE(@contact_id,0) = 0)
			BEGIN
				SELECT @retval = -1, @errmess = 'ERROR RETRIEVING contact_id FROM #tmp_audit'
				GOTO ERROR
			END

		BEGIN TRAN
			DELETE FROM contact_address WHERE contact_id = @contact_id

			DELETE FROM contact_phone WHERE contact_id = @contact_id

			DELETE FROM contact_email WHERE contact_id = @contact_id

			DELETE FROM contact_website WHERE contact_id = @contact_id

			DELETE FROM contact_main WHERE contact_id = @contact_id -- delete this record last due to FK constraints

			IF @@ROWCOUNT = 0
			BEGIN
				SELECT @retval = -1, @errmess = 'ERROR DELETING RECORD(S) IN trDEL_vcontact_data_all'
				GOTO ERROR
			END
			ELSE BEGIN
				SELECT @retval = 1 -- assume success
				GOTO SPEND
			END

		SPEND:
			COMMIT TRAN -- finalize transaction
			SELECT 'SUCCESS', @retval retval
			RETURN

		ERROR:
			ROLLBACK TRAN -- undo transaction
			SELECT 'FAIL', @retval retval, @errmess err
			RETURN
	END

GO

-----
--- DELETE TRIGGER on audit_contact
-----
IF OBJECT_ID('dbo.trDEL_audit_contact') is not null DROP TRIGGER [dbo].[trDEL_audit_contact]
GO

	CREATE TRIGGER [dbo].[trDEL_audit_contact] ON [dbo].[audit_contact]
	INSTEAD OF DELETE
	AS
	BEGIN
		DECLARE @user varchar(MAX), @allowdelete int, @retval int, @errmess varchar(250)

		SELECT @user = CURRENT_USER, @allowdelete = 0 -- default to curr user / allowdelete = false

		IF IS_ROLEMEMBER ('db_owner', @user) = 1
		BEGIN
			SET @allowdelete = 1 -- only db_owner can delete audit record(s)
		END

		IF (COALESCE(@allowdelete,0) = 0)
		BEGIN
			raiserror('USER DOES NOT HAVE DB PERMS TO DELETE AUDIT RECORDS.', 16, 1);
			SET @retval = -1
			GOTO ERROR
		END
		ELSE BEGIN
			DELETE i -- delete info record(s) first due to referential integrity
				FROM audit_info i
				JOIN deleted d (NOLOCK) ON d.contact_id = i.contact_id

			DELETE c
				FROM audit_contact c
				JOIN deleted d (NOLOCK) ON d.contact_id = c.contact_id

			IF @@ROWCOUNT = 0
			BEGIN
				SELECT @retval = -1, @errmess = 'ERROR DELETING RECORD FROM audit_contact / audit_info IN trDEL_audit_contact'
				GOTO ERROR
			END
			ELSE BEGIN
				SET @retval = 1
				GOTO SPEND
			END
		END

		SPEND:
			RETURN

		ERROR:
			SELECT 'FAIL', @retval retval, @errmess err
			RETURN
	END

GO