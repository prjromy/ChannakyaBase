CREATE TABLE [fin].[ANominee] (
    [NID]            INT           IDENTITY (1, 1) NOT NULL,
    [IAccno]         INT           NOT NULL,
    [NomNam]         VARCHAR (100) NOT NULL,
    [NomRel]         VARCHAR (100) NOT NULL,
    [CCertID]        TINYINT       NULL,
    [CCertno]        VARCHAR (25)  NULL,
    [Share]          REAL          NOT NULL,
    [ContactNo]      VARCHAR (50)  NULL,
    [ContactAddress] VARCHAR (50)  NULL,
    CONSTRAINT [PK_ANominee] PRIMARY KEY CLUSTERED ([NID] ASC),
    CONSTRAINT [FK_ANominee_ADetail] FOREIGN KEY ([CCertID]) REFERENCES [cust].[CertificateDef] ([CCCertID]),
    CONSTRAINT [FK_ANominee_ANominee] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

