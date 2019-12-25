CREATE TABLE [fin].[ChqSize] (
    [Id]     INT           IDENTITY (1, 1) NOT NULL,
    [Height] NVARCHAR (50) NULL,
    [Width]  NVARCHAR (50) NULL,
    CONSTRAINT [PK_ChqSize] PRIMARY KEY CLUSTERED ([Id] ASC)
);

