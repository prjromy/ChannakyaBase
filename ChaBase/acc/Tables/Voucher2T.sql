CREATE TABLE [acc].[Voucher2T] (
    [V2TId]        INT          IDENTITY (1, 1) NOT NULL,
    [Fid]          INT          NULL,
    [V1Tid]        INT          NULL,
    [Particulars]  VARCHAR (50) NULL,
    [DebitAmount]  MONEY        NULL,
    [CreditAmount] MONEY        NULL,
    CONSTRAINT [PK_Voucher2T] PRIMARY KEY CLUSTERED ([V2TId] ASC),
    CONSTRAINT [FK_Voucher2T_FinAcc] FOREIGN KEY ([Fid]) REFERENCES [acc].[FinAcc] ([Fid]),
    CONSTRAINT [FK_Voucher2T_Voucher1T] FOREIGN KEY ([V1Tid]) REFERENCES [acc].[Voucher1T] ([V1TId])
);

