CREATE TABLE [acc].[Voucher2] (
    [V2Id]         INT          IDENTITY (1, 1) NOT NULL,
    [Fid]          INT          NULL,
    [V1id]         INT          NULL,
    [Particulars]  VARCHAR (50) NULL,
    [DebitAmount]  MONEY        NULL,
    [CreditAmount] MONEY        NULL,
    CONSTRAINT [PK__TempVouc__0023CA1BF379D072] PRIMARY KEY CLUSTERED ([V2Id] ASC),
    CONSTRAINT [FK_Voucher2_FinAcc] FOREIGN KEY ([Fid]) REFERENCES [acc].[FinAcc] ([Fid]),
    CONSTRAINT [FK_Voucher2_FinAcc1] FOREIGN KEY ([Fid]) REFERENCES [acc].[FinAcc] ([Fid]),
    CONSTRAINT [FK_Voucher2_Voucher1] FOREIGN KEY ([V1id]) REFERENCES [acc].[Voucher1] ([V1Id]),
    CONSTRAINT [FK_Voucher2_Voucher11] FOREIGN KEY ([V1id]) REFERENCES [acc].[Voucher1] ([V1Id])
);

