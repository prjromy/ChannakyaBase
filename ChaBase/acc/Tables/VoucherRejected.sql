CREATE TABLE [acc].[VoucherRejected] (
    [RV2Id]   INT          IDENTITY (1, 1) NOT NULL,
    [V1id]    INT          NULL,
    [Remarks] VARCHAR (50) NULL,
    CONSTRAINT [PK_VoucherRejected] PRIMARY KEY CLUSTERED ([RV2Id] ASC),
    CONSTRAINT [FK_VoucherRejected_Voucher1] FOREIGN KEY ([V1id]) REFERENCES [acc].[Voucher1] ([V1Id])
);

