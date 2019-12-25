CREATE TABLE [acc].[DimensionTable] (
    [Tid]       INT          IDENTITY (1, 1) NOT NULL,
    [TableName] VARCHAR (20) NULL,
    CONSTRAINT [PK_DimensionTable] PRIMARY KEY CLUSTERED ([Tid] ASC)
);

