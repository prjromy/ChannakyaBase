CREATE TABLE [cust].[CustCertificate] (
    [CCertID]  INT          IDENTITY (1, 1) NOT NULL,
    [CID]      NUMERIC (10) NOT NULL,
    [CCCertID] TINYINT      NOT NULL,
    CONSTRAINT [PK_CustCertificate] PRIMARY KEY CLUSTERED ([CCertID] ASC),
    CONSTRAINT [FK_CustCertificate_CustInfo] FOREIGN KEY ([CID]) REFERENCES [cust].[CustInfo] ([CID])
);

