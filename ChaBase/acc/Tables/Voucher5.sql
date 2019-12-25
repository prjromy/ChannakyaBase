CREATE TABLE [acc].[Voucher5] (
    [V5Id]         INT          IDENTITY (1, 1) NOT NULL,
    [ChequeNo]     VARCHAR (50) NOT NULL,
    [ChequeDate]   DATE         NOT NULL,
    [ChequeAmount] MONEY        NOT NULL,
    [Payees]       VARCHAR (50) NULL,
    [V2Id]         INT          NOT NULL,
    CONSTRAINT [PK_ChequeInfo] PRIMARY KEY CLUSTERED ([V5Id] ASC),
    CONSTRAINT [FK_Voucher3_Voucher21] FOREIGN KEY ([V2Id]) REFERENCES [acc].[Voucher2] ([V2Id])
);

