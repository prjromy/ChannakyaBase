CREATE TABLE [fin].[AOfCust] (
    [AOCId]  INT          IDENTITY (1, 1) NOT NULL,
    [IAccno] INT          NOT NULL,
    [CID]    NUMERIC (10) NOT NULL,
    [Sno]    TINYINT      NULL,
    CONSTRAINT [PK_AOfCust] PRIMARY KEY CLUSTERED ([AOCId] ASC),
    CONSTRAINT [FK_AOfCust_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno]),
    CONSTRAINT [FK_AOfCust_CustInfo] FOREIGN KEY ([CID]) REFERENCES [cust].[CustInfo] ([CID])
);

