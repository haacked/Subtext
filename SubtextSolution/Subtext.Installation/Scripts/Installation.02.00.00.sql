/*Add the Mail to weblog parameters to subtext_config - GY*/
IF NOT EXISTS 
	(
		SELECT	* FROM [information_schema].[columns] 
		WHERE	table_name = 'subtext_Config' 
		AND		table_schema = '<dbUser,varchar,dbo>'
		AND		column_name = 'pop3User' 
		AND		column_name = 'pop3Pass'
		AND		column_name = 'pop3Server'
		AND		column_name = 'pop3StartTag'
		AND		column_name = 'pop3EndTag'
		AND		column_name = 'pop3SubjectPrefix'
		AND		column_name = 'pop3MTBEnable'
		AND		column_name = 'pop3DeleteOnlyProcessed'
		AND		column_name = 'pop3InlineAttachedPictures'
		AND		column_name = 'pop3HeightForThumbs'
	)
	
	ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config] WITH NOCHECK
	ADD [pop3User] [varchar](32) NULL
		,[pop3Pass] [varchar] (32) NULL
		,[pop3Server] [varchar] (56) NULL
		,[pop3StartTag] [varchar] (10) NULL
		,[pop3EndTag] [varchar] (10) NULL
		,[pop3SubjectPrefix] [nvarchar] (20) NULL
		,[pop3MTBEnable] [bit] NULL
		,[pop3DeleteOnlyProcessed] [bit] NULL
		,[pop3InlineAttachedPictures] [bit] NULL
		,[pop3HeightForThumbs] [int] NULL 

GO