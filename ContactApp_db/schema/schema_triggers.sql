/* SCHEMA FOR C# CONTACT APPLICATION */

USE [db_contacts]
GO


/* --- TRIGGERS --- */
-- TO DO
-- delete trigger (similar to cascade functionality)

-- drop and create in case of master schema changes
IF OBJECT_ID('dbo.trINSUPD_vcontact_data_all') is not null DROP TRIGGER [dbo].[trINSUPD_vcontact_data_all] 
GO

	CREATE TRIGGER [dbo].[trINSUPD_vcontact_data_all] ON [dbo].[vcontact_data_all]
	INSTEAD OF INSERT, UPDATE
	AS
	BEGIN
		DECLARE @contact_id int, @retval int, @errmess varchar(MAX)

		-- create temp table
		SELECT * INTO #vcontact_data_all FROM inserted

		-- handle insert / update logic (assuming no entries with same first/last name combination)
		IF EXISTS(SELECT 1 FROM vcontact_data_all a
			JOIN inserted i (NOLOCK) ON a.first_name = i.first_name AND a.last_name = i.last_name)
		BEGIN
			-- update existing records
			UPDATE exist SET first_name = i.first_name, last_name = i.last_name
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
		END
		ELSE BEGIN
			-- create new records
			INSERT contact_main
			SELECT first_name, last_name FROM inserted i

				IF @@ROWCOUNT > 0
				BEGIN
					-- get newly created contact_id
					SELECT @contact_id = MAX(contact_id) FROM contact_main
				END
				ELSE BEGIN
					SELECT @retval = -1, @errmess = 'FAILURE CREATING contact_main RECORD'
					GOTO ERROR
				END

			INSERT contact_address
			SELECT @contact_id, address, city, state, zip FROM inserted i 
			
			INSERT contact_phone
			SELECT @contact_id, phone_home, phone_cell, phone_work FROM inserted i 
			
			INSERT contact_email
			SELECT @contact_id, email_personal, email_work FROM inserted i 
			
			INSERT contact_website
			SELECT @contact_id, website, github FROM inserted i 

				SELECT @retval = 1, @errmess = NULL -- assume success if reach this point
				GOTO SUCCESS
		END

		SUCCESS:
			SELECT 'SUCCESS'
			RETURN

		ERROR:
			SELECT 'FAIL', @retval retval, @errmess err
			RETURN
	END