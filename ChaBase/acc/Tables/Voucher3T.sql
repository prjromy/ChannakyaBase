CREATE TABLE [acc].[Voucher3T] (
    [V3TId]       INT          IDENTITY (1, 1) NOT NULL,
    [V2TId]       INT          NOT NULL,
    [SId]         INT          NOT NULL,
    [Description] VARCHAR (50) NOT NULL,
    [Amount]      DECIMAL (18) NOT NULL,
    CONSTRAINT [PK_Voucher3T] PRIMARY KEY CLUSTERED ([V3TId] ASC),
    CONSTRAINT [FK_Voucher3T_Voucher2T] FOREIGN KEY ([V2TId]) REFERENCES [acc].[Voucher2T] ([V2TId])
);

