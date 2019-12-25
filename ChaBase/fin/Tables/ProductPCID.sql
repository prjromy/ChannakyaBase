CREATE TABLE [fin].[ProductPCID] (
    [PPCId]     INT     IDENTITY (1, 1) NOT NULL,
    [PID]       INT     NOT NULL,
    [PCID]      TINYINT NOT NULL,
    [IsDefault] TINYINT NOT NULL,
    CONSTRAINT [PK_ProductPCID] PRIMARY KEY CLUSTERED ([PPCId] ASC),
    CONSTRAINT [FK_ProductPCID_PolicyPenCalc] FOREIGN KEY ([PCID]) REFERENCES [fin].[PolicyPenCalc] ([PCID]),
    CONSTRAINT [FK_ProductPCID_ProductDetail] FOREIGN KEY ([PID]) REFERENCES [fin].[ProductDetail] ([PID])
);

