CREATE TABLE [fin].[ReferenceInfo] (
    [RIId]       INT     IDENTITY (1, 1) NOT NULL,
    [IAccNo]     INT     NOT NULL,
    [ReferredBy] INT     NOT NULL,
    [BroughtBy]  INT     NOT NULL,
    [RType]      TINYINT NULL,
    PRIMARY KEY CLUSTERED ([RIId] ASC),
    CONSTRAINT [FK_ReferenceInfo_ADetail] FOREIGN KEY ([IAccNo]) REFERENCES [fin].[ADetail] ([IAccno])
);

