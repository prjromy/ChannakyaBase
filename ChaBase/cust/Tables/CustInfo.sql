CREATE TABLE [cust].[CustInfo] (
    [CID]          NUMERIC (10)   IDENTITY (1, 1) NOT NULL,
    [CtypeID]      TINYINT        NOT NULL,
    [CDepSector]   SMALLINT       NOT NULL,
    [CRegdate]     SMALLDATETIME  NOT NULL,
    [CCCertID]     TINYINT        NOT NULL,
    [CCCertno]     NVARCHAR (50)  NOT NULL,
    [IsIncomplete] BIT            NULL,
    [DoB]          DATETIME       NULL,
    [Postedby]     INT            NULL,
    [Appby]        INT            NULL,
    [PANNo]        NVARCHAR (50)  NULL,
    [Street]       NVARCHAR (150) NULL,
    CONSTRAINT [PK_CustInfo] PRIMARY KEY CLUSTERED ([CID] ASC),
    CONSTRAINT [FK_CustInfo_CertificateDef] FOREIGN KEY ([CCCertID]) REFERENCES [cust].[CertificateDef] ([CCCertID]),
    CONSTRAINT [FK_CustInfo_CustInfo] FOREIGN KEY ([CID]) REFERENCES [cust].[CustInfo] ([CID]),
    CONSTRAINT [FK_CustInfo_CustType] FOREIGN KEY ([CtypeID]) REFERENCES [cust].[CustType] ([CtypeID]),
    CONSTRAINT [FK_CustInfo_SectorDef] FOREIGN KEY ([CDepSector]) REFERENCES [fin].[SectorDef] ([CDepSector])
);

