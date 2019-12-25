CREATE TABLE [acc].[Voucher4] (
    [V4Id]   INT          IDENTITY (1, 1) NOT NULL,
    [V2Id]   INT          NOT NULL,
    [DDID]   INT          NOT NULL,
    [DVId]   INT          NULL,
    [Amount] VARCHAR (50) NULL,
    CONSTRAINT [PK_Voucher4] PRIMARY KEY CLUSTERED ([V4Id] ASC),
    CONSTRAINT [FK_Voucher4_DimensionValue] FOREIGN KEY ([DVId]) REFERENCES [acc].[DimensionValue] ([DVId]),
    CONSTRAINT [FK_Voucher4_Voucher2] FOREIGN KEY ([V2Id]) REFERENCES [acc].[Voucher2] ([V2Id])
);

