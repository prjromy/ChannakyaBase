CREATE TABLE [acc].[Voucher5T] (
    [V5TId]        INT          IDENTITY (1, 1) NOT NULL,
    [ChequeNo]     VARCHAR (50) NOT NULL,
    [ChequeDate]   DATE         NOT NULL,
    [ChequeAmount] MONEY        NOT NULL,
    [Payees]       VARCHAR (50) NULL,
    [V2TId]        INT          NOT NULL,
    CONSTRAINT [PK_UnverifiedCheque] PRIMARY KEY CLUSTERED ([V5TId] ASC),
    CONSTRAINT [FK_Voucher5T_Voucher2T] FOREIGN KEY ([V2TId]) REFERENCES [acc].[Voucher2T] ([V2TId])
);

