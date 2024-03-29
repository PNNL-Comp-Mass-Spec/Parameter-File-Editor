SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetParamFileID
/****************************************************
**
**	Desc: Gets ParamFileID for given ParamFileName
**
**	Return values: 0: failure, otherwise, ParamFileID
**
**	Parameters: 
**
**		Auth: grk
**		Date: 1/26/2001
**    
*****************************************************/
(
		@ParamFileName varchar(80) = " "
)
As
	declare @ParamFileID int
	set @ParamFileID = 0
	SELECT @ParamFileID = Param_File_ID FROM T_Param_Files WHERE (Param_File_Name = @ParamFileName)
	return(@ParamFileID)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

