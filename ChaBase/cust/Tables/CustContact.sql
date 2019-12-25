CREATE TABLE [cust].[CustContact] (
    [CCID]    INT            IDENTITY (1, 1) NOT NULL,
    [CID]     NUMERIC (10)   NOT NULL,
    [CNotype] TINYINT        NOT NULL,
    [CNo]     NVARCHAR (300) NOT NULL,
    CONSTRAINT [PK_CustContact] PRIMARY KEY CLUSTERED ([CCID] ASC),
    CONSTRAINT [FK_CustContact_ContactDef] FOREIGN KEY ([CNotype]) REFERENCES [cust].[ContactDef] ([CNotype]),
    CONSTRAINT [FK_CustContact_CustInfo] FOREIGN KEY ([CID]) REFERENCES [cust].[CustInfo] ([CID])
);

