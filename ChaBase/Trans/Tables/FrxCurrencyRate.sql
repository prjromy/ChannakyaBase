CREATE TABLE [Trans].[FrxCurrencyRate] (
    [TmpId]     TINYINT IDENTITY (1, 1) NOT NULL,
    [CurrID]    INT     NOT NULL,
    [BSPerUnit] MONEY   NOT NULL,
    [Brate]     MONEY   NOT NULL,
    [Srate]     MONEY   NOT NULL,
    [Mrate]     AS      (([Brate]+[Srate])/(2)),
    CONSTRAINT [PK_FrxCurrencyRate] PRIMARY KEY CLUSTERED ([TmpId] ASC),
    CONSTRAINT [FK_FrxCurrencyRate_CurrencyType] FOREIGN KEY ([CurrID]) REFERENCES [fin].[CurrencyType] ([CTId])
);

