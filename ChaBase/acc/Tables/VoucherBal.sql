CREATE TABLE [acc].[VoucherBal] (
    [Id]       INT   IDENTITY (1, 1) NOT NULL,
    [BranchId] INT   NOT NULL,
    [FId]      INT   NULL,
    [FYId]     INT   NULL,
    [OPBal]    MONEY NULL,
    [CLBal]    MONEY NULL,
    CONSTRAINT [PK_VoucherBal] PRIMARY KEY CLUSTERED ([Id] ASC)
);

