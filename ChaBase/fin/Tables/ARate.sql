CREATE TABLE [fin].[ARate] (
    [IAccno]  INT  NOT NULL,
    [IRate]   REAL NOT NULL,
    [ARMID]   INT  NOT NULL,
    [IRateId] INT  IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_ARate_1] PRIMARY KEY CLUSTERED ([IRateId] ASC),
    CONSTRAINT [FK_ARate_ADetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno]),
    CONSTRAINT [FK_ARate_ARateMaster] FOREIGN KEY ([ARMID]) REFERENCES [fin].[ARateMaster] ([ARMID])
);

