CREATE TABLE [cust].[CustIRegContact] (
    [IRCId] INT          IDENTITY (1, 1) NOT NULL,
    [CCID]  INT          NOT NULL,
    [CID]   NUMERIC (10) NOT NULL,
    CONSTRAINT [PK__CustIReg__04C7D03B4A6EAE91] PRIMARY KEY CLUSTERED ([IRCId] ASC),
    CONSTRAINT [FK_CustIRegContact_CustContact1] FOREIGN KEY ([CCID]) REFERENCES [cust].[CustContact] ([CCID]),
    CONSTRAINT [FK_CustIRegContact_CustInfo] FOREIGN KEY ([CID]) REFERENCES [cust].[CustInfo] ([CID])
);

