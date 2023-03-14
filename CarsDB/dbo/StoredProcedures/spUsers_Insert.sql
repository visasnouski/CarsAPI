CREATE PROCEDURE [dbo].[spUsers_Insert]
 @UserName NVARCHAR(256),
 @PasswordHash varbinary(max),
 @PasswordSalt varbinary(max)
AS 
begin
     INSERT into dbo.Users ([UserName],[PasswordHash],[PasswordSalt])
      VALUES (@UserName, @PasswordHash, @PasswordSalt);
end
