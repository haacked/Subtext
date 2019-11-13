/*
The CategoryType column is defined in an enum and 
not in the database. Unfortunately it defined "LinkCollection" 
to have the value 0.  However that value should be reserved for 
"None" as we need that.
*/
UPDATE [<dbUser,varchar,dbo>].[subtext_LinkCategories] SET CategoryType = 5 WHERE CategoryType = 0
GO
/*
This was a unique constraint missing on the subtext_URLs table that 
should be there.  We could not use the information_schema views because 
indexes are not represented by those views.
*/
CREATE TABLE #Indexes
(
	index_name nvarchar(100)
	, index_description nvarchar(256)
	, index_key nvarchar(256)
)
INSERT INTO #Indexes
	EXEC sp_helpindex '<dbUser,varchar,dbo>.subtext_URLs'

IF NOT EXISTS(SELECT * FROM #Indexes WHERE index_name = 'IX_subtext_URLs__Url')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX IX_subtext_URLs__Url ON [<dbUser,varchar,dbo>].[subtext_URLs]
		(
			URL
		) ON [PRIMARY]
END
DROP TABLE #Indexes
GO

/*
This was a unique constraint missing on the subtext_Referrals table that 
should be there.  We could not use the information_schema views because 
indexes are not represented by those views.
*/
CREATE TABLE #Indexes
(
	index_name nvarchar(100)
	, index_description nvarchar(256)
	, index_key nvarchar(256)
)
INSERT INTO #Indexes
	EXEC sp_helpindex '<dbUser,varchar,dbo>.subtext_Referrals'

IF NOT EXISTS(SELECT * FROM #Indexes WHERE index_name = 'IX_subtext_Referrals__EntryId_BlogId_UrlID')
BEGIN
	CREATE UNIQUE NONCLUSTERED INDEX IX_subtext_Referrals__EntryId_BlogId_UrlID ON [<dbUser,varchar,dbo>].[subtext_Referrals]
		(
			EntryID,
			BlogId,
			UrlID
		) ON [PRIMARY]
END
DROP TABLE #Indexes
GO
/* Missing a foreign key from subtext_Referrals.EntryId to subtext_Content.EntryID */
/*
First we need to cleanup bad referrals.
*/
DELETE [<dbUser,varchar,dbo>].[subtext_Referrals] WHERE EntryID NOT IN (SELECT [ID] FROM [<dbUser,varchar,dbo>].[subtext_Content])

IF NOT EXISTS(
    SELECT * 
    FROM [INFORMATION_SCHEMA].[REFERENTIAL_CONSTRAINTS] 
    WHERE CONSTRAINT_NAME = 'FK_subtext_Referrals_subtext_Content' 
      AND CONSTRAINT_SCHEMA = '<dbUser,varchar,dbo>'
)
BEGIN
  ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Referrals] WITH NOCHECK 
  ADD CONSTRAINT FK_subtext_Referrals_subtext_Content FOREIGN KEY
  (
    EntryID
  ) REFERENCES [<dbUser,varchar,dbo>].[subtext_Content]
  (
    Id
  )
END
GO