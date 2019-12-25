CREATE TABLE [fin].[ProductICBDur] (
    [PICBDurID] INT     IDENTITY (1, 1) NOT NULL,
    [PID]       INT     NOT NULL,
    [ICBDurID]  TINYINT NOT NULL,
    [IRate]     REAL    NULL,
    [IsDefault] BIT     NOT NULL,
    CONSTRAINT [PK_ProductICBDur] PRIMARY KEY CLUSTERED ([PICBDurID] ASC),
    CONSTRAINT [FK_ProductICBDur_ProductDetail] FOREIGN KEY ([PID]) REFERENCES [fin].[ProductDetail] ([PID]),
    CONSTRAINT [FK_ProductICBDur_RuleICBDuration] FOREIGN KEY ([ICBDurID]) REFERENCES [fin].[RuleICBDuration] ([ICBDurID])
);

