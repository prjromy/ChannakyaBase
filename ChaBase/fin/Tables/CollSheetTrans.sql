CREATE TABLE [fin].[CollSheetTrans] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [CollSheetId] BIGINT        NULL,
    [IAccNo]      INT           NOT NULL,
    [TNo]         BIGINT        NOT NULL,
    [SType]       INT           NOT NULL,
    [Amount]      MONEY         NOT NULL,
    [Description] VARCHAR (400) NULL,
    CONSTRAINT [PK_CollSheetTrans] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CollSheetTrans_CollSheet] FOREIGN KEY ([CollSheetId]) REFERENCES [fin].[CollSheet] ([CollSheetId])
);

