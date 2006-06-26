
/*
The CategoryType column is defined in an enum and 
not in the database. Unfortunately it defined "LinkCollection" 
to have the value 0.  However that value should be reserved for 
"None" as we need that.
*/
UPDATE [<dbUser,varchar,dbo>].[subtext_LinkCategories] SET CategoryType = 5 WHERE CategoryType = 0