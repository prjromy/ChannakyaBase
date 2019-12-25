CREATE TABLE [fin].[TempIntRateVal] (
    [TIRID] INT     IDENTITY (1, 1) NOT NULL,
    [TID]   TINYINT NOT NULL,
    [IRate] REAL    NOT NULL,
    [ULAmt] MONEY   NOT NULL,
    PRIMARY KEY CLUSTERED ([TIRID] ASC),
    CONSTRAINT [FK_tempIntRateVal_tempIntRate] FOREIGN KEY ([TID]) REFERENCES [fin].[TempIntRate] ([TID])
);

