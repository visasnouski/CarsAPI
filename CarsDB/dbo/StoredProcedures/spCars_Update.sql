CREATE PROCEDURE [dbo].[spCars_Update]
@CarId INT, 
@TagNumber varchar(20), 
@Make varchar(20), 
@Model varchar(20),
@CarYear SMALLINT, 
@Category varchar(20),
@mp3layer bit, 
@DVDPlayer bit,
@AirConditioner bit,
@ABS bit,
@ASR bit, 
@Navigation bit, 
@Available bit
AS 
begin
UPDATE Cars Set TagNumber=@TagNumber, Make=@Make, Model=@Model, CarYear=@CarYear, Category=@Category, mp3layer=@mp3layer, DVDPlayer=@DVDPlayer, AirConditioner=@AirConditioner, ABS=@ABS, ASR=@ASR, Navigation=@Navigation, Available=@Available
where CarId=@CarId
end