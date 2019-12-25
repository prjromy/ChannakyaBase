CREATE TABLE [fin].[ProductPSID] (
    [PID]       INT     NOT NULL,
    [PSID]      TINYINT NOT NULL,
    [IsDefault] BIT     NULL,
    [PCalCId]   INT     IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ProductPSID] PRIMARY KEY CLUSTERED ([PCalCId] ASC),
    CONSTRAINT [FK_ProductPSID_PolicyIntCalc] FOREIGN KEY ([PSID]) REFERENCES [fin].[PolicyIntCalc] ([PSID]),
    CONSTRAINT [FK_ProductPSID_ProductDetail] FOREIGN KEY ([PID]) REFERENCES [fin].[ProductDetail] ([PID])
);

