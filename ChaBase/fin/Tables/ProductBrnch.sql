CREATE TABLE [fin].[ProductBrnch] (
    [PBId]      INT IDENTITY (1, 1) NOT NULL,
    [BrnchID]   INT NOT NULL,
    [PID]       INT NOT NULL,
    [StartAcNo] INT NULL,
    [LastAcNo]  INT NULL,
    CONSTRAINT [PK_ProductBrnch] PRIMARY KEY CLUSTERED ([PBId] ASC),
    CONSTRAINT [FK_BrnchProducts_ProductDetail] FOREIGN KEY ([PID]) REFERENCES [fin].[ProductDetail] ([PID])
);

