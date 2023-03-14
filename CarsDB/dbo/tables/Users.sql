CREATE TABLE [dbo].[Users]
(
    [UserName] NVARCHAR(256) NOT NULL PRIMARY KEY, 
    [PasswordHash] varbinary(max) NOT NULL, 
    [PasswordSalt] varbinary(max) NOT NULL
)
