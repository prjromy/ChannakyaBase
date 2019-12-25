CREATE TABLE [acc].[DimensionDefination] (
    [DDId]     INT          IDENTITY (1, 1) NOT NULL,
    [DefName]  VARCHAR (50) NOT NULL,
    [IsManual] TINYINT      NULL,
    [TId]      INT          NULL,
    CONSTRAINT [PK_Dimension1] PRIMARY KEY CLUSTERED ([DDId] ASC),
    CONSTRAINT [FK_DimensionDefination_DimensionTable] FOREIGN KEY ([TId]) REFERENCES [acc].[DimensionTable] ([Tid])
);

