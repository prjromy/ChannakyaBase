CREATE TABLE [fin].[ADrLimit] (
    [IAccno]  INT   NOT NULL,
    [QuotAmt] MONEY NULL,
    [AppAmt]  MONEY NOT NULL,
    CONSTRAINT [PK_ADrLimit] PRIMARY KEY CLUSTERED ([IAccno] ASC),
    CONSTRAINT [FK_ADrLimit_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

