CREATE TABLE [cust].[CustTypeCertificate] (
    [CTCID]    INT     IDENTITY (1, 1) NOT NULL,
    [CTypeID]  TINYINT NOT NULL,
    [CCCertID] TINYINT NOT NULL,
    [CCState]  TINYINT NOT NULL,
    PRIMARY KEY CLUSTERED ([CTCID] ASC),
    CONSTRAINT [FK_CustTypeCertificate_CertificateDef] FOREIGN KEY ([CCCertID]) REFERENCES [cust].[CertificateDef] ([CCCertID]),
    CONSTRAINT [FK_CustTypeCertificate_CustType] FOREIGN KEY ([CTypeID]) REFERENCES [cust].[CustType] ([CtypeID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Mandatory; 2-Compulsory; 3- ID', @level0type = N'SCHEMA', @level0name = N'cust', @level1type = N'TABLE', @level1name = N'CustTypeCertificate', @level2type = N'COLUMN', @level2name = N'CTypeID';

