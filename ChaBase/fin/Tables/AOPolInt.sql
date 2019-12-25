CREATE TABLE [fin].[AOPolInt] (
    [IAccno] INT     NOT NULL,
    [OPSID]  TINYINT NOT NULL,
    CONSTRAINT [PK_AOPolInt] PRIMARY KEY CLUSTERED ([IAccno] ASC),
    CONSTRAINT [FK_AccountOPolInt_AccountDetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

