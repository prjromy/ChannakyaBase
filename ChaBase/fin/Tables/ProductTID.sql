CREATE TABLE [fin].[ProductTID] (
    [PFIID]     INT      IDENTITY (1, 1) NOT NULL,
    [PID]       INT      NOT NULL,
    [TID]       TINYINT  NOT NULL,
    [UpdatedOn] DATETIME NULL,
    [UpdatedBy] INT      NULL,
    [Flag]      BIT      NULL,
    CONSTRAINT [PK__ProductT__3F887DF85337ADAD] PRIMARY KEY CLUSTERED ([PFIID] ASC),
    CONSTRAINT [FK_ProductTID_ProductDetail] FOREIGN KEY ([PID]) REFERENCES [fin].[ProductDetail] ([PID]),
    CONSTRAINT [FK_ProductTID_TempIntRate] FOREIGN KEY ([TID]) REFERENCES [fin].[TempIntRate] ([TID])
);

