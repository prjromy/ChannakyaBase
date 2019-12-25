CREATE TABLE [fin].[IntLog] (
    [Tdate]   SMALLDATETIME NOT NULL,
    [IAccno]  INT           NOT NULL,
    [Fday]    TINYINT       NOT NULL,
    [Balance] MONEY         NOT NULL,
    [Rate]    REAL          NOT NULL,
    [Intcal]  AS            (round((([balance]*[fday])*[rate])/(36500),(2))),
    [Valued]  TINYINT       NOT NULL,
    CONSTRAINT [FK_IntLog_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

