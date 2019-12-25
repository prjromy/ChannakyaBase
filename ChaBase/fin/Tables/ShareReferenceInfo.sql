CREATE TABLE [fin].[ShareReferenceInfo] (
    [RIId]       INT          IDENTITY (1, 1) NOT NULL,
    [IAccNo]     NUMERIC (18) NOT NULL,
    [ReferredBy] INT          NOT NULL,
    [BroughtBy]  INT          NOT NULL,
    [RType]      TINYINT      NULL,
    CONSTRAINT [PK__ShareRef__464E990EB07CB16E] PRIMARY KEY CLUSTERED ([RIId] ASC),
    CONSTRAINT [FK_ShareReferenceInfo_ShareRegistration] FOREIGN KEY ([IAccNo]) REFERENCES [fin].[ShrReg] ([RegNo])
);

