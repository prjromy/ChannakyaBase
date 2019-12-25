CREATE TABLE [fin].[ATempFloatIntRate] (
    [IAccno]         INT NOT NULL,
    [TempFloatIntId] INT NOT NULL,
    CONSTRAINT [PK_ATempIntRate] PRIMARY KEY CLUSTERED ([IAccno] ASC),
    CONSTRAINT [FK_ATempIntRate_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

