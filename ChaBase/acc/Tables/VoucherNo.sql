CREATE TABLE [acc].[VoucherNo] (
    [VNId]      INT      IDENTITY (1, 1) NOT NULL,
    [BId]       INT      NOT NULL,
    [CurrentNo] INT      NOT NULL,
    [FYID]      SMALLINT NOT NULL,
    [VTypeId]   INT      NOT NULL,
    [CompanyId] INT      NULL,
    CONSTRAINT [PK_BatchCount] PRIMARY KEY CLUSTERED ([VNId] ASC),
    CONSTRAINT [FK_BatchCount_BatchDescription] FOREIGN KEY ([BId]) REFERENCES [acc].[BatchDescription] ([BId]),
    CONSTRAINT [FK_BatchCount_FiscalYear] FOREIGN KEY ([FYID]) REFERENCES [acc].[FiscalYear] ([FYID]),
    CONSTRAINT [FK_BatchCount_VoucherType] FOREIGN KEY ([VTypeId]) REFERENCES [acc].[VoucherType] ([VTypeID]),
    CONSTRAINT [FK_VoucherNo_BatchDescription] FOREIGN KEY ([BId]) REFERENCES [acc].[BatchDescription] ([BId]),
    CONSTRAINT [FK_VoucherNo_FiscalYear] FOREIGN KEY ([FYID]) REFERENCES [acc].[FiscalYear] ([FYID]),
    CONSTRAINT [FK_VoucherNo_VoucherType] FOREIGN KEY ([VTypeId]) REFERENCES [acc].[VoucherType] ([VTypeID])
);

