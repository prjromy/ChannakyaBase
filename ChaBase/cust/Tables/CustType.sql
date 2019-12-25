CREATE TABLE [cust].[CustType] (
    [CtypeID] TINYINT       IDENTITY (1, 1) NOT NULL,
    [Ctype]   NVARCHAR (35) NOT NULL,
    [TaxID]   TINYINT       NOT NULL,
    [isind]   TINYINT       CONSTRAINT [DF_CustType_isind] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_CUSTTYPE] PRIMARY KEY CLUSTERED ([CtypeID] ASC),
    CONSTRAINT [FK_CustType_TaxsetupDef] FOREIGN KEY ([TaxID]) REFERENCES [fin].[TaxsetupDef] ([TaxID])
);

