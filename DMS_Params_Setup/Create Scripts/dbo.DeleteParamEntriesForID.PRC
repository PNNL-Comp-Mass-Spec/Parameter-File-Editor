SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE DeleteParamEntriesForID
/****************************************************
**
**	Desc: Deletes Sequest Param Entries from a given Param File ID
**
**	Return values: 0: success, otherwise, error code
**
**	Parameters: 
**
**	
**
**		Auth: kja
**		Date: 7/22/2004
**    
*****************************************************/
(
	@paramFileID int,
	@message varchar(512) output
)
As
	set nocount on

	declare @myError int
	set @myError = 0

	declare @myRowCount int
	set @myRowCount = 0
	
	set @message = ''
	
	declare @msg varchar(256)
	
	declare @result int

	---------------------------------------------------
	-- Start transaction
	---------------------------------------------------

	declare @transName varchar(32)
	set @transName = 'DeleteParamEntries'
	begin transaction @transName
--	print 'start transaction' -- debug only

	---------------------------------------------------
	-- delete any entries for the parameter file from the entries table
	---------------------------------------------------

	DELETE FROM T_Param_Entries 
	WHERE (Param_File_ID = @ParamFileID)
	if @@error <> 0
	begin
		rollback transaction @transName
		RAISERROR ('Delete from entries table was unsuccessful for param file',
			10, 1)
		return 51130
	end
		
	---------------------------------------------------
	-- delete any entries for the parameter file from the global mod mapping table
	---------------------------------------------------
	
	DELETE FROM T_Peptide_Mod_Param_File_List
	WHERE (Param_File_ID = @ParamFileID)
	
	if @@error <> 0
	begin
		rollback transaction @transname
		RAISERROR ('Delete from global mapping table was unsuccessful for param file',
			10, 1)
		return 51131
	end
	
	commit transaction @transname
	
	return 0

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

