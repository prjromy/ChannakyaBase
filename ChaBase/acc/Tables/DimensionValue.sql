CREATE TABLE [acc].[DimensionValue] (
    [DVId]           INT          IDENTITY (1, 1) NOT NULL,
    [DDId]           INT          NOT NULL,
    [DimensionValue] VARCHAR (50) NOT NULL,
    [CodeNo]         VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_DimensionN] PRIMARY KEY CLUSTERED ([DVId] ASC),
    CONSTRAINT [FK_DimensionValue_DimensionDefination] FOREIGN KEY ([DDId]) REFERENCES [acc].[DimensionDefination] ([DDId])
);

