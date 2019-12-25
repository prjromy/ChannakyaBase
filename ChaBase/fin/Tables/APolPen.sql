CREATE TABLE [fin].[APolPen] (
    [IAccno] INT     NOT NULL,
    [PCID]   TINYINT NOT NULL,
    CONSTRAINT [PK_APolPen] PRIMARY KEY CLUSTERED ([IAccno] ASC),
    CONSTRAINT [FK_AccountPolPen_AccountDetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

