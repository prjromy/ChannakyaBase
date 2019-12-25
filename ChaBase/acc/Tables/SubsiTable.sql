CREATE TABLE [acc].[SubsiTable] (
    [STBId]     INT          IDENTITY (1, 1) NOT NULL,
    [TableName] VARCHAR (50) NOT NULL,
    [NeedMap]   BIT          NULL,
    [IsActive]  BIT          NULL,
    CONSTRAINT [PK_SubsiTable] PRIMARY KEY CLUSTERED ([STBId] ASC)
);

