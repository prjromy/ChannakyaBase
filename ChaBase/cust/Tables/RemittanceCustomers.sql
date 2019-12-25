CREATE TABLE [cust].[RemittanceCustomers] (
    [RID] INT          IDENTITY (1, 1) NOT NULL,
    [CID] NUMERIC (10) NOT NULL,
    [FID] INT          NOT NULL,
    CONSTRAINT [PK_RemittanceCustomers] PRIMARY KEY CLUSTERED ([RID] ASC),
    CONSTRAINT [FK_RemittanceCustomers_CustCompany] FOREIGN KEY ([CID]) REFERENCES [cust].[CustCompany] ([CID]),
    CONSTRAINT [FK_RemittanceCustomers_FinAcc] FOREIGN KEY ([FID]) REFERENCES [acc].[FinAcc] ([Fid])
);

