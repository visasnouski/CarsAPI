CREATE PROCEDURE [dbo].[spCars_Delete]
 @CarId int
AS 
begin 
	delete
	from dbo.Cars
	where CarId=@CarId;
end;

