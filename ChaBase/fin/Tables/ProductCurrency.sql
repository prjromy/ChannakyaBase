CREATE TABLE [fin].[ProductCurrency] (
    [PID]     INT      NOT NULL,
    [BrchID]  SMALLINT NOT NULL,
    [CurrID]  SMALLINT NOT NULL,
    [DefCurr] BIT      NOT NULL,
    CONSTRAINT [FK_ProductCurrency_ProductDetail] FOREIGN KEY ([PID]) REFERENCES [fin].[ProductDetail] ([PID])
);

