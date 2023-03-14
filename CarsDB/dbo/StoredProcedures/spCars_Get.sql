CREATE PROCEDURE [dbo].[spCars_Get]
 @CarId int
AS 
begin
      select [CarId]
      ,[TagNumber]
      ,[Make]
      ,[Model]
      ,[CarYear]
      ,[Category]
      ,[mp3layer]
      ,[DVDPlayer]
      ,[AirConditioner]
      ,[ABS]
      ,[ASR]
      ,[Navigation]
      ,[Available] from dbo.[Cars]
      where [CarId]=@CarId;
end
