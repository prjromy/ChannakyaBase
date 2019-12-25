CREATE TABLE [dbo].[ADrlimitLog] (
    [Id]       INT      IDENTITY (1, 1) NOT NULL,
    [IAccno]   INT      NOT NULL,
    [Amount]   MONEY    NOT NULL,
    [PostedBy] INT      NOT NULL,
    [PostedOn] DATETIME NOT NULL,
    CONSTRAINT [PK_ADrlimitLog] PRIMARY KEY CLUSTERED ([Id] ASC)
);

