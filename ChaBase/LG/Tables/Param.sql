CREATE TABLE [LG].[Param] (
    [PId]      INT            IDENTITY (1, 1) NOT NULL,
    [PName]    NVARCHAR (100) NOT NULL,
    [ParentId] INT            NOT NULL,
    [IsGroup]  BIT            NOT NULL,
    CONSTRAINT [PK_LG.Param] PRIMARY KEY CLUSTERED ([PId] ASC)
);

