CREATE TABLE [cust].[CustTypeContact] (
    [CTCntID]  INT     IDENTITY (1, 1) NOT NULL,
    [CTypeID]  TINYINT NOT NULL,
    [CNoType]  TINYINT NOT NULL,
    [CNoState] TINYINT NOT NULL,
    PRIMARY KEY CLUSTERED ([CTCntID] ASC),
    CONSTRAINT [FK_CustTypeContact_ContactDef] FOREIGN KEY ([CNoType]) REFERENCES [cust].[ContactDef] ([CNotype]),
    CONSTRAINT [FK_CustTypeContact_CustType] FOREIGN KEY ([CTypeID]) REFERENCES [cust].[CustType] ([CtypeID])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1-Mandatory; 2-Compulsory', @level0type = N'SCHEMA', @level0name = N'cust', @level1type = N'TABLE', @level1name = N'CustTypeContact', @level2type = N'COLUMN', @level2name = N'CTypeID';

