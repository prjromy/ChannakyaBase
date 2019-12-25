CREATE TABLE [fin].[CollTemp] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [RetId]       INT           NOT NULL,
    [IAccNo]      INT           NOT NULL,
    [FId]         INT           NOT NULL,
    [SType]       INT           NOT NULL,
    [Amount]      MONEY         NOT NULL,
    [Description] VARCHAR (400) NULL,
    CONSTRAINT [PK_CollTemp] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CollTemp_CollMainTemp] FOREIGN KEY ([RetId]) REFERENCES [fin].[CollMainTemp] ([RetId])
);

