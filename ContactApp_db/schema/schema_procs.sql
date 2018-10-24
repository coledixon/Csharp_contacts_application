/* SCHEMA FOR C# CONTACT APPLICATION */

USE [db_contacts]
GO


/* --- PROCS ---*/

-- drop and create in case of master schema changes
IF OBJECT_ID ('dbo.spwrite_audit') is not null DROP PROC [dbo].[spwrite_audit]
GO

	CREATE PROC spwrite_audit
	AS

	-- assume #tmp_audit exists from trDEL_vcontact_data_all
	-- read from #tmp_audit
	
	-- logic for writing to audit tables
	-- evela

GO