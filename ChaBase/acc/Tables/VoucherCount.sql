CREATE TABLE [acc].[VoucherCount] (
    [VCountID] INT      NOT NULL,
    [VTypeID]  INT      NOT NULL,
    [FYID]     SMALLINT NOT NULL,
    [VCount]   INT      NOT NULL,
    CONSTRAINT [PK_VoucherCount] PRIMARY KEY CLUSTERED ([VCountID] ASC),
    CONSTRAINT [FK_VoucherCount_FiscalYear] FOREIGN KEY ([FYID]) REFERENCES [acc].[FiscalYear] ([FYID]),
    CONSTRAINT [FK_VoucherCount_VoucherType] FOREIGN KEY ([VTypeID]) REFERENCES [acc].[VoucherType] ([VTypeID])
);

