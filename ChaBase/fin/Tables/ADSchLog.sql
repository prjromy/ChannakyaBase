CREATE TABLE [fin].[ADSchLog] (
    [IAccno]  INT           NOT NULL,
    [MDate]   SMALLDATETIME NOT NULL,
    [AdschId] INT           IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ADSchLog] PRIMARY KEY CLUSTERED ([AdschId] ASC)
);

