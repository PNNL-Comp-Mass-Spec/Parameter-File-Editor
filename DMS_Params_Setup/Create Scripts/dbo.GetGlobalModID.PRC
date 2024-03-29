SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetGlobalModID
/****************************************************
**
**	Desc: Gets global ModID for given ModSymbol
**
**	Return values: 0: failure, otherwise, ParamFileID
**
**	Parameters: 
**
**		Auth: kja
**		Date: 08/02/2004
**    
*****************************************************/
(
		@modMass varchar(32),
		@modType char(1),
		@modResidues char(32)
)
As
	declare @ModID int
	set @ModID = 0
	SELECT @ModID = Mod_ID FROM T_Peptide_Mod_Global_List 
		WHERE (Mass_Correction_Factor = @modMass) AND 
			(SD_Flag = @modType) AND 
			(Affected_Residues = @modResidues)
			
	return(@ModID)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

