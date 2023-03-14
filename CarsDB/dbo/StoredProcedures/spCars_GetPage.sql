CREATE PROCEDURE [dbo].[spCars_GetPage]
 @Offset int,
 @Limit int
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
      ORDER BY [CarId] 
      OFFSET @Offset ROWS
	  FETCH NEXT @Limit ROWS ONLY
end
