CREATE PROCEDURE [dbo].[spCars_Insert]
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
INSERT into dbo.Cars (CarId, TagNumber, Make, Model, CarYear, Category, mp3layer, DVDPlayer, AirConditioner, ABS, ASR, Navigation, Available) 
VALUES (@CarId, @TagNumber, @Make, @Model, @CarYear, @Category, @mp3layer, @DVDPlayer, @AirConditioner, @ABS, @ASR, @Navigation, @Available)
end;
