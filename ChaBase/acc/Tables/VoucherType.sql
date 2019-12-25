CREATE TABLE [acc].[VoucherType] (
    [VTypeID]     INT          IDENTITY (1, 1) NOT NULL,
    [VoucherName] VARCHAR (50) NOT NULL,
    [Prefix]      VARCHAR (10) NOT NULL,
    CONSTRAINT [PK__VoucherT__C8097B156236C1CB] PRIMARY KEY CLUSTERED ([VTypeID] ASC)
);

