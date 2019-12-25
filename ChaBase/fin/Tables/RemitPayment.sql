CREATE TABLE [fin].[RemitPayment] (
    [RemitPaymentId]   INT             IDENTITY (1, 1) NOT NULL,
    [RemittanceCode]   VARCHAR (10)    NOT NULL,
    [Tno]              DECIMAL (18)    NOT NULL,
    [RemitCompanyId]   INT             NOT NULL,
    [ReceiverName]     NVARCHAR (250)  NOT NULL,
    [ReceiverIdNumber] NVARCHAR (50)   NOT NULL,
    [ReceiverAddress]  NVARCHAR (250)  NOT NULL,
    [Amount]           DECIMAL (18, 2) NOT NULL,
    [BranchId]         INT             NOT NULL,
    [Vno]              DECIMAL (18)    NULL,
    [Remarks]          VARCHAR (500)   NULL,
    [PostedBy]         INT             NOT NULL,
    [PostedOn]         DATETIME        NOT NULL,
    [ApproveBy]        INT             NULL,
    [ApproveOn]        DATETIME        NULL,
    CONSTRAINT [PK_RemitPayment] PRIMARY KEY CLUSTERED ([RemitPaymentId] ASC),
    CONSTRAINT [FK_RemitPayment_LicenseBranch] FOREIGN KEY ([BranchId]) REFERENCES [fin].[LicenseBranch] ([BrnhID]),
    CONSTRAINT [FK_RemitPayment_RemittanceCustomer] FOREIGN KEY ([RemitCompanyId]) REFERENCES [cust].[RemittanceCustomers] ([RID])
);

