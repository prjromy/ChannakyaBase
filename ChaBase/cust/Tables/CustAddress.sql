CREATE TABLE [cust].[CustAddress] (
    [CAId]     INT           IDENTITY (1, 1) NOT NULL,
    [CID]      NUMERIC (10)  NULL,
    [LId]      INT           NULL,
    [BlockNo]  VARCHAR (50)  NULL,
    [StreetNo] VARCHAR (250) NULL,
    [OName]    VARCHAR (250) NULL,
    [ATypeId]  INT           NULL,
    CONSTRAINT [PK_CustAddress] PRIMARY KEY CLUSTERED ([CAId] ASC),
    CONSTRAINT [FK_CustAddress_CustInfo] FOREIGN KEY ([CID]) REFERENCES [cust].[CustInfo] ([CID]),
    CONSTRAINT [FK_CustAddress_Location] FOREIGN KEY ([LId]) REFERENCES [loc].[Location] ([LId]),
    CONSTRAINT [FK_CustAddress_LocationTypeDef] FOREIGN KEY ([ATypeId]) REFERENCES [loc].[LocationTypeDef] ([ATypeId])
);

