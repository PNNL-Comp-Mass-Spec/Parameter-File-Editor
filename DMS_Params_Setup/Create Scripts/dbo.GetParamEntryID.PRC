SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetParamEntryID
/****************************************************
**
**	Desc: Gets ParamEntryID for given set of param entry specs
**
**	Return values: 0: failure, otherwise, ParamFileID
**
**	Parameters: 
**
**		Auth: kja
**		Date: 7/22/2004
**    
*****************************************************/
(
		@ParamFileID int,
		@EntryType varchar(32),
		@EntrySpecifier varchar(32),
		@EntrySeqOrder int
)
As
	declare @ParamEntryID int
	set @ParamEntryID = 0
	
	
	SELECT @ParamEntryID = Param_Entry_ID FROM T_Param_Entries
		WHERE ((Param_File_ID = @ParamFileID) AND (Entry_Type = @EntryType) 
			AND (Entry_Specifier = @EntrySpecifier) 
			AND (Entry_Sequence_Order = @EntrySeqOrder))
		
	return(@ParamEntryID)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

