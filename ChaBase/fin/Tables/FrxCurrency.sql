CREATE TABLE [fin].[FrxCurrency] (
    [CurrID]   SMALLINT      NOT NULL,
    [FID]      INT           NOT NULL,
    [Currency] NVARCHAR (20) NOT NULL,
    [Symbol]   CHAR (4)      NOT NULL,
    [Country]  NVARCHAR (30) NOT NULL,
    CONSTRAINT [PK_FrxCurrency] PRIMARY KEY CLUSTERED ([CurrID] ASC),
    CONSTRAINT [IX_FrxCurrency] UNIQUE NONCLUSTERED ([CurrID] ASC)
);

