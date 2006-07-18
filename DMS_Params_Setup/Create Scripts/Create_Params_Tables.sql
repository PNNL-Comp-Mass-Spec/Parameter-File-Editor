if not exists (select * from master.dbo.syslogins where loginname = N'DMSReader')
BEGIN
	declare @logindb nvarchar(132), @loginlang nvarchar(132) select @logindb = N'DMS5', @loginlang = N'us_english'
	if @logindb is null or not exists (select * from master.dbo.sysdatabases where name = @logindb)
		select @logindb = N'master'
	if @loginlang is null or (not exists (select * from master.dbo.syslanguages where name = @loginlang) and @loginlang <> N'us_english')
		select @loginlang = @@language
	exec sp_addlogin N'DMSReader', null, @logindb, @loginlang
END
GO

if not exists (select * from master.dbo.syslogins where loginname = N'DMSWebUser')
BEGIN
	declare @logindb nvarchar(132), @loginlang nvarchar(132) select @logindb = N'master', @loginlang = N'us_english'
	if @logindb is null or not exists (select * from master.dbo.sysdatabases where name = @logindb)
		select @logindb = N'master'
	if @loginlang is null or (not exists (select * from master.dbo.syslanguages where name = @loginlang) and @loginlang <> N'us_english')
		select @loginlang = @@language
	exec sp_addlogin N'DMSWebUser', null, @logindb, @loginlang
END
GO

if not exists (select * from master.dbo.syslogins where loginname = N'ERS_User')
BEGIN
	declare @logindb nvarchar(132), @loginlang nvarchar(132) select @logindb = N'DMS5', @loginlang = N'us_english'
	if @logindb is null or not exists (select * from master.dbo.sysdatabases where name = @logindb)
		select @logindb = N'master'
	if @loginlang is null or (not exists (select * from master.dbo.syslanguages where name = @loginlang) and @loginlang <> N'us_english')
		select @loginlang = @@language
	exec sp_addlogin N'ERS_User', null, @logindb, @loginlang
END
GO

if not exists (select * from master.dbo.syslogins where loginname = N'LCMSReader')
BEGIN
	declare @logindb nvarchar(132), @loginlang nvarchar(132) select @logindb = N'LCMSConfig', @loginlang = N'us_english'
	if @logindb is null or not exists (select * from master.dbo.sysdatabases where name = @logindb)
		select @logindb = N'master'
	if @loginlang is null or (not exists (select * from master.dbo.syslanguages where name = @loginlang) and @loginlang <> N'us_english')
		select @loginlang = @@language
	exec sp_addlogin N'LCMSReader', null, @logindb, @loginlang
END
GO

if not exists (select * from master.dbo.syslogins where loginname = N'LCMSUser')
BEGIN
	declare @logindb nvarchar(132), @loginlang nvarchar(132) select @logindb = N'LCMSConfig', @loginlang = N'us_english'
	if @logindb is null or not exists (select * from master.dbo.sysdatabases where name = @logindb)
		select @logindb = N'master'
	if @loginlang is null or (not exists (select * from master.dbo.syslanguages where name = @loginlang) and @loginlang <> N'us_english')
		select @loginlang = @@language
	exec sp_addlogin N'LCMSUser', null, @logindb, @loginlang
END
GO

if not exists (select * from master.dbo.syslogins where loginname = N'pnl\D3E383')
	exec sp_grantlogin N'pnl\D3E383'
	exec sp_defaultdb N'pnl\D3E383', N'Marge'
	exec sp_defaultlanguage N'pnl\D3E383', N'us_english'
GO

if not exists (select * from master.dbo.syslogins where loginname = N'pnl\D3J426')
	exec sp_grantlogin N'pnl\D3J426'
	exec sp_defaultdb N'pnl\D3J426', N'LCMSConfig'
	exec sp_defaultlanguage N'pnl\D3J426', N'us_english'
GO

if not exists (select * from master.dbo.syslogins where loginname = N'pnl\ditsjobmgr')
	exec sp_grantlogin N'pnl\ditsjobmgr'
	exec sp_defaultdb N'pnl\ditsjobmgr', N'DMS5'
	exec sp_defaultlanguage N'pnl\ditsjobmgr', N'us_english'
GO

if not exists (select * from dbo.sysusers where name = N'BUILTIN\Administrators' and uid < 16382)
	EXEC sp_grantdbaccess N'BUILTIN\Administrators', N'BUILTIN\Administrators'
GO

if not exists (select * from dbo.sysusers where name = N'DMSReader' and uid < 16382)
	EXEC sp_grantdbaccess N'DMSReader', N'DMSReader'
GO

if not exists (select * from dbo.sysusers where name = N'DMSWebUser' and uid < 16382)
	EXEC sp_grantdbaccess N'DMSWebUser', N'DMSWebUser'
GO

if not exists (select * from dbo.sysusers where name = N'ERS_User' and uid < 16382)
	EXEC sp_grantdbaccess N'ERS_User', N'ERS_User'
GO

if not exists (select * from dbo.sysusers where name = N'pnl\ditsjobmgr' and uid < 16382)
	EXEC sp_grantdbaccess N'pnl\ditsjobmgr', N'pnl\ditsjobmgr'
GO

if not exists (select * from dbo.sysusers where name = N'DMS_SP_User' and uid > 16399)
	EXEC sp_addrole N'DMS_SP_User'
GO

exec sp_addrolemember N'db_accessadmin', N'BUILTIN\Administrators'
GO

exec sp_addrolemember N'db_backupoperator', N'BUILTIN\Administrators'
GO

exec sp_addrolemember N'db_datareader', N'BUILTIN\Administrators'
GO

exec sp_addrolemember N'db_datareader', N'DMSReader'
GO

exec sp_addrolemember N'db_datareader', N'DMSWebUser'
GO

exec sp_addrolemember N'db_datareader', N'ERS_User'
GO

exec sp_addrolemember N'db_datareader', N'pnl\ditsjobmgr'
GO

exec sp_addrolemember N'db_datawriter', N'BUILTIN\Administrators'
GO

exec sp_addrolemember N'db_datawriter', N'DMSWebUser'
GO

exec sp_addrolemember N'db_ddladmin', N'BUILTIN\Administrators'
GO

exec sp_addrolemember N'db_denydatareader', N'BUILTIN\Administrators'
GO

exec sp_addrolemember N'db_denydatawriter', N'BUILTIN\Administrators'
GO

exec sp_addrolemember N'db_owner', N'BUILTIN\Administrators'
GO

exec sp_addrolemember N'db_securityadmin', N'BUILTIN\Administrators'
GO

exec sp_addrolemember N'DMS_SP_User', N'BUILTIN\Administrators'
GO

exec sp_addrolemember N'DMS_SP_User', N'DMSWebUser'
GO

exec sp_addrolemember N'DMS_SP_User', N'pnl\ditsjobmgr'
GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[T_Param_Entries]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[T_Param_Entries] (
	[Entry_Sequence_Order] [int] NULL ,
	[Entry_Type] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Entry_Specifier] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Entry_Value] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Entry_Parent_Type] [int] NULL ,
	[Param_File_ID] [int] NULL 
) ON [PRIMARY]
END

GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[T_Param_Entry_Types]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[T_Param_Entry_Types] (
	[Entry_Type_ID] [int] NULL ,
	[Entry_Name] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Entry_Description] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
END

GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[T_Param_File_Types]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[T_Param_File_Types] (
	[Param_File_Type_ID] [int] NOT NULL ,
	[Param_File_Type] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
END

GO

if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[T_Param_Files]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
 BEGIN
CREATE TABLE [dbo].[T_Param_Files] (
	[Param_File_ID] [int] NOT NULL ,
	[Param_File_Name] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Param_File_Description] [varchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Param_File_Type_ID] [int] NULL ,
	[Date_Created] [datetime] NULL ,
	[Date_Modified] [datetime] NULL 
) ON [PRIMARY]
END

GO

ALTER TABLE [dbo].[T_Param_File_Types] WITH NOCHECK ADD 
	CONSTRAINT [PK_T_Param_File_Types] PRIMARY KEY  CLUSTERED 
	(
		[Param_File_Type_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[T_Param_Files] WITH NOCHECK ADD 
	CONSTRAINT [PK_T_Param_Files] PRIMARY KEY  CLUSTERED 
	(
		[Param_File_ID]
	)  ON [PRIMARY] 
GO

 