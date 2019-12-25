CREATE TABLE [cust].[CustIndividual] (
    [CID]         NUMERIC (10)   NOT NULL,
    [Fname]       NVARCHAR (200) NOT NULL,
    [Mname]       NVARCHAR (200) NULL,
    [Lname]       NVARCHAR (200) NOT NULL,
    [Gender]      TINYINT        NOT NULL,
    [Occpn]       SMALLINT       NOT NULL,
    [GFatherName] NVARCHAR (300) NOT NULL,
    [FatherName]  NVARCHAR (300) NOT NULL,
    [SpouseName]  NVARCHAR (300) NULL,
    CONSTRAINT [PK_CustIndividual] PRIMARY KEY CLUSTERED ([CID] ASC),
    CONSTRAINT [FK_CustIndividual_CustInfo] FOREIGN KEY ([CID]) REFERENCES [cust].[CustInfo] ([CID]),
    CONSTRAINT [FK_CustIndividual_OccupationDef] FOREIGN KEY ([Occpn]) REFERENCES [cust].[OccupationDef] ([Occpn])
);

