CREATE TABLE [fin].[RemitDeposit] (
    [RemitDepositId]  INT             IDENTITY (1, 1) NOT NULL,
    [Tno]             NUMERIC (18)    NOT NULL,
    [SenderName]      VARCHAR (250)   NOT NULL,
    [SenderContact]   VARCHAR (10)    NOT NULL,
    [RemitCompanyId]  INT             NOT NULL,
    [Amount]          DECIMAL (18, 2) NOT NULL,
    [Remarks]         VARCHAR (500)   NULL,
    [PostedBy]        INT             NOT NULL,
    [PostedOn]        DATETIME        NOT NULL,
    [ApproveBy]       INT             NULL,
    [ApproveOn]       DATETIME        NULL,
    [BranchId]        INT             NOT NULL,
    [Vno]             DECIMAL (18)    NULL,
    [ReceiverName]    VARCHAR (250)   NOT NULL,
    [ReceiverAddress] VARCHAR (350)   NOT NULL,
    [ReceiverContact] VARCHAR (10)    NOT NULL,
    CONSTRAINT [PK_RemitDeposit] PRIMARY KEY CLUSTERED ([RemitDepositId] ASC),
    CONSTRAINT [FK_RemitDeposit_LicenseBranch] FOREIGN KEY ([BranchId]) REFERENCES [fin].[LicenseBranch] ([BrnhID]),
    CONSTRAINT [FK_RemitDeposit_RemittanceCustomer] FOREIGN KEY ([RemitCompanyId]) REFERENCES [cust].[RemittanceCustomers] ([RID])
);

