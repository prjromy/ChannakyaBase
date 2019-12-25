CREATE TABLE [fin].[ProductPay] (
    [PPId]      INT     IDENTITY (1, 1) NOT NULL,
    [PID]       INT     NOT NULL,
    [PAYID]     TINYINT NOT NULL,
    [IsDefault] BIT     NOT NULL,
    CONSTRAINT [PK_ProductPay] PRIMARY KEY CLUSTERED ([PPId] ASC),
    CONSTRAINT [FK_ProductPay_ProductDetail] FOREIGN KEY ([PID]) REFERENCES [fin].[ProductDetail] ([PID]),
    CONSTRAINT [FK_ProductPay_RulePay] FOREIGN KEY ([PAYID]) REFERENCES [fin].[RulePay] ([PAYID]) ON UPDATE CASCADE
);

