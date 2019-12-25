CREATE TABLE [fin].[AIntExpenses] (
    [Iaccno] INT             NOT NULL,
    [Tdate]  SMALLDATETIME   NOT NULL,
    [IntAmt] NUMERIC (19, 2) NOT NULL,
    [valued] TINYINT         NOT NULL,
    [VNo]    INT             NULL,
    CONSTRAINT [FK_AIntExpenses_ADetail] FOREIGN KEY ([Iaccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

