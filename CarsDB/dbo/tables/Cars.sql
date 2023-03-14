CREATE TABLE [dbo].[Cars]
(
[CarId] INT NOT NULL PRIMARY KEY,
[TagNumber] [varchar](20) NULL,
[Make] [varchar](50) NULL,
[Model] [varchar](50) NOT NULL,
[CarYear] [SMALLINT] NULL,
[Category] [varchar](50) NULL,
[mp3layer] [BIT] NULL,
[DVDPlayer] [BIT] NULL,
[AirConditioner] [BIT] NULL,
[ABS] [BIT] NULL,
[ASR] [BIT] NULL,
[Navigation] [BIT] NULL,
[Available] [BIT] NULL,
)
