CREATE TABLE [fin].[TaxsetupDef] (
    [TaxID]   TINYINT       IDENTITY (1, 1) NOT NULL,
    [TaxName] NVARCHAR (30) NOT NULL,
    [TaxRate] REAL          NOT NULL,
    CONSTRAINT [PK_Taxsetup] PRIMARY KEY CLUSTERED ([TaxID] ASC),
    CONSTRAINT [IX_CustTaxsetup] UNIQUE NONCLUSTERED ([TaxID] ASC, [TaxName] ASC)
);

