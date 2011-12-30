IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[<dbUser,varchar,dbo>].[subtext_DropColumnCascading]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE [<dbUser,varchar,dbo>].[subtext_DropColumnCascading]
GO

/*
Following procedure adapted from a StackOverflow answer (http://stackoverflow.com/a/7251546/598) 
by Steve (http://stackoverflow.com/users/634027/steve).

Answer licensed under the Creative Commons Share Alike license http://creativecommons.org/licenses/by-sa/3.0/
*/
CREATE PROCEDURE [<dbUser,varchar,dbo>].[subtext_DropColumnCascading]
(
    @tablename nvarchar(500), 
    @columnname nvarchar(500)
)
AS
    SELECT CONSTRAINT_NAME, 'C' AS type
    INTO #dependencies
    FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE WHERE TABLE_NAME = @tablename AND COLUMN_NAME = @columnname

    INSERT INTO #dependencies
    select d.name, 'C'
    from sys.default_constraints d
    join sys.columns c ON c.column_id = d.parent_column_id AND c.object_id = d.parent_object_id
    join sys.objects o ON o.object_id = d.parent_object_id
    WHERE o.name = @tablename AND c.name = @columnname

    INSERT INTO #dependencies
    SELECT i.name, 'I'
    FROM sys.indexes i
    JOIN sys.index_columns ic ON ic.index_id = i.index_id and ic.object_id=i.object_id
    JOIN sys.columns c ON c.column_id = ic.column_id and c.object_id=i.object_id
    JOIN sys.objects o ON o.object_id = i.object_id
    where o.name = @tableName AND i.type=2 AND c.name = @columnname AND is_unique_constraint = 0

    INSERT INTO #dependencies
    SELECT s.NAME, 'S'
    FROM sys.stats AS s
    INNER JOIN sys.stats_columns AS sc 
        ON s.object_id = sc.object_id AND s.stats_id = sc.stats_id
    INNER JOIN sys.columns AS c 
        ON sc.object_id = c.object_id AND c.column_id = sc.column_id
    WHERE s.object_id = OBJECT_ID(@tableName)
    AND c.NAME = @columnname
    AND s.NAME LIKE '_dta_stat%'

    DECLARE @dep_name nvarchar(500)
    DECLARE @type nchar(1)

    DECLARE dep_cursor CURSOR
    FOR SELECT * FROM #dependencies

    OPEN dep_cursor

    FETCH NEXT FROM dep_cursor 
    INTO @dep_name, @type;

    DECLARE @sql nvarchar(max)

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @sql = 
            CASE @type
                WHEN 'C' THEN 'ALTER TABLE [' + @tablename + '] DROP CONSTRAINT [' + @dep_name + ']'
                WHEN 'I' THEN 'DROP INDEX [' + @dep_name + '] ON dbo.[' + @tablename + ']'
                WHEN 'S' THEN 'DROP STATISTICS [' + @tablename + '].[' + @dep_name + ']'
            END
        print @sql
        EXEC sp_executesql @sql
        FETCH NEXT FROM dep_cursor 
        INTO @dep_name, @type;
    END

    DEALLOCATE dep_cursor

    DROP TABLE #dependencies

    SET @sql = 'ALTER TABLE [' + @tablename + '] DROP COLUMN [' + @columnname + ']'

    print @sql
    EXEC sp_executesql @sql
GO	

/* Adds new UTC based date columns and converts all the old data into these new columns */
IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'TimeZoneOffset'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
    ADD [TimeZoneOffset] int NOT NULL CONSTRAINT DF_subtext_Config_TimeZoneOffset DEFAULT 0
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateModifiedUtc'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
    ADD [DateModifiedUtc] datetime NOT NULL CONSTRAINT DF_subtext_Config_DateModifiedUtc DEFAULT getutcdate()
END
GO

/* RESETS OLD COLUMNS TEMPORARILY */
IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'LastUpdated'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
    ADD [LastUpdated] datetime NULL
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Content' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateAdded'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content]
    ADD [DateAdded] datetime NULL
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Content' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateUpdated'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content]
    ADD [DateUpdated] datetime NULL
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Content' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateSyndicated'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content]
    ADD [DateSyndicated] datetime NULL
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Feedback' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreated'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Feedback]
    ADD [DateCreated] datetime NULL

    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Feedback] ADD CONSTRAINT
    [DF_subtext_Feedback_DateCreated] DEFAULT getdate() FOR DateCreated
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Feedback' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateModified'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Feedback]
    ADD [DateModified] datetime NULL

    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Feedback] ADD CONSTRAINT
    [DF_subtext_Feedback_DateModified] DEFAULT getdate() FOR DateModified
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Host' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreated'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Host]
    ADD [DateCreated] datetime NULL
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Version' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreated'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Version]
    ADD [DateCreated] datetime NULL
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_MetaTag' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreated'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_MetaTag]
    ADD [DateCreated] datetime NULL

    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_MetaTag] ADD CONSTRAINT
    [DF_subtext_MetaTag_DateCreated] DEFAULT getdate() FOR DateCreated

END
GO

/* ADDS NEW COLUMNS */
IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreatedUtc'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Config]
    ADD [DateCreatedUtc] datetime NOT NULL CONSTRAINT DF_subtext_Config_DateCreatedUtc DEFAULT getutcdate()
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Content' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreatedUtc'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content]
    ADD [DateCreatedUtc] datetime NOT NULL CONSTRAINT DF_subtext_Content_DateCreatedUtc DEFAULT getutcdate()
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Content' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateModifiedUtc'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content]
    ADD [DateModifiedUtc] datetime NOT NULL CONSTRAINT DF_subtext_Content_DateModifiedUtc DEFAULT getutcdate()
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Content' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DatePublishedUtc'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Content]
    ADD [DatePublishedUtc] datetime NULL
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Feedback' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreatedUtc'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Feedback]
    ADD [DateCreatedUtc] datetime NOT NULL CONSTRAINT DF_subtext_Feedback_DateCreatedUtc DEFAULT getutcdate()
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Feedback' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateModifiedUtc'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Feedback]
    ADD [DateModifiedUtc] datetime NOT NULL CONSTRAINT DF_subtext_Feedback_DateModifiedUtc DEFAULT getutcdate()
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Host' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreatedUtc'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Host]
    ADD [DateCreatedUtc] datetime NOT NULL CONSTRAINT DF_subtext_Host_DateCreatedUtc DEFAULT getutcdate()
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Version' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreatedUtc'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_Version]
    ADD [DateCreatedUtc] datetime NOT NULL CONSTRAINT DF_subtext_Version_DateCreatedUtc DEFAULT getutcdate()
END
GO

IF NOT EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_MetaTag' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreatedUtc'
)
BEGIN
    ALTER TABLE [<dbUser,varchar,dbo>].[subtext_MetaTag]
    ADD [DateCreatedUtc] datetime NOT NULL CONSTRAINT DF_subtext_MetaTag_DateCreatedUtc DEFAULT getutcdate()
END
GO

UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -12 WHERE  TimeZoneId = 'Dateline Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -11 WHERE  TimeZoneId = 'UTC-11'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -11 WHERE  TimeZoneId = 'Samoa Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -10 WHERE  TimeZoneId = 'Hawaiian Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -9 WHERE  TimeZoneId = 'Alaskan Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -8 WHERE  TimeZoneId = 'Pacific Standard Time (Mexico)'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -8 WHERE  TimeZoneId = 'Pacific Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -7 WHERE  TimeZoneId = 'US Mountain Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -7 WHERE  TimeZoneId = 'Mountain Standard Time (Mexico)'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -7 WHERE  TimeZoneId = 'Mountain Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -6 WHERE  TimeZoneId = 'Central America Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -6 WHERE  TimeZoneId = 'Central Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -6 WHERE  TimeZoneId = 'Central Standard Time (Mexico)'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -6 WHERE  TimeZoneId = 'Canada Central Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -5 WHERE  TimeZoneId = 'SA Pacific Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -5 WHERE  TimeZoneId = 'Eastern Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -5 WHERE  TimeZoneId = 'US Eastern Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -4 WHERE  TimeZoneId = 'Venezuela Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -4 WHERE  TimeZoneId = 'Paraguay Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -4 WHERE  TimeZoneId = 'Atlantic Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -4 WHERE  TimeZoneId = 'Central Brazilian Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -4 WHERE  TimeZoneId = 'SA Western Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -4 WHERE  TimeZoneId = 'Pacific SA Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -3 WHERE  TimeZoneId = 'Newfoundland Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -3 WHERE  TimeZoneId = 'E. South America Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -3 WHERE  TimeZoneId = 'Argentina Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -3 WHERE  TimeZoneId = 'SA Eastern Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -3 WHERE  TimeZoneId = 'Greenland Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -3 WHERE  TimeZoneId = 'Montevideo Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -2 WHERE  TimeZoneId = 'UTC-02'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -2 WHERE  TimeZoneId = 'Mid-Atlantic Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -1 WHERE  TimeZoneId = 'Azores Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = -1 WHERE  TimeZoneId = 'Cape Verde Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 0 WHERE  TimeZoneId = 'Morocco Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 0 WHERE  TimeZoneId = 'UTC'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 0 WHERE  TimeZoneId = 'GMT Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 0 WHERE  TimeZoneId = 'Greenwich Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 1 WHERE  TimeZoneId = 'W. Europe Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 1 WHERE  TimeZoneId = 'Central Europe Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 1 WHERE  TimeZoneId = 'Romance Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 1 WHERE  TimeZoneId = 'Central European Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 1 WHERE  TimeZoneId = 'W. Central Africa Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 1 WHERE  TimeZoneId = 'Namibia Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 2 WHERE  TimeZoneId = 'Jordan Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 2 WHERE  TimeZoneId = 'GTB Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 2 WHERE  TimeZoneId = 'Middle East Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 2 WHERE  TimeZoneId = 'Egypt Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 2 WHERE  TimeZoneId = 'Syria Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 2 WHERE  TimeZoneId = 'South Africa Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 2 WHERE  TimeZoneId = 'FLE Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 2 WHERE  TimeZoneId = 'Israel Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 2 WHERE  TimeZoneId = 'E. Europe Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 3 WHERE  TimeZoneId = 'Arabic Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 3 WHERE  TimeZoneId = 'Arab Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 3 WHERE  TimeZoneId = 'Russian Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 3 WHERE  TimeZoneId = 'E. Africa Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 3 WHERE  TimeZoneId = 'Iran Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 4 WHERE  TimeZoneId = 'Arabian Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 4 WHERE  TimeZoneId = 'Azerbaijan Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 4 WHERE  TimeZoneId = 'Mauritius Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 4 WHERE  TimeZoneId = 'Georgian Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 4 WHERE  TimeZoneId = 'Caucasus Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 4 WHERE  TimeZoneId = 'Afghanistan Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 5 WHERE  TimeZoneId = 'Ekaterinburg Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 5 WHERE  TimeZoneId = 'Pakistan Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 5 WHERE  TimeZoneId = 'West Asia Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 5 WHERE  TimeZoneId = 'India Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 5 WHERE  TimeZoneId = 'Sri Lanka Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 5 WHERE  TimeZoneId = 'Nepal Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 6 WHERE  TimeZoneId = 'Central Asia Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 6 WHERE  TimeZoneId = 'Bangladesh Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 6 WHERE  TimeZoneId = 'N. Central Asia Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 6 WHERE  TimeZoneId = 'Myanmar Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 7 WHERE  TimeZoneId = 'SE Asia Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 7 WHERE  TimeZoneId = 'North Asia Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 8 WHERE  TimeZoneId = 'China Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 8 WHERE  TimeZoneId = 'North Asia East Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 8 WHERE  TimeZoneId = 'Singapore Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 8 WHERE  TimeZoneId = 'W. Australia Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 8 WHERE  TimeZoneId = 'Taipei Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 8 WHERE  TimeZoneId = 'Ulaanbaatar Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 9 WHERE  TimeZoneId = 'Tokyo Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 9 WHERE  TimeZoneId = 'Korea Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 9 WHERE  TimeZoneId = 'Yakutsk Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 9 WHERE  TimeZoneId = 'Cen. Australia Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 9 WHERE  TimeZoneId = 'AUS Central Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 10 WHERE  TimeZoneId = 'E. Australia Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 10 WHERE  TimeZoneId = 'AUS Eastern Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 10 WHERE  TimeZoneId = 'West Pacific Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 10 WHERE  TimeZoneId = 'Tasmania Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 10 WHERE  TimeZoneId = 'Vladivostok Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 11 WHERE  TimeZoneId = 'Magadan Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 11 WHERE  TimeZoneId = 'Central Pacific Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 12 WHERE  TimeZoneId = 'New Zealand Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 12 WHERE  TimeZoneId = 'UTC+12'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 12 WHERE  TimeZoneId = 'Fiji Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 12 WHERE  TimeZoneId = 'Kamchatka Standard Time'
GO
UPDATE [<dbUser,varchar,dbo>].[subtext_Config] SET TimeZoneOffset = 13 WHERE  TimeZoneId = 'Tonga Standard Time'
GO

IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Content' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateAdded'
)
BEGIN
    UPDATE [subtext_Content] SET [subtext_Content].[DateCreatedUtc] = DateAdd(hh, cfg.[TimeZoneOffset], [DateAdded])
    FROM [subtext_Config] cfg INNER JOIN [subtext_Content] ON cfg.[BlogId] = [subtext_Content].[BlogId]
    WHERE [DateAdded] IS NOT NULL

    EXEC subtext_DropColumnCascading 'subtext_Content', 'DateAdded'
END
GO

IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Content' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateUpdated'
)
BEGIN
    UPDATE [subtext_Content] SET [subtext_Content].[DateModifiedUtc] = DateAdd(hh, cfg.[TimeZoneOffset], [DateUpdated])
    FROM [subtext_Config] cfg INNER JOIN [subtext_Content] ON cfg.[BlogId] = [subtext_Content].[BlogId]
    WHERE [DateUpdated] IS NOT NULL

    EXEC subtext_DropColumnCascading 'subtext_Content', 'DateUpdated'
END
GO

IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Content' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateSyndicated'
)
BEGIN
    UPDATE [subtext_Content] SET [DatePublishedUtc] = DateAdd(hh, cfg.[TimeZoneOffset], [DateSyndicated])
    FROM [subtext_Config] cfg INNER JOIN [subtext_Content] ON cfg.[BlogId] = [subtext_Content].[BlogId]
    WHERE [DateSyndicated] IS NOT NULL

    EXEC subtext_DropColumnCascading 'subtext_Content', 'DateSyndicated' 
END
GO

IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Feedback' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreated'
)
BEGIN
    UPDATE [subtext_Feedback] SET [subtext_Feedback].[DateCreatedUtc] = DateAdd(hh, cfg.[TimeZoneOffset], [DateCreated])
    FROM [subtext_Config] cfg INNER JOIN [subtext_Feedback] ON cfg.[BlogId] = [subtext_Feedback].[BlogId]
    WHERE [DateCreated] IS NOT NULL

    EXEC subtext_DropColumnCascading 'subtext_Feedback', 'DateCreated'
END
GO

IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Feedback' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateModified'
)
BEGIN
    UPDATE [subtext_Feedback] SET [subtext_Feedback].[DateModifiedUtc] = DateAdd(hh, cfg.[TimeZoneOffset], [DateModified])
    FROM [subtext_Config] cfg INNER JOIN [subtext_Feedback] ON cfg.[BlogId] = [subtext_Feedback].[BlogId]
    WHERE [DateModified] IS NOT NULL

    EXEC subtext_DropColumnCascading 'subtext_Feedback', 'DateModified'
END
GO

IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Host' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreated'
)
BEGIN
    EXEC subtext_DropColumnCascading 'subtext_Host', 'DateCreated'
END
GO

IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Version' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreated'
)
BEGIN
    EXEC subtext_DropColumnCascading 'subtext_Version', 'DateCreated'
END
GO

IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_MetaTag' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'DateCreated'
)
BEGIN
    UPDATE [subtext_MetaTag] SET [subtext_MetaTag].[DateCreatedUtc] = DateAdd(hh, cfg.[TimeZoneOffset], [DateCreated])
    FROM [subtext_Config] cfg INNER JOIN [subtext_MetaTag] ON cfg.[BlogId] = [subtext_MetaTag].[BlogId]
    WHERE [DateCreated] IS NOT NULL

    EXEC subtext_DropColumnCascading 'subtext_MetaTag', 'DateCreated'
END
GO

IF EXISTS 
(
    SELECT * FROM [INFORMATION_SCHEMA].[COLUMNS] 
    WHERE   TABLE_NAME = 'subtext_Config' 
    AND TABLE_SCHEMA = '<dbUser,varchar,dbo>'
    AND COLUMN_NAME = 'LastUpdated'
)
BEGIN
    UPDATE [<dbUser,varchar,dbo>].[subtext_Config] 
    SET [DateCreatedUtc] = ISNULL([LastUpdated], getutcdate())

    EXEC subtext_DropColumnCascading 'subtext_Config', 'LastUpdated'
END
GO
