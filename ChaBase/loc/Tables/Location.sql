CREATE TABLE [loc].[Location] (
    [LId]          INT          IDENTITY (1, 1) NOT NULL,
    [LocationName] VARCHAR (50) NOT NULL,
    [PLId]         INT          NULL,
    [Lvl]          TINYINT      NOT NULL,
    [IsGroup]      BIT          NOT NULL,
    CONSTRAINT [PK_Location_LId] PRIMARY KEY CLUSTERED ([LId] ASC)
);

