CREATE TABLE [LG].[Religion] (
    [RId]          INT            IDENTITY (1, 1) NOT NULL,
    [ReligionName] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_LG.Religion] PRIMARY KEY CLUSTERED ([RId] ASC)
);

