CREATE TABLE [fin].[ProductOPSID] (
    [PID]       INT     NOT NULL,
    [PSID]      TINYINT NOT NULL,
    [IsDefault] TINYINT NOT NULL,
    CONSTRAINT [FK_ProductOPSID_PolicyIntCalc] FOREIGN KEY ([PSID]) REFERENCES [fin].[PolicyIntCalc] ([PSID]),
    CONSTRAINT [FK_ProductOPSID_ProductDetail] FOREIGN KEY ([PID]) REFERENCES [fin].[ProductDetail] ([PID])
);

