CREATE TABLE [acc].[DimensionVSLedger] (
    [Id]     INT     IDENTITY (1, 1) NOT NULL,
    [Fid]    INT     NOT NULL,
    [DDID]   INT     NOT NULL,
    [DOrder] TINYINT NULL,
    CONSTRAINT [PK_DimensionVSLedger] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DimensionVSLedger_FinAcc] FOREIGN KEY ([Fid]) REFERENCES [acc].[FinAcc] ([Fid])
);

