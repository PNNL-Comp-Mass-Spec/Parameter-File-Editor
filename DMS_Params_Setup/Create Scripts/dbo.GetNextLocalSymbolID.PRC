SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetNextLocalSymbolID
/****************************************************
**
**	Desc: Gets Next Available LocalSymbolID for a given paramFileID
**
**	Return values: 0: failure, otherwise, LocalSymbolID
**
**	Parameters: 
**
**		Auth: kja
**		Date: 08/10/2004
**    
*****************************************************/
(
		@ParamFileID int
)
As
	declare @LocalSymbolID int	
	
	SELECT @LocalSymbolID = MAX(Local_Symbol_ID) FROM T_Peptide_Mod_Param_File_List
		WHERE (Param_File_ID = @ParamFileID)
		
	if @LocalSymbolID is null
	begin
		set @LocalSymbolID = 0
	end
		
	return(@LocalSymbolID + 1)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

