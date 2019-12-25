CREATE TABLE [fin].[ANomAcc] (
    [NAId]       INT IDENTITY (1, 1) NOT NULL,
    [IAccno]     INT NOT NULL,
    [NIAccno]    INT NOT NULL,
    [TransOnMat] BIT NULL,
    CONSTRAINT [PK_ANomAcc] PRIMARY KEY CLUSTERED ([NAId] ASC),
    CONSTRAINT [FK_ANomAcc_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

