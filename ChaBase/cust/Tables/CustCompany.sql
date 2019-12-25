CREATE TABLE [cust].[CustCompany] (
    [CID]       NUMERIC (10)  NOT NULL,
    [CCName]    VARCHAR (500) NOT NULL,
    [CCGroupID] SMALLINT      NULL,
    [CCPerson]  VARCHAR (500) NULL,
    [CCno]      VARCHAR (500) NULL,
    CONSTRAINT [PK_CustCompany] PRIMARY KEY CLUSTERED ([CID] ASC),
    CONSTRAINT [FK_CustCompany_CustCompGroup] FOREIGN KEY ([CCGroupID]) REFERENCES [cust].[CustCompGroup] ([CCGroupID]),
    CONSTRAINT [FK_CustCompany_CustInfo] FOREIGN KEY ([CID]) REFERENCES [cust].[CustInfo] ([CID])
);

