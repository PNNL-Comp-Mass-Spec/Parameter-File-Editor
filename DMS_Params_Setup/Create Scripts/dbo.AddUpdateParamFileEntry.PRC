SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE AddUpdateParamFileEntry
/****************************************************
**
**	Desc: Adds new or updates existing parameter file entry in database
**
**	Return values: 0: success, otherwise, error code
**
**	Parameters: 
**
**		@paramFileID  name of new param file description
**		@entryType  description of paramfileentry 
**		@entrySpecifier
**	    @entryValue
**
**		Auth: kja
**		Date: 07/22/2004
**		
**		Date: 08/10/2004 - Added in code to update mapping table as well
**    
*****************************************************/
(
	@paramFileID int,
	@entrySeqOrder int,
	@entryType varchar(32), 
	@entrySpecifier varchar(32),
	@entryValue varchar(32),
	@mode varchar(12) = 'add', -- or 'update'
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

	---------------------------------------------------
	-- Validate input fields
	---------------------------------------------------

	set @myError = 0
	if @paramFileID = 0
	begin
		set @myError = 51000
		RAISERROR ('ParamFileID was blank',
			10, 1)
	end
	
	--

	if @entrySeqOrder = 0
	begin
		set @myError = 51001
		RAISERROR ('EntrySeqOrder was blank',
			10, 1)
	end
	
	--

	if LEN(@entryType) < 1
	begin
		set @myError = 51001
		RAISERROR ('EntryType was blank',
			10, 1)
	end

	--
		if LEN(@entrySpecifier) < 1
	begin
		set @myError = 51001
		RAISERROR ('EntrySpecifier was blank',
			10, 1)
	end

	--

		if LEN(@entryValue) < 1
	begin
		set @myError = 51001
		RAISERROR ('EntryValue was blank',
			10, 1)
	end

	--

	if @myError <> 0
		return @myError
	---------------------------------------------------
	-- Is entry already in database?
	---------------------------------------------------

	declare @ParamEntryID int
	set @ParamEntryID = 0
	--
	execute @ParamEntryID = GetParamEntryID @ParamFileID, @EntryType, @EntrySpecifier, @EntrySeqOrder
	
	
	if @ParamEntryID <> 0
	begin
		set @mode = 'update'
	end
	
	-- cannot create an entry that already exists
	--
	if @ParamEntryID <> 0 and @mode = 'add'
	begin
		set @msg = 'Cannot add: Param Entry "' + @ParamEntryID + '" already in database '
		RAISERROR (@msg, 10, 1)
		return 51004
	end

	-- cannot update a non-existent entry
	--
	if @ParamFileID = 0 and @mode = 'update'
	begin
		set @msg = 'Cannot update: Param Entry "' + @ParamEntryID + '" is not in database '
		RAISERROR (@msg, 10, 1)
		return 51004
	end

	---------------------------------------------------
	-- action for add mode
	---------------------------------------------------
	
	declare @transName varchar(32)
	set @transName = 'AddParamEntries'
	begin transaction @transName

	
	if @Mode = 'add'
	begin

		INSERT INTO T_Param_Entries (
			Entry_Sequence_Order, 
			Entry_Type, 
			Entry_Specifier, 
			Entry_Value, 
			Param_File_ID
		) VALUES (
			@entrySeqOrder, 
			@entryType, 
			@entrySpecifier, 
			@entryValue,  
			@paramFileID
		)
		--
		SELECT @myError = @@error, @myRowCount = @@rowcount
		--
		if @myError <> 0
		begin
			set @msg = 'Insert operation failed: "' + @ParamEntryID + '"'
			RAISERROR (@msg, 10, 1)
			return 51007
		end
	end -- add mode
	if @@error <> 0
	begin
		rollback transaction @transname
		RAISERROR ('Addition to param entry table was unsuccessful for param file',
			10, 1)
		return 51131
	end
	
	
	---------------------------------------------------
	-- add to global mod mapping table
	---------------------------------------------------
	
	if ((@entryType = 'DynamicModification') OR (@entryType = 'StaticModification'))
	begin
		declare @localSymbolID int
		declare @typeSymbol char(1)
		declare @affectedResidues varchar(32)
		declare @globalModID int

		if @entryType = 'StaticModification'
		begin
			set @localSymbolID = 0
			set @typeSymbol = 'S'
			
			if len(@entryspecifier) > 1
			begin
				set @affectedResidues = '-'
			end
			else
				set @affectedResidues = @entryspecifier
		end
	
		if @entryType = 'DynamicModification'
		begin
			execute @localSymbolID = GetNextLocalSymbolID @ParamFileID
			set @typeSymbol = 'D'
			set @affectedResidues = @entrySpecifier
		end

			execute @globalModID = GetGlobalModID @entryValue, @typeSymbol, @affectedresidues
			
	
			INSERT INTO T_Peptide_Mod_Param_File_List (
			Local_Symbol_ID,
			Mod_ID,
			Param_File_ID
		) VALUES (
			@localSymbolID, 
			@globalModID, 
			@paramFileID
		)
		
		--
		SELECT @myError = @@error, @myRowCount = @@rowcount
		--
		if @myError <> 0
		begin
			set @msg = 'Insert operation failed: "' + @ParamfileID + '"'
			RAISERROR (@msg, 10, 1)
			return 51007
		end
		
		if @@error <> 0
		begin
			rollback transaction @transname
			RAISERROR ('Update to global mod mapping table was unsuccessful for param file', 
				10, 1)
		end



	end
	
	

	---------------------------------------------------
	-- action for update mode
	---------------------------------------------------
	--
	if @Mode = 'update' 
	begin
		set @myError = 0
		--
		UPDATE T_Param_Entries 
		SET 
			Entry_Specifier = @entrySpecifier,
			Entry_Value = @entryValue
		WHERE (Param_Entry_ID = @ParamEntryID)
		--
		SELECT @myError = @@error, @myRowCount = @@rowcount
		--
		if @myError <> 0
		begin
			set @msg = 'Update operation failed: "' + @ParamEntryID + '"'
			RAISERROR (@msg, 10, 1)
			return 51004
		end
	end -- update mode

	if @@error <> 0
	begin
		rollback transaction @transname
		RAISERROR ('Update to param entry table was unsuccessful for param file',
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

