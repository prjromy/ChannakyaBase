CREATE TABLE [cust].[CustContactPerson] (
    [CPId]   INT           IDENTITY (1, 1) NOT NULL,
    [CID]    NUMERIC (10)  NOT NULL,
    [CPName] VARCHAR (100) NOT NULL,
    [CPCNo]  VARCHAR (50)  NOT NULL,
    PRIMARY KEY CLUSTERED ([CPId] ASC),
    CONSTRAINT [FK_CustContactPerson_CustInfo] FOREIGN KEY ([CID]) REFERENCES [cust].[CustInfo] ([CID])
);

