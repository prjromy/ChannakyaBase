CREATE TABLE [cust].[CertificateDef] (
    [CCCertID] TINYINT       IDENTITY (1, 1) NOT NULL,
    [CCCert]   VARCHAR (150) NOT NULL,
    CONSTRAINT [PK_CustCertificateDef] PRIMARY KEY CLUSTERED ([CCCertID] ASC)
);

