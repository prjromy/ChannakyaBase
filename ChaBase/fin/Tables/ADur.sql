CREATE TABLE [fin].[ADur] (
    [IAccno]     INT           NOT NULL,
    [Days]       REAL          NULL,
    [Month]      INT           NULL,
    [ValDat]     SMALLDATETIME NULL,
    [IsOD]       BIT           NOT NULL,
    [MatDat]     SMALLDATETIME NULL,
    [DurType]    INT           NULL,
    [DurationId] INT           NULL,
    CONSTRAINT [PK_ADur] PRIMARY KEY CLUSTERED ([IAccno] ASC),
    CONSTRAINT [FK_AccountDur_AccountDetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

