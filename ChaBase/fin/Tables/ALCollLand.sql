CREATE TABLE [fin].[ALCollLand] (
    [AcolLandId] INT            IDENTITY (1, 1) NOT NULL,
    [Sno]        INT            NOT NULL,
    [Fname]      NVARCHAR (500) NULL,
    [Gname]      NVARCHAR (500) NULL,
    [Location]   NVARCHAR (500) NULL,
    [Area]       NVARCHAR (500) NULL,
    [Direction]  NVARCHAR (500) NULL,
    [Seatno]     NVARCHAR (500) NULL,
    [Kittano]    NVARCHAR (500) NULL,
    [BArea]      NVARCHAR (500) NULL,
    CONSTRAINT [PK_ALCollLand] PRIMARY KEY CLUSTERED ([AcolLandId] ASC),
    CONSTRAINT [FK_ALCollLand_ALColl] FOREIGN KEY ([Sno]) REFERENCES [fin].[ALColl] ([Sno])
);

