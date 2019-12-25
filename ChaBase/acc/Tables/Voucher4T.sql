CREATE TABLE [acc].[Voucher4T] (
    [V4TId]  INT          IDENTITY (1, 1) NOT NULL,
    [V2TId]  INT          NOT NULL,
    [DDID]   INT          NOT NULL,
    [DVId]   INT          NULL,
    [Amount] VARCHAR (50) NULL,
    CONSTRAINT [PK_Voucher4T] PRIMARY KEY CLUSTERED ([V4TId] ASC),
    CONSTRAINT [FK_Voucher4T_DimensionValue] FOREIGN KEY ([DVId]) REFERENCES [acc].[DimensionValue] ([DVId]),
    CONSTRAINT [FK_Voucher4T_Voucher2T] FOREIGN KEY ([V2TId]) REFERENCES [acc].[Voucher2T] ([V2TId])
);

