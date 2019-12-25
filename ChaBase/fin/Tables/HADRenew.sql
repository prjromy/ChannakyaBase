CREATE TABLE [fin].[HADRenew] (
    [ID]          INT           IDENTITY (1, 1) NOT NULL,
    [IAccNo]      INT           NULL,
    [DurationId]  INT           NULL,
    [ValDate]     SMALLDATETIME NULL,
    [MatDate]     SMALLDATETIME NULL,
    [IntCalcRule] INT           NULL,
    [IntCaptDur]  INT           NULL,
    [IntRate]     MONEY         NULL,
    [AgrAmt]      MONEY         NULL,
    [RNDate]      SMALLDATETIME NULL,
    [Basic]       INT           NULL,
    CONSTRAINT [PK_HADRenew] PRIMARY KEY CLUSTERED ([ID] ASC)
);

