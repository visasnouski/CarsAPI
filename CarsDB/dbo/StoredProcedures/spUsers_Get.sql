CREATE PROCEDURE [dbo].[spUsers_Get]
 @UserName NVARCHAR(256)
AS 
begin
      select 
      [UserName],
      [PasswordHash],
      [PasswordSalt]
      from dbo.[Users]
      where [UserName]=@UserName;
end
