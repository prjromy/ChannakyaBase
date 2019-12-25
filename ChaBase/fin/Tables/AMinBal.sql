CREATE TABLE [fin].[AMinBal] (
    [IAccno] INT   NOT NULL,
    [Minbal] MONEY NOT NULL,
    CONSTRAINT [PK_AMinBal] PRIMARY KEY CLUSTERED ([IAccno] ASC),
    CONSTRAINT [FK_AccountMinBal_AccountDetail] FOREIGN KEY ([IAccno]) REFERENCES [fin].[ADetail] ([IAccno])
);

