CREATE TABLE [acc].[Voucher3] (
    [V3Id]        INT          IDENTITY (1, 1) NOT NULL,
    [V2Id]        INT          NOT NULL,
    [SId]         INT          NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    [Amount]      DECIMAL (18) NOT NULL,
    CONSTRAINT [PK_Voucher3] PRIMARY KEY CLUSTERED ([V3Id] ASC),
    CONSTRAINT [FK_Voucher3_Voucher2] FOREIGN KEY ([V2Id]) REFERENCES [acc].[Voucher2] ([V2Id])
);

